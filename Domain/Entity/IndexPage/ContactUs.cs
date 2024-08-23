using Domain.Common;
using Domain.Enums;

namespace Domain.Entity.IndexPage;

public class ContactUs:BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Subject Subject { get; set; }
    public DateTime InsertDate { get; set; }
    
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDesc { get; set; } = string.Empty;
    public string SeoCanonical { get; set; } = string.Empty;
}