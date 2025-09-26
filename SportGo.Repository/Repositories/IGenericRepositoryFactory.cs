namespace SportGo.Repository.Repositories;

public interface IGenericRepositoryFactory
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}

