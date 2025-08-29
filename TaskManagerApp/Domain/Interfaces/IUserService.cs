using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(string email, string password);
    }
}
