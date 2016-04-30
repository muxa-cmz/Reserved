﻿using System;

namespace Reserved.Models.DomainModels
{
    [Serializable]
    public class Category
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}