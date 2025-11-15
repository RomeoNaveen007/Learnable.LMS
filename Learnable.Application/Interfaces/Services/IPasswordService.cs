using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Services
{
    public interface IPasswordService
    {
        void CreatePasswordHash(string password, out string hash, out string salt);
        bool VerifyPassword(string password, string hash, string salt);
    }
}
