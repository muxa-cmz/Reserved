using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Reserved.Models.DomainModels;
using Reserved.Models.Mappers;
using Category = Reserved.Models.DomainModels.Category;
using Service = Reserved.Models.DomainModels.Service;
using CategoryDDL = DropDownList.Category;
using ServiceDDL = DropDownList.Service;
using ServiceYS = YourServices.Service;

namespace Reserved.TabsReserve
{
    public partial class OrderWithListServices : System.Web.UI.Page
    {
        private static List<Service> services;

        private static List<Service> yourServices;

        private static List<String> checkedServices;

        private static List<TimeIntervals> intervalses;

        private RadioButtonsList.RadioButtonsList radioButtonsList;

        private static int sequenseTime;
        private static int idDay;

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

        private static List<ServiceYS> ServicesToServicesYS(List<Service> services)
        {
            return services.Select(service => new ServiceYS(service.Id,
                                                             service.Name,
                                                             service.Notation,
                                                             service.Duration,
                                                             service.PathToImage,
                                                             service.IdCategory,
                                                             service.IdSubCategory,
                                                             service.Prices)).ToList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Предотвращение повторной инициализации
            if (IsPostBack)
                return;
            
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

            //if (Master != null)
            //{
            //    ContentPlaceHolder ph = (ContentPlaceHolder)Master.FindControl("MainContent");
            //    var yourServicesList = (YourServices.YourServices)ph.FindControl("yourServices");
            //    yourServicesList.Services = ServicesToServicesYS(yourServices);
            //}5

        }

        [WebMethod]
        public static String SetServicesList()
        {
            yourServices = new List<Service>();
            ServicesMapper servicesMapper = new ServicesMapper();
            String ids = checkedServices.Aggregate("", (current, item) => current + (item + ","));
            yourServices.AddRange(servicesMapper.GetServicesById(ids.Substring(0, ids.Length - 1)));

            #region Формирование json строки
            StringBuilder json = new StringBuilder("{\"array\": [");
            foreach (var el in yourServices)
            {
                json.Append("{\"name\":\"")
                .Append(el.Name)
                .Append("\", \"price\":{");
                foreach (var item in el.Prices)
                {
                    json.Append("\"" + item.Key + "\":\"" + item.Value + "\",");
                }
                json.Remove(json.Length - 1, 1)
                .Append("}},");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]}");
            #endregion

            return json.ToString();
        }

        [WebMethod]
        public static String GetTime(string date)
        {
            InformationOrdersMapper informationOrdersMapper = new InformationOrdersMapper();
            DayMapper dayMapper = new DayMapper();

            #region Список всех интервалов времени
            TimeIntervalsMapper intervalsMapper = new TimeIntervalsMapper();
            intervalses = new List<TimeIntervals>();
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
            checkedServices = new List<String>();
            if (httpCookie != null)
            {
                var value = httpCookie.Value;
                checkedServices.AddRange(value.Split(','));
            }
            #endregion

            //if (Master != null)
            //{
            //    ContentPlaceHolder ph = (ContentPlaceHolder)Master.FindControl("MainContent");
            //    var yourServicesList = (YourServices.YourServices)ph.FindControl("yourServices");
            //    yourServicesList.Services = ServicesToServicesYS(yourServices);
            //}
            

            /*FileMaster fileMaster = new FileMaster();*/
            // Из настроек получить минимальное время на операцию
            /*var settingsMap = new Dictionary<String, String>(fileMaster.Read());
            int minExpectancy = Convert.ToInt32(settingsMap["minAction"]); // минимальное время на операцию
            int countActions = Convert.ToInt32(settingsMap["countActions"]);  // максимальное количество действий в один промежуток времени
            */
            int minExpectancy = 20;
            int countActions = 2;

            //int idDay = informationOrderses.ElementAt(0).IdDay; // id даты бронирования
            idDay = dayMapper.GetDayId(date);   // id даты бронирования
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

            sequenseTime = (totalExpectancy / minExpectancy);   // Количество отрезков времени

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

        // Бронирование, занесение в бд
        protected void confirm_OnClick(object sender, EventArgs e)
        {
            #region Формирование заказа
            ServicesMapper servicesMapper = new ServicesMapper();
            String checkedServicesString = checkedServices.Aggregate("", (current, item) => current + (item + ","));

            String servicesString = servicesMapper.GetServicesById(checkedServicesString.Substring(0, checkedServicesString.Length - 1))
                                        .Aggregate("", (current, item) => current + (item.Name + ","));

            servicesString = servicesString.Substring(0, servicesString.Length - 1);

            Order newOrder = new Order(lastName.Value, firstName.Value, servicesString);
            #endregion

            #region Получение значения выбранного интервала времени
            int intervalId = Convert.ToInt32(Request["idrb"]); 
            #endregion

            #region Получение id интервалов времени
            List<int> timeIntervalId = new List<int>();
            for (int i = 0; i < sequenseTime; i++)
            {
                timeIntervalId.Add(intervalId + i);
            }
            #endregion
            
            #region Получение свободных боксов на данные промежутки времени
            List<Box> boxes = new List<Box>();  // полный список боксов
            BoxMapper boxMapper = new BoxMapper();
            boxes.AddRange(boxMapper.GetBoxes());

            List<int> busyBoxes = new List<int>();  // занятые боксы
            busyBoxes.AddRange(boxMapper.GetBusyBoxes(idDay, timeIntervalId));

            foreach (var box in boxes.ToArray().Where(box => busyBoxes.Contains(box.Id)))
            {
                boxes.Remove(box);
            }
            #endregion

            #region Из всех свободных боксов, выбираем бокс с минимальным id, для бронирования интервалов времени
            int minIdBox = boxes.ElementAt(0).Id;
            minIdBox = boxes.Select(box => box.Id).Concat(new[] {minIdBox}).Min();
            #endregion

            #region Заполнение БД
            OrderMapper orderMapper = new OrderMapper();
            int idOrder = orderMapper.InsertOrder(newOrder);

            InformationOrdersMapper informationOrdersMapper = new InformationOrdersMapper();
            foreach (var interval in timeIntervalId)
            {
                informationOrdersMapper.InsertInformationOrders(idDay,interval,minIdBox,idOrder);
            }
            #endregion
        }
    }
}