﻿using Domain.Common;

namespace Domain.Entity.Product;

public class Brand:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string LogoUri { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
 
}