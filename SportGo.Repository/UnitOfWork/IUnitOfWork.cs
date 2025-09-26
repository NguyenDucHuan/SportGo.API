using Microsoft.EntityFrameworkCore;
using SportGo.Repository.Repositories;


namespace SportGo.Repository.UnitOfWork;
public interface IUnitOfWork : IGenericRepositoryFactory, IDisposable
{
    int Commit();

    Task<int> CommitAsync();

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}

public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    TContext Context { get; }
}

