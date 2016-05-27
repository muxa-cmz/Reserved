using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class InformationOrdersMapper
    {
        private JsonMaster jsonMaster = new JsonMaster();
        private Dictionary<String, String> parameters = new Dictionary<String, String>();
        private String url = "http://autoline.h1n.ru/get_informationOrders_on_date.php";

        public List<InformationOrders> GetInformaIntervalsesOnDate(String date)
        {
            List<InformationOrders> informationOrders = new List<InformationOrders>();
            parameters.Add("date", date);
            String response = jsonMaster.GetJSON(url, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int)success == 1)
            {
                JToken jServices = jObject["informationOrders"];
                var jArray = jServices.ToArray();
                informationOrders = jArray.Select(element => new InformationOrders((int)element["id"],
                                                                    (int)element["id_day"],
                                                                    (int)element["id_box"],
                                                                    (int)element["id_order"],
                                                                    (int)element["id_time_interval"]
                                                                    )).ToList();
            }
            return informationOrders;
        }
    }
}