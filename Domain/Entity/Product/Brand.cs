﻿using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Entity.IndexPage;

namespace Domain.Entity.Product;

public class Brand:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public int? SubCategoryId { get; set; }
    [ForeignKey("SubCategoryId")] public SubCategory? SubCategory { get; set; }

}