using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TsnEducation2024.Models
{
    public partial class TsnSampleEducationContext : DbContext
    {
        public TsnSampleEducationContext()
            : base("name=TsnSampleEducationContext")
        {
        }

        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.LOGIN_PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.REMARKS)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.IS_DELETE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.INSERT_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.UPDATE_ID)
                .IsUnicode(false);
        }
    }
}
