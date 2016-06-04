using System;
using System.Collections.Generic;

namespace Reserved.Models.DomainModels
{
    [Serializable]
    public class Service
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Notation { get; set; }
        public String Duration { get; set; }  // продолжительность работы
        public String PathToImage { get; set; }
        public int IdCategory { get; set; }
        public int IdSubCategory { get; set; }
        public Dictionary<String, String> Prices { get; set; }

        public Service(int id, String name,  String notation, 
                       String duration, String pathToImage,
                       int idCategory, int idSubCategory)
        {
            Id = id;
            Name = name;
            Notation = notation;
            Duration = duration;
            PathToImage = pathToImage;
            IdCategory = idCategory;
            IdSubCategory = idSubCategory;
            Prices = new Dictionary<String, String>();
        }

        public Service(String name, 
                       String notation, String duration,
                       String pathToImage, int idCategory, int idSubCategory)
        {
            Name = name;
            Notation = notation;
            Duration = duration;
            PathToImage = pathToImage;
            IdCategory = idCategory;
            IdSubCategory = idSubCategory;
            Prices = new Dictionary<String, String>();
        }

        public Service(int id, String name)
        {
            Id = id;
            Name = name;
            Prices = new Dictionary<String, String>();
        }
    }
}