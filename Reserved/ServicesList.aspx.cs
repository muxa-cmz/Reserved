using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Reserved.Models.Mappers;

using CategoryDDL = ServicesList.Category;
using Service = Reserved.Models.DomainModels.Service;
using Category = Reserved.Models.DomainModels.Category;
using ServiceDDL = ServicesList.Service;


namespace Reserved
{
    public partial class ServicesList : System.Web.UI.Page
    {
        private global::ServicesList.ServicesList servicesList;

        private List<ServiceDDL> ServicesToServicesDLL(List<Service> services)
        {
            return services.Select(service => new ServiceDDL(service.Id,
                                                             service.Name,
                                                             service.Notation,
                                                             service.Duration,
                                                             service.PathToImage,
                                                             service.IdCategory,
                                                             service.IdSubCategory,
                                                             service.Prices)).ToList();
        }

        private List<CategoryDDL> CategoriesToCategoriesDLL(List<Category> categories)
        {
            return categories.Select(category => new CategoryDDL(category.Id, category.Name)).ToList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ServicesMapper servicesMapper = new ServicesMapper();
            var ew = servicesMapper.GetServicesWithPrice();

            CategoryMapper categoryMapper = new CategoryMapper();
            categoryMapper.GetCategories();

            if (Master != null)
            {
                ContentPlaceHolder ph = (ContentPlaceHolder)Master.FindControl("MainContent");
                servicesList = (global::ServicesList.ServicesList)ph.FindControl("servicesList");
                servicesList = (global::ServicesList.ServicesList)ph.FindControl("servicesList");
                servicesList.Services = ServicesToServicesDLL(servicesMapper.GetServicesWithPrice());
                servicesList.Categories = CategoriesToCategoriesDLL(categoryMapper.GetCategories());
            }
        }
    }
}