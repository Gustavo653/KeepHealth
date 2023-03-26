using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepNotes.Maui.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
    }
}
