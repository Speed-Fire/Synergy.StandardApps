using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.WorkerInteractor.Interactors
{
    public interface IServiceInteractor<T>
    {
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(long id);
        Task Enable(T entity);
    }
}
