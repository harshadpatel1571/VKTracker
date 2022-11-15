using System;
using System.Linq;
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
            using (var db = new VKTrackerEntities())
            {
                result = db.Users.Where(x => x.UserName == objModel.UserName && x.Password == objModel.Password).Any();
            }
            return result;
        }
    }
}
