using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DropDownList;
using Reserved.Models.DomainModels;
using Reserved.Models.Mappers;
using Category = Reserved.Models.DomainModels.Category;
using Service = Reserved.Models.DomainModels.Service;
//using DropDownList = System.Web.UI.WebControls.DropDownList.DropDownList;
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
                                                             service.Price,
                                                             service.Notation,
                                                             service.Duration,
                                                             service.PathToImage,
                                                             service.IdCategory,
                                                             service.IdSubCategory)).ToList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //List<Service> services = new List<Service>();
            //ServicesMapper servicesMapper = new ServicesMapper();
            //CategoryMapper categoriesMapper = new CategoryMapper();
            //foreach (var category in categories)
            //{
            //    services.AddRange(servicesMapper.GetServicesByCategory(category.Id));
            //}

            // Предотвращение повторной инициализации
            if (this.IsPostBack) return; 

            List<Category> categories = new List<Category>();
            CategoryMapper categoriesMapper = new CategoryMapper();
            categories.AddRange(categoriesMapper.GetCategory());

            List<Service> services = new List<Service>();
            ServicesMapper servicesMapper = new ServicesMapper();
            services.AddRange(servicesMapper.GetServices());


            if (Master != null)
            {
                ContentPlaceHolder placeHolder = (ContentPlaceHolder)Master.FindControl("MainContent");

                DropDownList.DropDownList dropDownList = (DropDownList.DropDownList)placeHolder.FindControl("ServiceList");

                dropDownList.Categories = CategoriesToCategoriesDLL(categories);
                dropDownList.Services = ServicesToServicesDLL(services);

                //var ew = dropDownList.Controls;

                //global::DropDownList.DropDownList ddList = new global::DropDownList.DropDownList
                //{
                //    Categories = CategoriesToCategoriesDLL(categories),
                //    Services = ServicesToServicesDLL(services)
                //};

            }
            
        }
    }
}


