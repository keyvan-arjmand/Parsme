using Domain.Common;

namespace Domain.Entity.IndexPage;

public class FooterLink:BaseEntity
{
    public string? DescCol { get; set; }
    public string? TitleCol1 { get; set; }
    public string? HrefCol1 { get; set; }
    public string? Title1Col1 { get; set; }
    public string? Href1Col1 { get; set; }
    public string? Title2Col1 { get; set; }
    public string? Href2Col1 { get; set; }
    public string? Title3Col1 { get; set; }
    public string? Href3Col1 { get; set; }
    
    public string? DescCol2 { get; set; }
    public string? TitleCol2 { get; set; }
    public string? HrefCol2 { get; set; }
    public string? Title1Col2 { get; set; }
    public string? Href1Col2 { get; set; }
    public string? Title2Col2 { get; set; }
    public string? Href2Col2 { get; set; }
    public string? Title3Col2 { get; set; }
    public string? Href3Col2 { get; set; }
    
    public string? DescCol3 { get; set; }
    public string? TitleCol3 { get; set; }
    public string? HrefCol3 { get; set; }
    public string? Title1Col3 { get; set; }
    public string? Href1Col3 { get; set; }
    public string? Title2Col3 { get; set; }
    public string? Href2Col3 { get; set; }
    public string? Title3Col3 { get; set; }
    public string? Href3Col3 { get; set; }

    public string DescFooter { get; set; } = string.Empty;
}