using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class LoginViewModel : BaseModel
    {
        [Required (ErrorMessage = "Username is required.")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public string Tocken { get; set; }
    }
}
