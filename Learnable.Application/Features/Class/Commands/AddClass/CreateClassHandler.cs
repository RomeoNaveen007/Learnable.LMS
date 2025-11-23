using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.AddClass
{
    public class CreateClassHandler : IRequestHandler<CreateClassCommand, ClassDto>
    {
        private readonly IGenericRepository<Learnable.Domain.Entities.Class> _classRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateClassHandler(
            IGenericRepository<Learnable.Domain.Entities.Class> classRepository,
            IUnitOfWork unitOfWork)
        {
            _classRepository = classRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClassDto> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            var newClass = new Learnable.Domain.Entities.Class
            {
                ClassId = Guid.NewGuid(),
                ClassName = request.ClassDto.ClassName,
                ClassJoinName = request.ClassDto.ClassJoinName,
                Description = request.ClassDto.Description,
                TeacherId = request.ClassDto.TeacherId,
                Status = request.ClassDto.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _classRepository.CreateAsync(newClass);

         
            await _unitOfWork.SaveChangesAsync();

        
            return newClass.ToDto();
        }

    }

}

