using Domain.Enums;
using MediatR;

namespace Application.Products.Commands;

public class InsertProductCommand:IRequest
{
  public Root Product { get; set; }
  public int UserId { get; set; }
}