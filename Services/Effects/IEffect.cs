using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Models;

namespace Services.Effects
{
    public interface IEffect<T>
    {
        Task Proceed(Effect effect, ICollection<T> projects);
    }
}