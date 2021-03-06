﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTime.Entities;
using MovieTime.ViewModels.Account;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Cryptography;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MovieTime.Services
{
    public class AccountService
    {
        private MovieTimeContext _movieTimeDb;

        public AccountService(MovieTimeContext movieTimeDb)
        {
            _movieTimeDb = movieTimeDb;
        }

        public bool IsUsernameTaken(string username)
        {
            return _movieTimeDb.User.Any(x => x.Username == username);
        }

        public void CreateAccount(AccountCreationViewModel userLogin)
        {
            var newUser = new User {
                Username = userLogin.UserName,
                PasswordHash = GetPasswordHash(userLogin.Password),
                CreateTimestamp = DateTime.Now
            };

            _movieTimeDb.Add(newUser);
            _movieTimeDb.SaveChanges();
        }

        public bool IsLoginValid(LoginViewModel userLogin)
        {
            return _movieTimeDb.User.Any(x => x.PasswordHash == GetPasswordHash(userLogin.Password) && x.Username == userLogin.Username);
        }

        private static string GetPasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashed = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashed).Replace("-", "");
            }
        }

        public ClaimsPrincipal GetUserPrincipal(string username)
        {
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var identity = new ClaimsIdentity(Claims, "login");

            // Authenticate using the identity
            return new ClaimsPrincipal(identity);
        }
    }
}
