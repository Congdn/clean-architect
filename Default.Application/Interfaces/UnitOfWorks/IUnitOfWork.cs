using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(bool ensureAutoHistory = false);
    }
}
