using System;

namespace MinorLinq.Models
{
    public class Post
    {
        public int Id { get; set; }
        public bool IsHidden { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime EditedOn { get; set; }
        public int ViewCount { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string DefaultLanguageCode { get; set; }
    }
}