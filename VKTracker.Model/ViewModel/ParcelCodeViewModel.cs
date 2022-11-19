using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class ParcelCodeViewModel : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Parcel Code is required.")]
        public string Code { get; set; }
    }
}
