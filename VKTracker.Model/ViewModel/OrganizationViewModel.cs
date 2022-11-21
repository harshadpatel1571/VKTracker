using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class OrganizationViewModel : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Organization Name is required.")]
        [RegularExpression(@"^\S+(?:\s+\S+)*$", ErrorMessage = "Organization Name not valid.")]
        public string Name { get; set; }
    }
}
