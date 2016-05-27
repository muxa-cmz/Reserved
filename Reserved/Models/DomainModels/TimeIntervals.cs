using System;

namespace Reserved.Models.DomainModels
{
    [Serializable]
    public class TimeIntervals
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public TimeIntervals(int id, String name)
        {
            Id = id;
            Name = name;
        }
    }
}