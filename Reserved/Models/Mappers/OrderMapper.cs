using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class OrderMapper
    {
        private JsonMaster jsonMaster = new JsonMaster();
        private Dictionary<String, String> parameters = new Dictionary<string, string>();
        private String url_insert_order = "http://autoline.h1n.ru/insert_order.php";

        public int InsertOrder(Order order)
        {
            parameters.Add("firstname", order.FirstName);
            parameters.Add("lastname", order.LastName);
            parameters.Add("services", order.Services);
            String response = jsonMaster.GetJSON(url_insert_order, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int) success == 1)
            {
                JToken jOrder = jObject["order"];
                var jArray = jOrder.ToArray();
                int id = jArray.Select(element => (int) element["id"]).ElementAt(0);
                return id;
            }
            return 0;
        }
    }
}