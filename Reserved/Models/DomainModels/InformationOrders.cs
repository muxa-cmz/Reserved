using System;

namespace Reserved.Models.DomainModels
{
    [Serializable]
    public class InformationOrders
    {
        public int Id { get; set; }
        public int IdDay { get; set; }
        public int IdBox { get; set; }
        public int IdOrder { get; set; }
        public int IdTimeInterval { get; set; }

        public InformationOrders(int id, int idDay, int idBox, int idOrder, int idTimeInterval)
        {
            Id = id;
            IdDay = idDay;
            IdBox = idBox;
            IdOrder = idOrder;
            IdTimeInterval = idTimeInterval;
        }
    }
}