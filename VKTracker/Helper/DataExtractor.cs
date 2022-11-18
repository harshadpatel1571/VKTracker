using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using VKTracker.Model.ViewModel;

namespace VKTracker.Helper
{
    public class DataExtractor
    {
        public static DataTableFilterDto Extract(HttpRequestBase request)
        {
            var sortColumn = request.Form["columns[" + request.Form["order[0][column]"][0] + "][name]"];

            return new DataTableFilterDto
            {
                Draw = request.Form["draw"],
                Skip = Convert.ToInt32(request.Form["start"]),
                Take = Convert.ToInt32(request.Form["length"]),
                SortColumn = sortColumn.Contains(' ')
                    ? string.Join(null, sortColumn.Split(' '))
                    : sortColumn,
                SortOrder = request.Form["order[0][dir]"],
                SearchValue = request.Form["search[value]"]
            };
        }
    }

    internal static class JsonSetting
    {
        internal static readonly JsonSerializerSettings Default = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}