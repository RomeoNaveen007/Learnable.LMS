using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.AiServises.Queries.Explanation
{
    public class ExplanationQuerie : IRequest<List<string>>
    {
        public ExplainDto? Input { get; set; }
    }
}
