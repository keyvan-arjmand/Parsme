﻿using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.Product;

public class ProductColor : BaseEntity
{
    public double Price { get; set; }//RIT
    public int Priority { get; set; }
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int GuaranteeId { get; set; }
    public int Inventory { get; set; }
}