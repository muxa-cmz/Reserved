using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using Reserved.Models.DomainModels;
using Reserved.Models.Mappers;
using Category = Reserved.Models.DomainModels.Category;
using Service = Reserved.Models.DomainModels.Service;
using CategoryDDL = DropDownList.Category;
using ServiceDDL = DropDownList.Service;

namespace Reserved.TabsReserve
{
    public partial class OrderWithListServices : System.Web.UI.Page
    {
        private static List<Service> services;

        private RadioButtonsList.RadioButtonsList radioButtonsList;

        private List<CategoryDDL> CategoriesToCategoriesDLL(List<Category> categories)
        {
            return categories.Select(category => new CategoryDDL(category.Id, category.Name)).ToList();
        }

        private List<ServiceDDL> ServicesToServicesDLL(List<Service> services)
        {
            return services.Select(service => new ServiceDDL(service.Id, 
                                                             service.Name,
                                                             service.Notation,
                                                             service.Duration,
                                                             service.PathToImage,
                                                             service.IdCategory,
                                                             service.IdSubCategory)).ToList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Предотвращение повторной инициализации
            if (IsPostBack) return; 
            services = new List<Service>();
            List<Category> categories = new List<Category>();
            CategoryMapper categoriesMapper = new CategoryMapper();
            categories.AddRange(categoriesMapper.GetCategories());
            
            ServicesMapper servicesMapper = new ServicesMapper();
            services.AddRange(servicesMapper.GetServices());

            if (Master != null)
            {
                ContentPlaceHolder placeHolder = (ContentPlaceHolder)Master.FindControl("MainContent");
                DropDownList.DropDownList dropDownList = (DropDownList.DropDownList)placeHolder.FindControl("ServiceList");
                dropDownList.Categories = CategoriesToCategoriesDLL(categories);
                dropDownList.Services = ServicesToServicesDLL(services);
            }

            #region Список всех интервалов времени
            TimeIntervalsMapper intervalsMapper = new TimeIntervalsMapper();
            List<TimeIntervals> intervalses = new List<TimeIntervals>();
            intervalses.AddRange(intervalsMapper.GetIntervals());
            #endregion

            #region Дефолтные статусы для всех радио кнопок (true)
            Dictionary<String, bool> times = intervalses.ToDictionary(interval => interval.Name, interval => true);
            #endregion

            if (Master != null)
            {
                ContentPlaceHolder ph = (ContentPlaceHolder)Master.FindControl("MainContent");
                radioButtonsList = (RadioButtonsList.RadioButtonsList)ph.FindControl("radioButtonsList");
                radioButtonsList.TimePeriods = times;
            }
        }

