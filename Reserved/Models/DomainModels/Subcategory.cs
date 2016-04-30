using System;

namespace Reserved.Models.DomainModels
{
    [Serializable]
    public class Subcategory
    {
        private int Id { get; set; }
        public String Name { get; set; }

        public Subcategory(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}