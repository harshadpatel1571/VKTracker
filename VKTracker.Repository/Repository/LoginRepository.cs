using System;
using System.Linq;
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

        public bool ValidateUser(LoginViewModel objModel)
        {
            bool result = false;
            string encPassword = Encryption.Encrypt(objModel.Password);
            using (var db = new VKTrackerEntities())
            {
                result = db.Users.Where(x => x.UserName == objModel.UserName && x.Password == encPassword).Any();
            }
            return result;
        }
    }
}
