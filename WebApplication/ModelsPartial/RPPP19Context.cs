using Microsoft.EntityFrameworkCore;

namespace WebApplication.Models
{
    public partial class RPPP19Context
    {

        public virtual DbSet<ViewHerbariumInfo> vw_Herbarium { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViewHerbariumInfo>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}