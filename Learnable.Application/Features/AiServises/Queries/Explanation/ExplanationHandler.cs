using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Services.External;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Learnable.Application.Features.AiServises.Queries.Explanation
{
    public class ExplanationHandler : IRequestHandler<ExplanationQuerie, List<string>>
    {
        private readonly IExplanationService _explanationService;

        // Constructor injection
        public ExplanationHandler(IExplanationService explanationService)
        {
            _explanationService = explanationService;
        }

        public async Task<List<string>> Handle(ExplanationQuerie request, CancellationToken cancellationToken)
        {
            // input eduthu service call panna
            var result = await _explanationService.ExplainText(request.Input);

            // result return panna
            return result;
        }
    }
}
