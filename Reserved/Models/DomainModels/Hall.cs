using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reserved.Models.DomainModels
{
    public class Hall
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String NumberOfSeats { get; set; }
        public String Description { get; set; }
        public String PathToImage { get; set; }

        public Hall(int id, String name, String numberOfSeats, String description, String pathToImage)
        {
            Id = id;
            Name = name;
            NumberOfSeats = numberOfSeats;
            Description = description;
            PathToImage = pathToImage;
        }
    }
}