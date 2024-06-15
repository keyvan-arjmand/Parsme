namespace Application.Dtos.Factor;

public class FactorDto
{
    public long Id { get; set; }
    // public UserLoginDto User { get; set; } = new();
    public DateTime DateTime { get; set; }
    public ICollection<ProductFactorDto> Products { get; set; } = default!;
    public string Address { get; set; } = string.Empty;
}