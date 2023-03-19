﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProjectCMS.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime First_Closure { get; set; }
        public DateTime Last_Closure { get; set;}
        public ICollection<Idea> Ideas { get; set; }
    }
}
