using AwePay.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AwePay.Generics;
using AwePay.Domains;
using AwePay.EF;

namespace AwePay.Repositories
{
    public class UserRepo : Repository<User>
    {

        private readonly DbCtx _context;
        public UserRepo(DbCtx context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        public async Task<DRQueRes<User>> GetList(PageQuery @params)
        {

            var query = _context.Users.AsNoTracking();
            //.Where(x => x.Status == RecordStatus.ENABLED);

            if (!string.IsNullOrEmpty(@params.SearchBy))
            {
                query = query.Where($"{@params.SearchCol}.Contains(@0)", @params.SearchBy);
            }
            var result = await query.OrderBy($"{@params.SortBy} {@params.SortDir}").Skip(@params.PageOff).Take(@params.PageSize).ToListAsync();

            var queres = new DRQueRes<User>(query, result);

            return queres;
        }
    }

}
