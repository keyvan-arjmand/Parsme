﻿using Domain.Entity.Factor;
using Domain.Entity.Factor.Product;
using Domain.Entity.IndexPage;
using Domain.Entity.Notif;
using Domain.Entity.Product;
using Domain.Entity.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain.DataBase;

public class AppDbContext : IdentityDbContext<User, Role, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

//"Data Source=185.165.118.72,1433;Initial Catalog=Develop;User ID=key1;Password=7Dwuv15@;Trust Server Certificate=True"
//"Data Source=DESKTOP-M202FR8\KEY1;Initial Catalog=Parsme;Integrated Security=True;Trust Server Certificate=True"
    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Data Source=185.165.118.72;Initial Catalog=NewPars;User ID=pars;Password=I$w225am;Trust Server Certificate=True"
        );
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Brand> Brands { set; get; }
    public DbSet<Category> Categories { set; get; }
    public DbSet<CategoryDetail> CategoryDetails { set; get; }
    public DbSet<Color> Colors { set; get; }
    public DbSet<DiscountCode> DiscountCodes { set; get; }
    public DbSet<Feature> Features { set; get; }
    public DbSet<Guarantee> Guarantees { set; get; }
    public DbSet<ImageGallery> ImageGalleries { set; get; }
    public DbSet<Product> Products { set; get; }
    public DbSet<ProductColor> ProductColors { set; get; }
    public DbSet<ProductDetail> ProductDetails { set; get; }
    public DbSet<SubCategory> SubCategories { set; get; }
    public DbSet<State> States { set; get; }
    public DbSet<City> Cities { set; get; }
    public DbSet<Offer> Offers { set; get; }
    public DbSet<UserAddress> Addresses { set; get; }
    public DbSet<UserFav> UserFavs { set; get; }
    public DbSet<Banner> Banners { set; get; }
    public DbSet<PostMethod> PostMethods { set; get; }
    public DbSet<SaleServices> SaleServices { set; get; }
    public DbSet<SearchResult> SearchResults { set; get; }
    public DbSet<Faq> Faqs { set; get; }
    public DbSet<ContactUs> ContactUs { set; get; }
    public DbSet<Factor> Factors { set; get; }
    public DbSet<FactorProduct> FactorProducts { set; get; }
    public DbSet<FactorProductColor> FactorProductColors { set; get; }
    public DbSet<LogFactor> LogFactors { set; get; }
    public DbSet<LogReturnedFactor> LogReturnedFactors { set; get; }
    public DbSet<ReturnedFactor> ReturnedFactors { set; get; }
    public DbSet<ContactPage> ContactPages { set; get; }
    public DbSet<AboutUsPage> AboutUsPages { set; get; }
    public DbSet<InventoryNotification> InventoryNotifications { set; get; }
    public DbSet<FooterLink> FooterLinks { set; get; }
    public DbSet<BrandLanding> BrandLandings { set; get; }
    public DbSet<SeoPage> SeoPage { set; get; }
    public DbSet<FooterPage> FooterPages { set; get; }
    public DbSet<SubCategoryDetail> SubCategoryDetails { set; get; }
    public DbSet<FaqCat> FaqCats { set; get; }
    public DbSet<BrandTag> BrandTags { set; get; }
    public DbSet<BankTransaction> BankTransactions { set; get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Brand>().HasQueryFilter(x => !x.IsDelete&&x.IsActive);
        // modelBuilder.Entity<BrandTag>().HasQueryFilter(x => !x.IsDelete);
        // modelBuilder.Entity<BrandLanding>().HasQueryFilter(x => !x.IsDelete);
        // modelBuilder.Entity<UserAddress>().HasQueryFilter(x => !x.IsDelete);
        // modelBuilder.Entity<MainCategory>().HasQueryFilter(x => !x.IsDelete && x.IsActive);
        // modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDelete && x.IsActive);
        // modelBuilder.Entity<CategoryDetail>().HasQueryFilter(x => !x.IsDelete && x.IsActive);
        // modelBuilder.Entity<Color>().HasQueryFilter(x => !x.IsDelete);
        // modelBuilder.Entity<DiscountCode>().HasQueryFilter(x => !x.IsDelete);
        // modelBuilder.Entity<Feature>().HasQueryFilter(x => !x.IsDelete && x.IsActive);
        // modelBuilder.Entity<Guarantee>().HasQueryFilter(x => !x.IsDelete);
        // modelBuilder.Entity<ImageGallery>().HasQueryFilter(x => !x.IsDelete);
        // modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDelete && x.IsActive);
        // modelBuilder.Entity<ProductColor>().HasQueryFilter(x => !x.IsDelete);
        // modelBuilder.Entity<ProductDetail>().HasQueryFilter(x => !x.IsDelete);
        // modelBuilder.Entity<SubCategory>().HasQueryFilter(x => !x.IsDelete && x.IsActive);
        // modelBuilder.Entity<State>().HasQueryFilter(x => !x.IsDelete);
        // modelBuilder.Entity<City>().HasQueryFilter(x => !x.IsDelete);

       modelBuilder.Entity<Brand>().HasQueryFilter(x => !x.IsDelete);
       modelBuilder.Entity<BrandTag>().HasQueryFilter(x => !x.IsDelete);
       modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDelete );
       modelBuilder.Entity<CategoryDetail>().HasQueryFilter(x => !x.IsDelete );
       modelBuilder.Entity<MainCategory>().HasQueryFilter(x => !x.IsDelete);
       modelBuilder.Entity<Color>().HasQueryFilter(x => !x.IsDelete);
       modelBuilder.Entity<DiscountCode>().HasQueryFilter(x => !x.IsDelete);
       modelBuilder.Entity<Feature>().HasQueryFilter(x => !x.IsDelete );
       modelBuilder.Entity<Guarantee>().HasQueryFilter(x => !x.IsDelete);
       modelBuilder.Entity<ImageGallery>().HasQueryFilter(x => !x.IsDelete);
       modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDelete );
       modelBuilder.Entity<ProductColor>().HasQueryFilter(x => !x.IsDelete);
       modelBuilder.Entity<ProductDetail>().HasQueryFilter(x => !x.IsDelete);
       modelBuilder.Entity<SubCategory>().HasQueryFilter(x => !x.IsDelete );
       modelBuilder.Entity<State>().HasQueryFilter(x => !x.IsDelete);
       modelBuilder.Entity<City>().HasQueryFilter(x => !x.IsDelete);

        base.OnModelCreating(modelBuilder);
    }
}