using Learnable.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Services.External
{
    public interface IExplanationService
    {
        Task<List<string>> ExplainText(ExplainDto dto);
    }
}
