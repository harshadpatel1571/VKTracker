using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class LocationViewModel : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [RegularExpression(@"^\S+(?:\s+\S+)*$", ErrorMessage = "Location Name not valid.")]
        public string LocationName { get; set; }
    }
}
