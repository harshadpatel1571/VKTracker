using System;

namespace VKTracker.Model.ViewModel
{
    public class BaseModel
    {
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Action { get; set; }
        public string LogUserName { get; set; }
        public string LogStrDate { get; set; }
    }
}
