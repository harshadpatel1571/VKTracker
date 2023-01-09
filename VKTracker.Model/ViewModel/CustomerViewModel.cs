using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class CustomerViewModel : BaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^\S+(?:\s+\S+)*$", ErrorMessage = "First Name not valid.")]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Address { get; set; }
        [Required(ErrorMessage = "Mobile No is required.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Mobile No only 10 digit allow.")]
        public string Mobile { get; set; }

        public int? LocationId { get; set; }
        public string LocationName { get; set; }

    }
}
