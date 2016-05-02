using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DropDownList;
using Reserved.Models.DomainModels;
using Reserved.Models.Mappers;
using Category = Reserved.Models.DomainModels.Category;
using DropDownList = DropDownList.DropDownList;
using CategoryDDL = DropDownList.Category;
using Service = Reserved.Models.DomainModels.Service;
using ServiceDDL = DropDownList.Service;

namespace Reserved.TabsReserve
{
    public partial class OrderWithListServices : System.Web.UI.Page
    {

        private List<CategoryDDL> CategoriesToCategoriesDLL(List<Category> categories)
        {
            List<CategoryDDL> categoriesDDL = new List<CategoryDDL>();
            foreach (var category in categories)
            {
                categoriesDDL.Add(new CategoryDDL(category.Id, category.Name));
            }
            return categoriesDDL;
        }

        private List<ServiceDDL> ServicesToServicesDLL(List<Service> services)
        {
            List<ServiceDDL> servicesDDL = new List<ServiceDDL>();
            foreach (var service in services)
            {
                servicesDDL.Add(new ServiceDDL(service.Id,
                                               service.Name,
                                               service.Price,
                                               service.Notation,
                                               service.Duration,
                                               service.PathToImage,
                                               service.IdCategory,
                                               service.IdSubCategory));
            }
            return servicesDDL;
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

            List<Category> categories = new List<Category>();
            CategoryMapper categoriesMapper = new CategoryMapper();
            categories.AddRange(categoriesMapper.GetCategory());

            List<Service> services = new List<Service>();
            ServicesMapper servicesMapper = new ServicesMapper();
            services.AddRange(servicesMapper.GetServices());


            if (Master != null)
            {
                ContentPlaceHolder placeHolder = (ContentPlaceHolder)Master.FindControl("MainContent");

                global::DropDownList.DropDownList ddList = new global::DropDownList.DropDownList
                {
                    Categories = CategoriesToCategoriesDLL(categories),
                    Services = ServicesToServicesDLL(services)
                };
                placeHolder.Controls.Add(ddList);
            }
            
        }

    }
}


