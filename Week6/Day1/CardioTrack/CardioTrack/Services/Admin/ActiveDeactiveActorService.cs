using CardioTrack.Data;
using CardioTrack.DTOs.Admin;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAdmin;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Admin
{
    public class ActiveDeactiveActorService : IActiveDeactive
    {
        private readonly CardioTrackDbContext dbContext;
        public ActiveDeactiveActorService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<string> ActiveActor(int userId, ActiveDeactiveDto Actor)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("");

            var actor = await dbContext.users
                .FirstOrDefaultAsync(a=>a.Id == Actor.ActorId
                                     && !a.IsActive);

            if (actor == null)
                throw new BadRequestException("Actor Not Found!");

            actor.IsActive = true;
            await dbContext.SaveChangesAsync();
            return "تم تفعيل الحساب بنجاح.";

        }

        public async Task<string> DeactiveActor(int userId , ActiveDeactiveDto Actor)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("Auth Unothoeized");

            var actor = await dbContext.users
                .FirstOrDefaultAsync(a => a.Id == Actor.ActorId
                                     && a.IsActive);

            if (actor == null)
                throw new BadRequestException("Actor Not Found!");

            actor.IsActive = false;
            await dbContext.SaveChangesAsync();
            return "تم تعطيل الحساب بنجاح.";
        }
    }
}
