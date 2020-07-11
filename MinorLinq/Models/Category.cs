using System;

namespace MinorLinq.Models
{
    public class Category
    {
        public int Id { get; set; }
        public bool IsHidden { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DefaultLanguageCode { get; set; }
    }
}