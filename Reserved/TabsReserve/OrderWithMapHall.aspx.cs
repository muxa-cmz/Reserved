using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using Reserved.Models.Mappers;
using HallHl = HallList.Hall;
using HallM = HallMap.Hall;
using Hall = Reserved.Models.DomainModels.Hall;

namespace Reserved.TabsReserve
{
    public partial class OrderWithMapHall : System.Web.UI.Page
    {
        private HallList.HallList hallsList;
        private HallMap.HallMap hallsMapList;

        private List<HallHl> HallsToHallsHl(List<Hall> halls)
        {
            return halls.Select(hall => new HallHl(hall.Id, hall.Name, hall.NumberOfSeats, hall.Description, hall.PathToImage)).ToList();
        }

        private List<HallM> HallsToHallsM(List<Hall> halls)
        {
            return halls.Select(hall => new HallM(hall.Id, hall.Name, hall.NumberOfSeats, hall.Description, hall.PathToImage)).ToList();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HallMapper hallMapper = new HallMapper();
            List<Hall> halls = new List<Hall>();

            halls.AddRange(hallMapper.GetHalls());

            if (Master != null)
            {
                ContentPlaceHolder ph = (ContentPlaceHolder)Master.FindControl("MainContent");
                hallsList = (HallList.HallList)ph.FindControl("hallList");
                hallsMapList = (HallMap.HallMap)ph.FindControl("hallMap");
                hallsList.Halls = HallsToHallsHl(halls);
                hallsMapList.Halls = HallsToHallsM(halls);
            }
        }

        [WebMethod]
        public static String ViewFreeTablesHall(string hallId, string date, string time)
        {
            #region Находим список забранированных столов на необходимый день
            ReservedTablesMapper reservedTablesMapper = new ReservedTablesMapper();
            DayMapper dayMapper = new DayMapper();
            List<int> busyTables = new List<int>();

            date = date.Substring(6, 4) + "-" + date.Substring(3, 2) + "-" + date.Substring(0, 2);
            int idDay = dayMapper.GetDayId(date);   // id даты бронирования
            busyTables.AddRange(reservedTablesMapper.GetBusyTables(idDay.ToString()));
            #endregion

            #region Список всех столов в зале
            HallMapper hallMapper = new HallMapper();
            List<int> tablesInHall = new List<int>();
            tablesInHall.AddRange(hallMapper.GetTablesInHall(hallId));
            #endregion

            
            #region Свободные столы в зале
            foreach (var table in busyTables)
            {
                tablesInHall.Remove(table);
            }
            #endregion

            String cookieString = tablesInHall.Aggregate("", (current, table) => current + (table + " "));
            cookieString = cookieString.Substring(0, cookieString.Length - 1);

            return cookieString;
        }
    }
}