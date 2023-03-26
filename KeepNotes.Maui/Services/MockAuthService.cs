using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepNotes.Maui.Services
{
    public class MockAuthService : IAuthService
    {
        public Task<bool> LoginAsync(string username, string password)
        {
            return Task.FromResult(true);
        }
    }

}
