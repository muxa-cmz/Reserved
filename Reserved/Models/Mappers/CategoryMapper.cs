using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class CategoryMapper
    {
        private JsonMaster jsonMaster = new JsonMaster();
        private Dictionary<String, String> parameters = new Dictionary<string, string>();
        private String url = "http://autoline.h1n.ru/get_all_categories.php";

        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            String response = jsonMaster.GetJSON(url, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int)success == 1)
            {
                JToken jServices = jObject["categories"];
                var jArray = jServices.ToArray();
                categories = jArray.Select(element => new Category((int)element["id"],
                                                                    element["name"].ToString())).ToList();
            }
            return categories;

        }
    }
}