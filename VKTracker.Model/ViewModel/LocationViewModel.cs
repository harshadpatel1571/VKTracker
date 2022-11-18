using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class LocationViewModel : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string LocationName { get; set; }
    }
}
