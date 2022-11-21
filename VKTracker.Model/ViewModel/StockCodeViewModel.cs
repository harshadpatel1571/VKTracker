using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class StockCodeViewModel : BaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Stock Code is required.")]
        [RegularExpression(@"^\S+(?:\s+\S+)*$", ErrorMessage = "Stock Name not valid.")]
        public string Code { get; set; }
    }
}
