using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Reserved.Models.DomainModels;
using Reserved.Models.Mappers;
using TileWithCheckBox = TileWithCheckBox.TileWithCheckBox;

namespace Reserved.TabsReserve
{
    public partial class OrderWithListServices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Service> services = new List<Service>();
            ServicesMapper servicesMapper = new ServicesMapper();

            List<Category> categories = new List<Category>();
            CategoryMapper categoriesMapper = new CategoryMapper();
            
            categories.AddRange(categoriesMapper.GetCategory());

            foreach (var category in categories)
            {
                services.AddRange(servicesMapper.GetServicesByCategory(category.Id));
            }


            if (Master != null)
            {
                ContentPlaceHolder placeHolder = (ContentPlaceHolder)Master.FindControl("MainContent");
                var div1 = placeHolder.FindControl("accordionPanel");
                global::TileWithCheckBox.TileWithCheckBox weBox = new global::TileWithCheckBox.TileWithCheckBox
                {
                    ID = "TileWithCheckBox1",
                    Title = "Бесконтактная ;bjj",
                    Image = "../Image/39.jpg",
                    CheckBoxName = "checkbox1",
                    ValueInput = "ch2"
                };
                placeHolder.Controls.Add(weBox);
            }
            
        }

    }
}


