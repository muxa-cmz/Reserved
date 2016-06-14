using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class ReservedTablesMapper
    {
        private JsonMaster jsonMaster = new JsonMaster();
        private Dictionary<String, String> parameters = new Dictionary<string, string>();
        private String url_get_busy_table = "http://autoline.h1n.ru/get_busy_table.php";
        private String url_reserve_table = "http://autoline.h1n.ru/reserve_table.php";

        public List<int> GetBusyTables(String idDay)
        {
            List<int> busyTable = new List<int>();
            parameters.Add("day", idDay);
            String response = jsonMaster.GetJSON(url_get_busy_table, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int)success == 1)
            {
                JToken jServices = jObject["tables"];
                var jArray = jServices.ToArray();
                busyTable = jArray.Select(element => (int)element["id_table"]).ToList();
            }
            return busyTable;
        }

        public String ReserveTable(String name, String phone, String table, String time, int dayId)
        {
            parameters.Add("firstname", name);
            parameters.Add("phone", phone);
            parameters.Add("table", table);
            parameters.Add("time", time);
            parameters.Add("dayId", dayId.ToString());
            String response = jsonMaster.GetJSON(url_reserve_table, parameters);
            //response = response.Remove(0, response.IndexOf("{", StringComparison.Ordinal));
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            String idOrder = "";
            if ((int)success == 1)
            {
                JToken jServices = jObject["orderInformation"];
                var jArray = jServices.ToArray();
                idOrder = jArray.Select(element => { return element["id"].ToString(); }).ElementAt(0);
            }
            return idOrder;
        }


    }
}