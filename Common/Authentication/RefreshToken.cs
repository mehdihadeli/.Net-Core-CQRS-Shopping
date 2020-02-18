using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Common.Authentication
{
    public class RefreshToken
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public bool Revoked => RevokedAt.HasValue;
        public bool IsAlive => DateTime.Now <= Expires;

        public DateTime Expires { get; private set; }

        protected RefreshToken()
        {
        }

        public void ValidateRefreshToken()
        {
            if (Revoked)
            {
                throw new Exception($"Refresh token: '{Id}' already revoked.");
            }  
            
            if (IsAlive == false)
            {
                throw new Exception($"Refresh token: '{Id}' is expired.");
            }
        }
        public RefreshToken(int userId, long dayToExpire = 3)
        {
            UserId = userId;
            CreatedAt = DateTime.Now;
            Token = GenerateToken();
            Expires = DateTime.Now.AddDays(dayToExpire);
        }

        private string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public void Revoke()
        {
            if (Revoked)
            {
                throw new Exception($"Refresh token: '{Id}' was already revoked at '{RevokedAt}'.");
            }

            RevokedAt = DateTime.Now;
        }
    }
}