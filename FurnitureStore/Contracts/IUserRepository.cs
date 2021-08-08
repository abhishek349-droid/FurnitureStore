using FurnitureStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Contracts
{
    public interface IUserRepository
    {
        IEnumerable<UsersModel> GetUsers();
        UsersModel GetUser(int userId);
        bool RegisterUser(UsersModel user);
        bool UpdateUser(int id, UsersModel user);
        bool DeleteUser(int userId);
    }
}
