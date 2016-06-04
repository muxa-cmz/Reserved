

using System;

namespace Reserved.Models.DomainModels
{
    public class Box
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public Box(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}