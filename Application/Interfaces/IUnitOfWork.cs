using Domain.Common;

namespace Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<T> GenericRepository<T>() where T : BaseEntity;
}