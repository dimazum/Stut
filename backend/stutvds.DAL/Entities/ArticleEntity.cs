using System;
using StopStatAuth_6_0.Entities.Enums;

namespace StopStatAuth_6_0.Entities
{
    public class ArticleEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Locale { get; set; }
        public AgeGroup AgeGroup { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