        [WebMethod]
        public static String GetTime(string date)
        {
            InformationOrdersMapper informationOrdersMapper = new InformationOrdersMapper();

            #region Список всех интервалов времени
            TimeIntervalsMapper intervalsMapper = new TimeIntervalsMapper();
            List<TimeIntervals> intervalses = new List<TimeIntervals>();
            intervalses.AddRange(intervalsMapper.GetIntervals());
            #endregion

            #region Дефолтные статусы для всех радио кнопок (true)
            Dictionary<String, String> times = intervalses.ToDictionary(interval => interval.Id.ToString(), interval => "false");
            #endregion

            #region Список уже забранированных на день интервалов времени
            List<InformationOrders> informationOrderses = new List<InformationOrders>();
            informationOrderses.AddRange(informationOrdersMapper.GetInformaIntervalsesOnDate(date));
            #endregion

            #region Считывание файла cookie, определение выбранных позиций
            var httpCookie = HttpContext.Current.Request.Cookies["checked_services"];
            List<String> checkedServices = new List<String>();
            if (httpCookie != null)
            {
                var value = httpCookie.Value;
                checkedServices.AddRange(value.Split(','));
            }
            #endregion

            /*FileMaster fileMaster = new FileMaster();*/
            // Из настроек получить минимальное время на операцию
            /*var settingsMap = new Dictionary<String, String>(fileMaster.Read());
            int minExpectancy = Convert.ToInt32(settingsMap["minAction"]); // минимальное время на операцию
            int countActions = Convert.ToInt32(settingsMap["countActions"]);  // максимальное количество действий в один промежуток времени
            */
            int minExpectancy = 20;
            int countActions = 2;

            int idDay = informationOrderses.ElementAt(0).IdDay; // id даты бронирования
            List<TimeIntervals> timeForReserved = new List<TimeIntervals>();

            #region Выбираем те интервалы времени, в которых есть лимит для работы
            timeForReserved = (from interval in intervalses 
                               let count = informationOrderses
                                                .Count(info => info.IdTimeInterval == interval.Id)
                               where count != countActions 
                               select interval).ToList();

            #endregion

            #region Подсчет времени на выполнение заказа
            int totalExpectancy = services.Sum(service => checkedServices
                                                            .Where(element => service.Id == Convert.ToInt32(element))
                                                            .Sum(element => Convert.ToInt32(service.Duration)));    // Полное время на выполнение операции
            #endregion

            int sequenseTime = (totalExpectancy / minExpectancy);   // Количество отрезков времени

            #region Выборка интервалов времени для выбранных услуг
            List<TimeIntervals> tempTimeOrders = new List<TimeIntervals>();
            List<TimeIntervals> TimeForOrders = new List<TimeIntervals>();
            if (sequenseTime > 1)
            {
                for (int i = 0; i < timeForReserved.Count; i++)
                {
                    int rowNumberCurrent = timeForReserved.ElementAt(i).Id;
                    tempTimeOrders.Add(timeForReserved.ElementAt(i));
                    for (int j = 1; j < timeForReserved.Count; j++)
                    {
                        int rowNumberNext = timeForReserved.ElementAt(j).Id;
                        if ((rowNumberNext - rowNumberCurrent) == 1)
                        {
                            tempTimeOrders.Add(timeForReserved.ElementAt(j));
                            rowNumberCurrent++;
                            if (tempTimeOrders.Count == sequenseTime)
                            {
                                TimeForOrders.AddRange(tempTimeOrders);
                                tempTimeOrders.Clear();
                                break;
                            }
                        }
                    }
                    tempTimeOrders.Clear();
                }
            }
            else
            {
                TimeForOrders.AddRange(timeForReserved);
            }
            #endregion

            #region Выбираем интервалы времени где не придется менять бокс
            int seq = 0;
            List<int> boxList = new List<int>();
            List<TimeIntervals> finalTimeForOrders = new List<TimeIntervals>();
            for (int index = 0; index <= TimeForOrders.Count; index++)
            {
                List<InformationOrders> info = new List<InformationOrders>();
                TimeIntervals element = null;
                if (index < TimeForOrders.Count)
                {
                    element = TimeForOrders[index];
                    info = InformationOrdersMapper.GetInformations(idDay, element.Id);
                }

                if (seq < sequenseTime)
                {
                    if (info.Count != 0)
                    {
                        boxList.Add(info.ElementAt(0).IdBox);
                        seq++;
                        tempTimeOrders.Add(element);
                    }
                    else
                    {
                        boxList.Add(0);
                        seq++;
                        tempTimeOrders.Add(element);
                    }
                }
                else
                {
                    List<int> uniqueBoxList = boxList.Distinct().ToList();
                    uniqueBoxList.Remove(0);
                    if (uniqueBoxList.Count < countActions)
                    {
                        finalTimeForOrders.AddRange(tempTimeOrders);
                        seq = 0;
                        tempTimeOrders.Clear();
                        boxList.Clear();
                        index--;
                    }
                    else
                    {
                        seq = 0;
                        tempTimeOrders.Clear();
                        boxList.Clear();
                        index--;
                    }
                }
            }
            #endregion

            #region Уникальные врмененные промежутки для радиокнопок
            List<TimeIntervals> timeForOrder = finalTimeForOrders
                                                    .Where((t, i) => i != finalTimeForOrders.Count - 1 
                                                        && ((t.Id == finalTimeForOrders[i + 1].Id - 1) 
                                                        || (t.Id == finalTimeForOrders[i + 1].Id)))
                                                    .ToList();
            var distinctTimeInterval = timeForOrder.GroupBy(s => s.Id).Select(s => s.First());

            foreach (var timeInterval in distinctTimeInterval)
                times[timeInterval.Id.ToString()] = "true";
            #endregion

            #region Формирование json строки
            StringBuilder json = new StringBuilder("{\"array\": [");
            foreach (var el in times)
            {
                json.Append("{\"interval\":\"")
                .Append(el.Key)
                .Append("\", \"flag\":")
                .Append(el.Value)
                .Append("},");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]}");
            #endregion

            return json.ToString();
        }
    }
}


