using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels.Models;
using Dtos.Projects;
using Microsoft.Extensions.Options;
using Services.Options;

namespace Services.Effects
{
    public class SmtpEffect : IEffect<ProjectDto>
    {
        private readonly SmtpOptions _smtpOptions;

        public SmtpEffect(IOptions<SmtpOptions> options)
        {
            _smtpOptions = options.Value;
        }

        public async Task Proceed(Effect effect, ICollection<ProjectDto> projects)
        {

            throw new System.NotImplementedException();
        }
    }
}
