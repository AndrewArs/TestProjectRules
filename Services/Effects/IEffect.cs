using Dtos.Projects;

namespace Services.Effects
{
    public interface IEffect
    {
        void Proceed(IEffect effect, ProjectDto project);
    }
}