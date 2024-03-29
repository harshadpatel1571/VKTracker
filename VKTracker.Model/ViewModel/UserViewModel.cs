﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class UserViewModel : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        [RegularExpression(@"^\S+(?:\s+\S+)*$", ErrorMessage = "User Name not valid.")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8,ErrorMessage = "Password must be a minimum length of 8.")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^\S+(?:\s+\S+)*$", ErrorMessage = "First Name not valid.")]
        public string FirstName { get; set; }

        [RegularExpression(@"^\S+(?:\s+\S+)*$", ErrorMessage = "Last Name not valid.")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email Id is required.")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Incorrect Email Format")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Mobile No is required.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Mobile No only 10 digit allow.")]
        public string MobileNo { get; set; }


        [Required(ErrorMessage = "Organization is required.")]
        public IList<int> OrganizationId { get; set; }
    }
}
