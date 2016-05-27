using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class TimeIntervalsMapper
    {
        private JsonMaster jsonMaster = new JsonMaster();
        private Dictionary<String, String> parameters = new Dictionary<string, string>();
        private String url = "http://autoline.h1n.ru/get_all_timeintervals.php";

        public List<TimeIntervals> GetCategories()
        {
            List<TimeIntervals> timeIntervals = new List<TimeIntervals>();
            String response = jsonMaster.GetJSON(url, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int)success == 1)
            {
                JToken jServices = jObject["timeIntervals"];
                var jArray = jServices.ToArray();
                timeIntervals = jArray.Select(element => new TimeIntervals((int)element["id"],
                                                                    element["name"].ToString())).ToList();
            }
            return timeIntervals;
        }
    }
}