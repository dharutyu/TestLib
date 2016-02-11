using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsDal
{
    public class EntityBase
    {
    }

    public interface IRepository<TEntity>
        where TEntity : EntityBase
    {
        string TestFn();
    }
    public interface IService<TEntity>
        where TEntity : EntityBase
    {
        string TestSvcFn();
    }

    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() 
            where TEntity : EntityBase;
        void Save();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
            repositories = new Hashtable();
            context = new DbContext("");
        }
        private Hashtable repositories;
        private DbContext context;
        public IRepository<TEntity> Repository<TEntity>() 
            where TEntity : EntityBase
        {

            if (repositories == null)
                repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(TEntity)), context);

                repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity>)repositories[type];

        }
        public void Save()
        {
            throw new NotImplementedException();
        }
    }
    public abstract class Service<TEntity> : IService<TEntity>
        where TEntity : EntityBase
    {
        protected internal readonly IUnitOfWork uow;
        protected internal readonly IRepository<TEntity> repo;
        protected Service(IUnitOfWork uow)
        {
            this.uow = uow;
            this.repo = uow.Repository<TEntity>();

        }
        public string TestSvcFn()
        {
            throw new NotImplementedException();
        }
    }


    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
    {

        protected DbSet<TEntity> dbSet;
        protected DbContext context;
        public Repository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public string TestFn()
        {
            return "ok";
        }
    }


    public class TestEntity : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestService : Service<TestEntity>
    {
        public TestService(IUnitOfWork uow) : base(uow)
        {
            
        }

        public void TestFn()
        {
        }
    }

}
