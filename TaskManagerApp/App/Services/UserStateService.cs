using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserStateService
    {
        public string? UserId { get; private set; }
        public string? UserName { get; private set; }

        public event Action? OnChange;

        public void SetUser(string userId, string userName)
        {
            UserId = userId;
            UserName = userName;
            NotifyStateChanged();
        }
        public string GetUser()
        {
            if (UserId == null)
                return null;
            return UserId;
        }
        public void ClearUser()
        {
            UserId = null;
            UserName = null;
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
