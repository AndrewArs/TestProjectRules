using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Models;
using Dtos.Projects;

namespace Services.Effects
{
    public interface IEffect<T>
    {
        Task Proceed(Effect effect, ICollection<T> projects);
    }
}