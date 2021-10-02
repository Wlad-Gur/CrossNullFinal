using CrossNull.Logic.Models;
using CSharpFunctionalExtensions;
using System.Collections;
using System.Collections.Generic;

namespace CrossNull.Logic.Services
{
    public interface IUserService
    {
        Result<User, ApiError> AddUser(UpdateUserModel updateUserModel);
        Result AddUser(RegisterModel registerModel);
        Result ResetPassword(string email);
        Result ResetPassword(string userId, string token, string password);
        Result SendCode(string userId);
        Result VerifyEmailToken(string token, string userId);
        Result<User, ApiError> FindUserByEmail(string email);
        Result<IEnumerable<User>, ApiError> GetAllUsers();
        Result<User, ApiError> ChangeWholeUser(string id, UpdateWholeUserModel updateWholeUserModel);
        Result DeleteUser(string id);
        Result<User, ApiError> PartialChange(string id, string nameProp, string valueProp);
    }
}