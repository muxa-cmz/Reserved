using System;

namespace Reserved.Models.DomainModels
{
    [Serializable]
    public class Service
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Price { get; set; }
        public String Notation { get; set; }
        public String Duration { get; set; }  // продолжительность работы
        public String PathToImage { get; set; }
        public int IdCategory { get; set; }
        public int IdSubCategory { get; set; }

        public Service(int id, String name, 
                       String price, String notation, 
                       String duration, String pathToImage,
                       int idCategory, int idSubCategory)
        {
            Id = id;
            Name = name;
            Price = price;
            Notation = notation;
            Duration = duration;
            PathToImage = pathToImage;
            IdCategory = idCategory;
            IdSubCategory = idSubCategory;
        }

        public Service(String name, String price, 
                       String notation, String duration,
                       String pathToImage, int idCategory, int idSubCategory)
        {
            Name = name;
            Price = price;
            Notation = notation;
            Duration = duration;
            PathToImage = pathToImage;
            IdCategory = idCategory;
            IdSubCategory = idSubCategory;
        }

        public Service(int id, String name)
        {
            Id = id;
            Name = name;
        }
    }
}