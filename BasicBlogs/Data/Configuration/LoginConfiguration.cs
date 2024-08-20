using BasicBlogs.Entities;
using BasicBlogs.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicBlogs.Data.Configuration
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("UserLogin");

            builder.HasKey(ua => ua.Id);

            builder.Property(ua => ua.UserNameOrEmail)
                .HasColumnName("username")
                .HasMaxLength(255)
                .IsRequired();

           

            builder.Property(ua => ua.Password)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(ua => ua.Role)
                .HasColumnName("role")
                .HasMaxLength(50)
                .IsRequired();

            // Seeding the admin user
            builder.HasData(
                new UserLogin
                {
                    Id = 1,
                    UserNameOrEmail = "admin@gmail.com",
                    Password = "admin",
                    Role = "admin"
                });
        }
    }
}
