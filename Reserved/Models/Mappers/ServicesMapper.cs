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
        private String url = "http://autoline.h1n.ru/get_all_services.php";

        public List<Service> GetServices()
        {
            List<Service> services = new List<Service>();
            String response = jsonMaster.GetJSON(url, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int)success == 1)
            {
                JToken jServices = jObject["services"];
                var jArray = jServices.ToArray();
                services = jArray.Select(element =>
                {
                    if (element == null) throw new ArgumentNullException("element");
                    return new Service((int) element["id"],
                        element["name"].ToString(),
                        element["notation"].ToString(),
                        element["duration"].ToString(),
                        element["path_to_image"].ToString(),
                        (int) element["id_category"],
                        (int) element["id_subcategory"]);
                }).ToList();
            }
            return services;

        }
    }
}