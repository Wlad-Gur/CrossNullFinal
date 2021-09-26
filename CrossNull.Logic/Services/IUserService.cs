using CrossNull.Logic.Models;
using CSharpFunctionalExtensions;
using System.Collections;
using System.Collections.Generic;

namespace CrossNull.Logic.Services
{
    public interface IUserService
    {
        Result AddUser(RegisterModel registerModel);
        Result ResetPassword(string email);
        Result ResetPassword(string userId, string token, string password);
        Result SendCode(string userId);
        Result VerifyEmailToken(string token, string userId);
        Result<User, ApiError> FindUserByEmail(string email);
        Result<IEnumerable<User>, ApiError> GetAllUsers();


    }
}