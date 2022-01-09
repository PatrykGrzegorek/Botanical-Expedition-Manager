using Microsoft.EntityFrameworkCore;

namespace WebApplication.Models
{
    public partial class RPPP19Context
    {
        public virtual DbSet<ViewCollection> vw_Collection { get; set; }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewCollection>(entity =>
            {
                entity.HasNoKey();
            });

        }
    }
}