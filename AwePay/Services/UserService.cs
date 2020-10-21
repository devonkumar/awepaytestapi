using AwePay.Domains;
using AwePay.EF;
using AwePay.Generics;
using AwePay.Helpers;
using AwePay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Services
{
    public interface IUserService
    {

        User AuthUser(string username, string password);
        Task<DRQueRes<User>> GetAllUsersAsync(PageQuery @qry);

        Task<User> GetUserByIdAsync(int id);

        User CreateUser(User user, string password);
        Task UpdateUser(User user, string password = null, bool chpass = false);

        Task DeleteUser(long id);


    }


    public class UserService : IUserService
    {

        //private DbCtx _context;
        private readonly Func<DbCtx> _factory;
        private readonly DbCon _dbcon;

        public Func<DbCtx> Factory => _factory;

        public UserService(Func<DbCtx> factory, DbCon dbcon)
        {
            _dbcon = dbcon ?? throw new ArgumentNullException(nameof(dbcon));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }


        public User AuthUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            using (var DbCtx = Factory.Invoke())
            {
                var uow = new UnitOfWork(DbCtx);

                var user = uow.UserRepo.Single(x => x.UName == username);

                if (user == null)
                    return null;

                if (!VerifyPasswordHash(password, user.PHash, user.PSalt))
                    return null;

                return user;

            }
        }

        public async Task<DRQueRes<User>> GetAllUsersAsync(PageQuery @qry)
        {
            using (var DbCtx = Factory.Invoke())
            {
                var uow = new UnitOfWork(DbCtx);
                var userList = await uow.UserRepo.GetList(@qry);
                return userList;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            using (var DbCtx = Factory.Invoke())
            {
                var uow = new UnitOfWork(DbCtx);
                var user = await uow.UserRepo.Get(id);
                return user;
            }
        }

        public User CreateUser(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            using (var DbCtx = Factory.Invoke())
            {
                var uow = new UnitOfWork(DbCtx);

                if (uow.UserRepo.Any(x => x.UName == user.UName))
                {
                    throw new AppException("Username \"" + user.UName + "\" is already taken");

                }

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PHash = passwordHash;
                user.PSalt = passwordSalt;
                user.RoleCSV = "Admin";

                uow.UserRepo.Add(user);
                uow.Commit();

                return user;
            }
        }


        public async Task UpdateUser(User userParam, string password = null, bool chpass = false)
        {
            using (var DbCtx = Factory.Invoke())
            {
                var uow = new UnitOfWork(DbCtx);

                var user = await uow.UserRepo.Get(userParam.Id);

                if (user == null)
                    throw new AppException("User not found");

                if (userParam.UName != user.UName)
                {
                    // username has changed so check if the new username is already taken
                    if (uow.UserRepo.Any(x => x.UName == userParam.UName))
                        throw new AppException("Username " + userParam.UName + " is already taken");
                }

                // update user properties


                user.FName = userParam.FName;
                user.LName = userParam.LName;
                user.UName = userParam.UName;
                user.En = userParam.En;
                user.Email = userParam.Email;
                user.Phone = userParam.Phone;

                // update password if it was entered
                if (chpass && !string.IsNullOrWhiteSpace(password))
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    user.PHash = passwordHash;
                    user.PSalt = passwordSalt;
                }

                uow.UserRepo.Update(user);
                uow.Commit();

            }

        }

        public async Task DeleteUser(long id)
        {
            using (var DbCtx = Factory.Invoke())
            {
                var uow = new UnitOfWork(DbCtx);
                var member = await uow.UserRepo.Get(id);

                if (member != null)
                {
                    uow.UserRepo.Remove(member);
                    uow.Commit();
                }


            }
        }



        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }





    }



}
