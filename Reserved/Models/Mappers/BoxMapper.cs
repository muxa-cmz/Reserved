using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class BoxMapper
    {
        private JsonMaster jsonMaster = new JsonMaster();
        private Dictionary<String, String> parameters = new Dictionary<string, string>();
        private String url_get_all_box = "http://autoline.h1n.ru/get_all_boxs.php";
        private String url_get_busy_box = "http://autoline.h1n.ru/get_busy_boxes.php";


        public List<Box> GetBoxes()
        {
            List<Box> boxs = new List<Box>();
            String response = jsonMaster.GetJSON(url_get_all_box, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int)success == 1)
            {
                JToken jServices = jObject["boxs"];
                var jArray = jServices.ToArray();
                boxs = jArray.Select(element => new Box((int)element["id"],
                                                        element["name"].ToString())).ToList();
            }
            return boxs;
        }

        public List<int> GetBusyBoxes(int idDay, List<int> intervalTime)
        {
            List<int> busyBox = new List<int>();
            String intervalTimeString = intervalTime.Aggregate("", (current, item) => current + (item + ","));
            parameters.Add("day", idDay.ToString());
            parameters.Add("intervalTime", intervalTimeString.Substring(0, intervalTimeString.Length - 1));
            String response = jsonMaster.GetJSON(url_get_busy_box, parameters);
            JObject jObject = JObject.Parse(response);
            JToken success = jObject["success"];
            if ((int) success == 1)
            {
                JToken jServices = jObject["boxs"];
                var jArray = jServices.ToArray();
                busyBox = jArray.Select(element => (int) element["id"]).ToList();
            }
            return busyBox;
        }
    }
}