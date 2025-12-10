using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace TO_Do_List.Model
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Book> Book { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            // ===== Tabla: book =====
            mb.Entity<Book>(e =>
            {
                e.ToTable("book", "dbo");

                e.HasKey(x => x.IdB);

                e.Property(x => x.IdB)
                    .ValueGeneratedOnAdd();

                e.Property(x => x.IdUserFK)
                    .IsRequired();

                e.Property(x => x.TitleB)
                    .HasMaxLength(150)
                    .IsRequired()
                    .IsUnicode(false);

                e.Property(x => x.DescriptionB)
                    .HasColumnType("varchar(max)")
                    .IsUnicode(false);

                e.Property(x => x.StatusB)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                e.Property(x => x.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");

                e.Property(x => x.UpdateDate)
                    .HasColumnType("datetime");

                // Relación FK con Users
                e.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(x => x.IdUserFK)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== Tabla: Users =====
            mb.Entity<User>(e =>
            {
                e.ToTable("Users", "dbo");

                e.HasKey(x => x.IdU);

                e.Property(x => x.IdU)
                    .ValueGeneratedOnAdd();

                e.Property(x => x.NameU)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                e.Property(x => x.EmailU)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                e.HasIndex(x => x.EmailU)
                    .IsUnique();

                e.Property(x => x.PasswordU)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                e.Property(x => x.StatusU)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                e.Property(x => x.DateU)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
            });
        }


    }
}
