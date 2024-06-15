namespace WebApp.Models;

public class SearchHistory
{
    public int Id { get; set; }
    public string Search { get; set; } = string.Empty;
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
    public int Page { get; set; }
}