using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    public class UserWithClassesDto : UserDto
    {
        public List<ClassDto> Classes { get; set; } = [];
    }
}
