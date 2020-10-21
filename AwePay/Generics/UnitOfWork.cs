using AwePay.EF;
using AwePay.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Generics
{
    public interface IUnitOfWork
    {
        /// ADD ALL YOUR REPOSITORIES HERE

        void Commit();
        Task CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbCtx _context;
        //  private readonly DbCon _dbcon;


        public UnitOfWork(DbCtx context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// ADD ALL YOUR REPOSITORIES HERE
        /// 
        public UserRepo UserRepo => new UserRepo(_context) ?? throw new ArgumentNullException(nameof(UserRepo));




        //public ExchangeRepo ExchangeRepo => new ExchangeRepo(_context, _dbcon) ?? throw new ArgumentNullException(nameof(ExchangeRepo));
        //public OrderRepo OrderRepo => new OrderRepo(_context) ?? throw new ArgumentNullException(nameof(OrderRepo));


        public void Commit() => _context.SaveChanges();

        public IDbContextTransaction Transaction() => _context.Database.BeginTransaction();

        public async Task CommitAsync() => await _context.SaveChangesAsync();
    }
}
