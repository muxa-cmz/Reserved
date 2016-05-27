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

            List<Category> categories = new List<Category>();
            CategoryMapper categoriesMapper = new CategoryMapper();
            categories.AddRange(categoriesMapper.GetCategories());

            List<Service> services = new List<Service>();
            ServicesMapper servicesMapper = new ServicesMapper();
            services.AddRange(servicesMapper.GetServices());

            if (Master != null)
            {
                ContentPlaceHolder placeHolder = (ContentPlaceHolder)Master.FindControl("MainContent");
                DropDownList.DropDownList dropDownList = (DropDownList.DropDownList)placeHolder.FindControl("ServiceList");
                dropDownList.Categories = CategoriesToCategoriesDLL(categories);
                dropDownList.Services = ServicesToServicesDLL(services);
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
            Dictionary<String, String> times = intervalses.ToDictionary(interval => interval.Id.ToString(), interval => "true");
            #endregion

            #region Список уже забранированных на день интервалов времени
            List<InformationOrders> informationOrderses = new List<InformationOrders>();
            informationOrderses.AddRange(informationOrdersMapper.GetInformaIntervalsesOnDate(date));
            #endregion

            #region Оставляем только свободные интервалы времени
            foreach (var interval in intervalses.ToArray()
                                        .Where(interval => informationOrderses
                                                                .Any(io => interval.Id == io.IdTimeInterval)))
            {
                intervalses.Remove(interval);
            }
            #endregion
            
            HttpCookieCollection cookieCollection = new HttpCookieCollection();
            var qw = cookieCollection.Get("services");

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


