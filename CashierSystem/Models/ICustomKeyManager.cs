using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CashierSystem.Models
{
    public interface ICustomKeyManager : IKeyManager
    {
        string Protect(string input);
        string Unprotect(string input);
        new void SetKey(string key);
    }
}
