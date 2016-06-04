using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class DayMapper
    {
        private JsonMaster jsonMaster = new JsonMaster();
        private Dictionary<String, String> parameters = new Dictionary<string, string>();
        private String url_existence_day = "http://autoline.h1n.ru/existence_day.php";

        public int GetDayId(String date)
        {
            int id = 0;
            parameters.Add("date",date);
            String response = jsonMaster.GetJSON(url_existence_day, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int)success == 1)
            {
                JToken jServices = jObject["day"];
                var jArray = jServices.ToArray();
                id = jArray.Select(element => { return (int) element["id"]; }).ElementAt(0);
            }
            return id;
        }
    }
}