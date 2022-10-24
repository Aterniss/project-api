using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Project_API.Models
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Dish> Dishes { get; set; } = null!;
        public virtual DbSet<FoodCategory> FoodCategories { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDish> OrderDishes { get; set; } = null!;
        public virtual DbSet<Restaurant> Restaurants { get; set; } = null!;
        public virtual DbSet<Rider> Riders { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Zone> Zones { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("accounts");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email_address");

                entity.Property(e => e.IdUsers).HasColumnName("id_users");

                entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.TelNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tel_number");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_password");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_accounts_roles");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("dishes");

                entity.Property(e => e.DishId).HasColumnName("dish_id");

                entity.Property(e => e.DishDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("dish_description");

                entity.Property(e => e.DishName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("dish_name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Require18).HasColumnName("require_18");

                entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dishes_restaurants");
            });

            modelBuilder.Entity<FoodCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryName);

                entity.ToTable("food_category");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("category_name");

                entity.Property(e => e.CategoryDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("category_description");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("order_status");

                entity.Property(e => e.RiderId).HasColumnName("rider_id");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orders_users");

                entity.HasOne(d => d.Rider)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RiderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_orders_riders1");
            });

            modelBuilder.Entity<OrderDish>(entity =>
            {
                entity.ToTable("order_dishes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DishId).HasColumnName("dish_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.OrderDishes)
                    .HasForeignKey(d => d.DishId)
                    .HasConstraintName("FK_order_dishes_dishes");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDishes)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_order_dishes_orders");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("restaurants");

                entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("category_name");

                entity.Property(e => e.RestaurantAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("restaurant_address");

                entity.Property(e => e.RestaurantName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("restaurant_name");

                entity.Property(e => e.ZoneId).HasColumnName("zone_id");

                entity.HasOne(d => d.CategoryNameNavigation)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.CategoryName)
                    .HasConstraintName("FK_restaurants_food_category");

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_restaurants_zones");
            });

            modelBuilder.Entity<Rider>(entity =>
            {
                entity.ToTable("riders");

                entity.Property(e => e.RiderId).HasColumnName("rider_id");

                entity.Property(e => e.RiderName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("rider_name");

                entity.Property(e => e.ZoneId).HasColumnName("zone_id");

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.Riders)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_riders_zones");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("users");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("full_name");

                entity.Property(e => e.IsOver18).HasColumnName("is_over_18");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_update");

                entity.Property(e => e.UserAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_address");
            });

            modelBuilder.Entity<Zone>(entity =>
            {
                entity.ToTable("zones");

                entity.Property(e => e.ZoneId).HasColumnName("zone_id");

                entity.Property(e => e.ZoneName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("zone_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
