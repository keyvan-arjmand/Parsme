namespace Panel.Models;

public class IndexReport
{
    public int Year { get; set; } // سال
    public int Month { get; set; } // ماه
    public double TotalAmount { get; set; } // مبلغ کل فاکتورها
    public double TotalWithTaxAmount { get; set; } // مبلغ کل فاکتورها
    public double TotalProfit { get; set; } // سود کل فاکتورها
    public double TotalTax { get; set; } // مالیات کل فاکتورها
}