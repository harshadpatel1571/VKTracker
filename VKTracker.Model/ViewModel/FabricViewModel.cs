using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class FabricViewModel : BaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Fabric Name is required.")]
        [RegularExpression(@"^\S+(?:\s+\S+)*$", ErrorMessage = "Fabric Name not valid.")]
        public string FabricName { get; set; }
    }
}
