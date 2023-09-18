using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.WorkerIntercator.Interactors
{
    public interface IServiceIntercator<T>
    {
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(long id);
    }
}
