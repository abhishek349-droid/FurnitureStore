using FurnitureStore.Contracts;
using FurnitureStore.Entity;
using FurnitureStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureStore.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UsersRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                var user = _db.User.Where(f => f.UserId == userId).FirstOrDefault();
                if (user != null)
                {
                    _db.User.Remove(user);
                    var result = _db.SaveChanges();
                    return result >= 1 ? true : false;
                }
                throw new Exception("No user with provided ID");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public UsersModel GetUser(int userId)
        {
            try
            {
                var user = _db.User.Where(f => f.UserId == userId).FirstOrDefault();
                if (user != null) { return user; }
                throw new Exception("No user with the provided ID");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<UsersModel> GetUsers()
        {
            try
            {
                var users = _db.User.ToList();
                if (users.Count > 0) { return users; }
                throw new Exception("No users to display");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool RegisterUser(UsersModel user)
        {
            try
            {
                _db.User.Add(user);
                var result = _db.SaveChanges();
                return result >= 1 ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool UpdateUser(int id, UsersModel user)
        {
            try
            {
                var userToBeUpdated = _db.User.Where(f => f.UserId == id).FirstOrDefault();
                if (userToBeUpdated != null)
                {
                    userToBeUpdated.Username = user.Username;
                    userToBeUpdated.Password = user.Password;
                    userToBeUpdated.Email = user.Email;
                    userToBeUpdated.Admin = user.Admin;
                    _db.User.Update(userToBeUpdated);
                    var result = _db.SaveChanges();
                    return result >= 1 ? true : false;
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
