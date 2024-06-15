namespace Application.Dtos.User;

public class UserDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public DateTime InsertDate { get; set; } = DateTime.Now;
    public string Address { get; set; } = string.Empty;
}