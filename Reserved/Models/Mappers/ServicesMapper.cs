using System;
using System.Collections.Generic;
using System.Linq;
using Reserved.Models.DomainModels;
using Newtonsoft.Json.Linq;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class ServicesMapper
    {
        private JsonMaster jsonMaster = new JsonMaster();
        private Dictionary<String, String> parameters = new Dictionary<string, string>();
        private String url_get_services = "http://autoline.h1n.ru/get_all_services.php";
        private String url_get_services_with_price = "http://autoline.h1n.ru/get_all_services_with_price.php";
        private String url_get_services_by_id = "http://autoline.h1n.ru/get_services_by_id.php";

        public List<Service> GetServices()
        {
            List<Service> services = new List<Service>();
            String response = jsonMaster.GetJSON(url_get_services, parameters);
            JObject jObject = JObject.Parse(response);

            JToken success = jObject["success"];
            
            if ((int)success == 1)
            {
                JToken jServices = jObject["services"];
                var jArray = jServices.ToArray();
                services = jArray.Select(element => new Service((int)element["id"],
                                                                element["name"].ToString(),
                                                                element["notation"].ToString(),
                                                                element["duration"].ToString(),
                                                                element["path_to_image"].ToString(),
                                                                (int)element["id_category"],
                                                                (int)element["id_subcategory"])).ToList();
            }
            return services;
        }

        public List<Service> GetServicesWithPrice()
        {
            List<Service> services = new List<Service>();
            String response = jsonMaster.GetJSON(url_get_services_with_price, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];

            JToken[] prices = new JToken[] { };
            if ((int)success == 1)
            {
                JToken jServices = jObject["services"];
                var jArray = jServices.ToArray();
                services = jArray.Select(element => new Service((int)element["id"],
                                                                element["name"].ToString(),
                                                                element["notation"].ToString(),
                                                                element["duration"].ToString(),
                                                                element["path_to_image"].ToString(),
                                                                (int)element["id_category"],
                                                                (int)element["id_subcategory"])).ToList();

                prices = jArray.Select(element => element["prices"]).ToArray();
            }

            for (int i = 0; i < services.Count; i++)
            {
                foreach (var item in prices[i])
                {
                    String key = item.ToString();
                    key = key.Substring(key.IndexOf("\"", StringComparison.Ordinal) + 1, key.IndexOf(":", StringComparison.Ordinal) - 2);
                    services[i].Prices.Add(key, item.First.ToString());
                }
            }
            return services;
        }

        public List<Service> GetServicesById(String stringId)
        {
            List<Service> services = new List<Service>();
            parameters.Add("id", stringId);
            String response = jsonMaster.GetJSON(url_get_services_by_id, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];

            JToken[] prices = new JToken[] { };
            if ((int)success == 1)
            {
                JToken jServices = jObject["services"];
                var jArray = jServices.ToArray();
                services = jArray.Select(element => new Service((int)element["id"],
                                                                element["name"].ToString(),
                                                                element["notation"].ToString(),
                                                                element["duration"].ToString(),
                                                                element["path_to_image"].ToString(),
                                                                (int)element["id_category"],
                                                                (int)element["id_subcategory"])).ToList();

                prices = jArray.Select(element => element["prices"]).ToArray();
            }

            for (int i = 0; i < services.Count; i++)
            {
                foreach (var item in prices[i])
                {
                    String key = item.ToString();
                    key = key.Substring(key.IndexOf("\"", StringComparison.Ordinal) + 1, key.IndexOf(":", StringComparison.Ordinal) - 2);
                    services[i].Prices.Add(key, item.First.ToString());
                }
            }
            return services;
        }
    }
}