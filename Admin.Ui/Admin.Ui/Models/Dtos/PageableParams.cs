namespace Admin.Ui.Models.Dtos;

public class PageableParams
{
    public int Page { get; set; } = 1;
    public int TotalPage { get; set; } = 0;
    public int PageSize { get; set; } = 8;
    public string? Search { get; set; }
}