using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection;
using CashierSystem.Models;

public class KeyManager : Microsoft.AspNetCore.DataProtection.KeyManagement.IKeyManager, ICustomKeyManager
{
    private readonly ApplicationDbContext _context;
    private readonly IDataProtector protector;


    public KeyManager(ApplicationDbContext context,IDataProtectionProvider dataProtectionProvider)
    {
        _context = context;
        protector = dataProtectionProvider.CreateProtector("6661%121*1339jau9");

    }

    public IKey CreateNewKey(DateTimeOffset activationDate, DateTimeOffset expirationDate)
    {
        var key = new ProtectionKey
        {
            KeyId = Guid.NewGuid(),
            CreationDate = DateTimeOffset.Now,
            ActivationDate = activationDate,
            ExpirationDate = expirationDate,
            IsRevoked = false
        };

        _context.ProtectionKeys.Add(key);
        _context.SaveChanges();

        return key;
    }

    public IReadOnlyCollection<IKey> GetAllKeys()
    {
        return _context.ProtectionKeys
           .OrderByDescending(k => k.CreationDate)
           .Cast<IKey>()
           .ToList()
           .AsReadOnly();
    }

    public CancellationToken GetCacheExpirationToken()
    {
        return CancellationToken.None;
    }

    public string GetKey()
    {
        var key = _context.ProtectionKeys.FirstOrDefault();
        return key?.Value ?? throw new InvalidOperationException("Key not found.");
    }

    public string Protect(string input)
    {
        return protector.Protect(input);
    }

    public string Unprotect(string input)
    {
        return protector.Unprotect(input);
    }

    public void RevokeAllKeys(DateTimeOffset revocationDate, string? reason = null)
    {
        foreach (var key in _context.ProtectionKeys)
        {
            key.IsRevoked = true;
            key.RevocationDate = revocationDate;
            key.RevocationReason = reason;
        }
        _context.SaveChanges();
    }

    public void RevokeKey(Guid keyId, string? reason = null)
    {
        var key = _context.ProtectionKeys.SingleOrDefault(k => k.KeyId == keyId);
        if (key != null)
        {
            key.IsRevoked = true;
            key.RevocationDate = DateTimeOffset.Now;
            key.RevocationReason = reason;
            _context.SaveChanges();
        }
    }

    public void SetKey(string key)
    {
        var existingKey = _context.ProtectionKeys.FirstOrDefault();
        if (existingKey != null)
        {
            existingKey.Value = key;
        }
        else
        {
            _context.ProtectionKeys.Add(new ProtectionKey { Value = key });
        }

        _context.SaveChanges();
    }

    
}
