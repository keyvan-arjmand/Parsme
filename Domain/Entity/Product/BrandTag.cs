﻿using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entity.IndexPage;

namespace Domain.Entity.Product;

public class BrandTag : BaseEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public bool IsClick { get; set; }
    public ICollection<Product> Products { get; set; } = default!;
    public int? BrandLandingId { get; set; }
    [ForeignKey("BrandLandingId")] public BrandLanding? BrandLanding { get; set; }
}