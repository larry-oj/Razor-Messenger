﻿using Razor_Messenger.Data.Models;

namespace Razor_Messenger.Services;

public interface IAuthService
{
    User Register(string username, string password);
    User Login(string username, string password);
    string HashPassword(string password, string salt);
    string CreateSalt(int size);
}