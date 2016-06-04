using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reserved.Models.DomainModels
{
    public class Order
    {
        public int Id { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String Services { get; set; }

        public Order(int id, string lastName, string firstName, string services)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            Services = services;
        }

        public Order(string lastName, string firstName, string services)
        {
            LastName = lastName;
            FirstName = firstName;
            Services = services;
        }
    }
}