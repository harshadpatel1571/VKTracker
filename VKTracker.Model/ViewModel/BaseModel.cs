using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
