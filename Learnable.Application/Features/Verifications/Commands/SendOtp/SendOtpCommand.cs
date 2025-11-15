using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Verifications.Commands.SendOtp
{
    public record SendOtpCommand(string Email) : IRequest<bool>;
}
