using ETicaretAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories
{
    public class CustomerReadRepository: ReadRepository<Customer>, ICustomerReadRepository
    {
        public CustomerReadRepository(ETicaretAPIDbContext context) : base(context) { }
    }
}
