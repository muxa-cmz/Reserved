using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reserved.Models.DomainModels
{
    public class Table
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public Table(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}