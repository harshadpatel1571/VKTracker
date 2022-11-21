using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class ParcelCodeViewModel : BaseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Parcel Code is required.")]
        [RegularExpression(@"^\S+(?:\s+\S+)*$", ErrorMessage = "Parcel Name not valid.")]
        public string Code { get; set; }
    }
}
