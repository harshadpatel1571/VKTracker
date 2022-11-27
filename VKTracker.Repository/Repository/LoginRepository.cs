using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VKTracker.Common.Helper;
using VKTracker.Model.Context;
using VKTracker.Model.ViewModel;

namespace VKTracker.Repository
{
    public class LoginRepository : IDisposable
    {
        public void Dispose()
        {
        }

        public async Task<UserViewModel> ValidateUser(LoginViewModel objModel)
        {
            string encPassword = Encryption.Encrypt(objModel.Password);
            using (var db = new VKTrackerEntities())
            {
                   return await db.Users.Where(x => x.UserName == objModel.UserName && x.Password == encPassword)
                    .Select(x=> new UserViewModel
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        IsAdmin = x.IsAdmin,
                        EmailId= x.EmailId
                    }).FirstOrDefaultAsync().ConfigureAwait(false);
            }
        }
    }
}
