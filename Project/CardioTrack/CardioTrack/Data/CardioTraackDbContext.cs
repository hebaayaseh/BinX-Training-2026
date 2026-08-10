using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Data
{
    public class CardioTraackDbContext : DbContext
    {
        public CardioTraackDbContext(DbContextOptions<CardioTraackDbContext> options)
        : base(options) { }

    }
}
