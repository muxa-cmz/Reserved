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
        private static JsonMaster jsonMaster = new JsonMaster();
        private static Dictionary<String, String> parameters = new Dictionary<String, String>();
        private static String urlGetInformaIntervalsesOnDate = "http://autoline.h1n.ru/get_informationOrders_on_date.php";
        private static String urlGetInformations = "http://autoline.h1n.ru/get_informations.php";
        private static String url_insert_information_order = "http://autoline.h1n.ru/insert_information_order.php";

        public List<InformationOrders> GetInformaIntervalsesOnDate(String date)
        {
            List<InformationOrders> informationOrders = new List<InformationOrders>();
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("date", date);
            String response = jsonMaster.GetJSON(urlGetInformaIntervalsesOnDate, parameters);
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

        public static List<InformationOrders> GetInformations(int idDay, int idTimeInterval)
        {
            List<InformationOrders> informationOrders = new List<InformationOrders>();
            Dictionary<String, String> parameters = new Dictionary<String, String>();
            parameters.Add("date", idDay.ToString());
            parameters.Add("idTimeInterval", idTimeInterval.ToString());
            String response = jsonMaster.GetJSON(urlGetInformations, parameters);
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

        public int InsertInformationOrders(int idDay, int idTimeInterval, int idBox, int idOrder)
        {
            parameters = new Dictionary<string, string>();
            parameters.Add("idDay", idDay.ToString());
            parameters.Add("idTimeInterval", idTimeInterval.ToString());
            parameters.Add("idBox", idBox.ToString());
            parameters.Add("idOrder", idOrder.ToString());
            String response = jsonMaster.GetJSON(url_insert_information_order, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int)success == 1)
            {
                JToken jOrder = jObject["order"];
                var jArray = jOrder.ToArray();
                int id = jArray.Select(element => (int)element["id"]).ElementAt(0);
                return id;
            }
            return 0;
        }
    }
}