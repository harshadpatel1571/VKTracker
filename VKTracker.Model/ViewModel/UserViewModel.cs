using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKTracker.Model.ViewModel
{
    public class UserViewModel : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailId { get; set; }

        public string MobileNo { get; set; }


        [Required(ErrorMessage = "Organization is required.")]
        public IList<int> OrganizationId { get; set; }
    }
}
