using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Users.Data.Constant;
using Users.Data.DataAccess;
using Users.Data.Entities;
using Users.Data.Response;

namespace Users.Service.Core
{
    public interface IUserService
    {
        Task<ResultModel> LoginAccount(string userName, string password);
        Task<ResultModel> RegisterAccount(string UserName, string Password, string FullName);
    }
    public class UsersService : IUserService
    {
        readonly UserDbContext _db;
        public ResultModel _result;
        public UsersService(UserDbContext db)
        {
            _db = db;
            _result = new ResultModel();
        }

        public async Task<ResultModel> LoginAccount(string userName, string password)
        {
            try
            {
                var customer = await _db.Users.FirstAsync(u => u.UserName == userName && u.Password == password);
                _result.Data = customer;
                _result.IsSuccess = true;


            }
            catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;
        }

        public async Task<ResultModel> RegisterAccount(string UserName, string Password, string FullName)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.AddAsync(new User()
                    {
                        UserName = UserName,
                        Password = Password,
                        FullName = FullName,
                        Active = 0,
                        Role = Role.Customer
                    }); 
                    _db.SaveChanges();
                    _result.IsSuccess = true;
                    _result.Message = "Register successful";
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _result.IsSuccess = false;
                    _result.Message = "UserName is exist";
                }
            }
            return _result;
            
        }
    }
}
