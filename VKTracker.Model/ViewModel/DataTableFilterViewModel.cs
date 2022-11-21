using System.Collections.Generic;

namespace VKTracker.Model.ViewModel
{
    public class DataTableFilterViewModel
    {
        public string Draw { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }

        public string SearchValue { get; set; }
    }

    public class DataTableResponseDto<T>
    {
        public string Draw { get; set; }

        public int RecordsFiltered { get; set; }

        public int RecordsTotal { get; set; }

        public IEnumerable<T> Data { get; set; }
    }

    public class DataTableResponseCarrier<T>
    {
        public int TotalCount { get; set; }

        public ICollection<T> Data { get; set; }
    }
}
