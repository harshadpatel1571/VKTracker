using System;
using System.ComponentModel.DataAnnotations;

namespace VKTracker.Model.ViewModel
{
    public class ItemViewModel : BaseModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Item Name is required.")]
        public string ItemName { get; set; }
    }
}
