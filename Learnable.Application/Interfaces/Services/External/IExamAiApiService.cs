using Learnable.Application.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Services.External
{
    public interface IExamAiApiService
    {
        Task<List<ExamQuestionDto>> GenerateQuestions(List<OcrPdfDto> chunks);
    }

}

