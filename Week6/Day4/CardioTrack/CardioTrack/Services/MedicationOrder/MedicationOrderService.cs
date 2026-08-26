using CardioTrack.Data;
using CardioTrack.DTOs.MedicationOrder;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAuditLog;
using CardioTrack.Interfaces.IMedicationOrder;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CardioTrack.Services.MedicationOrder
{
    public class MedicationOrderService : IMedicationOrder
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IAuditLog log;
        public MedicationOrderService(CardioTrackDbContext dbContext, IAuditLog log)
        {
            this.dbContext = dbContext;
            this.log = log;
        }

        public async Task<CreateMedicationOrderResponseDto> CreateOrderAsync(int userId, CreateMedicationOrderRequestDto request)
        {
            var orderingUser = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && (u.Role == UserRole.Doctor || u.Role == UserRole.Nurse));

            if (orderingUser == null)
                throw new ForbiddenException("Auth forbidden");

            var patient = await dbContext.patients
                .FirstOrDefaultAsync(p => p.Id == request.PatientId);

            if (patient == null)
                throw new BadRequestException("Patient not found");

            if (request.Items == null || !request.Items.Any())
                throw new BadRequestException("يجب إضافة دواء واحد على الأقل للطلب");

            var requestedQuantities = request.Items
                .GroupBy(i => i.PharmacyStockId)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

            IDbContextTransaction? transaction = dbContext.Database.IsRelational()
                ? await dbContext.Database.BeginTransactionAsync()
                : null;

            try
            {
                var order = new Models.MedicationOrder
                {
                    PatientId = patient.Id,
                    OrderedByUserId = orderingUser.Id,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    TotalAmount = 0m
                };

                await dbContext.medicationOrders.AddAsync(order);
                await dbContext.SaveChangesAsync(); // needed to obtain order.Id for the items below

                decimal orderTotal = 0m;
                var responseItems = new List<OrderItemResponseDto>();

                foreach (var (stockId, quantity) in requestedQuantities)
                {
                    var stock = await dbContext.pharmacyStocks
                        .FirstOrDefaultAsync(s => s.Id == stockId);

                    if (stock == null)
                        throw new NotFoundException($"Medication with id {stockId} not found in stock");

                    if (stock.QuantityAvailable < quantity)
                        throw new ConflictException($"Insufficient stock for '{stock.DrugName}'. Available: {stock.QuantityAvailable}, requested: {quantity}");

                    decimal lineTotal = stock.UnitPrice * quantity;
                    orderTotal += lineTotal;

                    var orderItem = new MedicationOrderItem
                    {
                        MedicationOrderId = order.Id,
                        PharmacyStockId = stock.Id,
                        Quantity = quantity,
                        UnitPrice = stock.UnitPrice,
                        LineTotal = lineTotal
                    };

                    stock.QuantityAvailable -= quantity;
                    stock.UpdatedAt = DateTime.UtcNow;

                    await dbContext.medicationOrderItems.AddAsync(orderItem);

                    responseItems.Add(new OrderItemResponseDto
                    {
                        PharmacyStockId = stock.Id,
                        DrugName = stock.DrugName,
                        Quantity = quantity,
                        UnitPrice = stock.UnitPrice,
                        LineTotal = lineTotal
                    });
                }

                order.TotalAmount = orderTotal;

                await log.LogAsync("Create Medication Order", "MedicationOrder", order.Id, null, order);
                await dbContext.SaveChangesAsync();

                if (transaction != null)
                    await transaction.CommitAsync();

                return new CreateMedicationOrderResponseDto
                {
                    OrderId = order.Id,
                    PatientId = patient.Id,
                    PatientName = patient.FullName,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    Items = responseItems
                };
            }
            catch
            {
                if (transaction != null)
                    await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                if (transaction != null)
                    await transaction.DisposeAsync();
            }
        }
    }
}