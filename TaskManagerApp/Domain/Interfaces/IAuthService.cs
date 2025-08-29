using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateUserAsync(string email, string password);
        Task<IdentityUser?> GetUserAsync(string email, string password);
    }
}
