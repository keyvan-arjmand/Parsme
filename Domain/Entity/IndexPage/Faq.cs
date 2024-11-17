using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entity.IndexPage;

public class Faq:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public int? FaqCatId { get; set; }
    public bool ShowFirst { get; set; }
    [ForeignKey(nameof(FaqCatId))] public FaqCat? FaqCat { get; set; }
    
}