using CSharpFunctionalExtensions;

namespace CrossNull.Logic.Services
{
    public interface IUserService
    {
        Result AddUser(RegisterModel registerModel);

    }
}