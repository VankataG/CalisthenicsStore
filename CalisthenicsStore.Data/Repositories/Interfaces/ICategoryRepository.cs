using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalisthenicsStore.Data.Models;

namespace CalisthenicsStore.Data.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, Guid>, IAsyncRepository<Category, Guid>
    {

    }
}
