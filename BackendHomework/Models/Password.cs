using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BackendHomework.BusinessLogic.Auth
{
    public class Password
    {

        public Password(string hash)
        {
            PasswordHash = hash;
        }
        
        public Password(string password, Guid salt)
        {
            PasswordHash = GenerateHash(password, salt);
        }

        private string GenerateHash(string password, Guid salt)
        {
            return Convert.ToBase64String(inArray: KeyDerivation.Pbkdf2(
                password: password,
                salt: salt.ToByteArray(),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        public string PasswordHash { get; }

        public bool EqualsTo(string hash)
        {
            return PasswordHash.Equals(hash);
        }
    }
}