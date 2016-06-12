using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class HallMapper
    {
        private JsonMaster jsonMaster = new JsonMaster();
        private Dictionary<String, String> parameters = new Dictionary<string, string>();
        private String url_get_all_hall = "http://autoline.h1n.ru/get_all_hall.php";
        private String url_get_tables_in_hall = "http://autoline.h1n.ru/get_tables_in_hall.php";

        public List<Hall> GetHalls()
        {
            List<Hall> halls = new List<Hall>();
            String response = jsonMaster.GetJSON(url_get_all_hall, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int)success == 1)
            {
                JToken jServices = jObject["halls"];
                var jArray = jServices.ToArray();
                halls = jArray.Select(element => new Hall((int)element["id"],
                                                        element["name"].ToString(),
                                                        element["numberOfSeats"].ToString(),
                                                        element["description"].ToString(),
                                                        element["path_to_image"].ToString()
                                                        )).ToList();
            }
            return halls;
        }

        public List<int> GetTablesInHall(String idHall)
        {
            List<int> allTable = new List<int>();
            parameters.Add("hall", idHall);
            String response = jsonMaster.GetJSON(url_get_tables_in_hall, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int)success == 1)
            {
                JToken jServices = jObject["tables"];
                var jArray = jServices.ToArray();
                allTable = jArray.Select(element => (int)element["id"]).ToList();
            }
            return allTable;
        }
    }
}