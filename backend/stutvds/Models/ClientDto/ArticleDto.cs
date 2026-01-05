using System;
using StopStatAuth_6_0.Entities.Enums;

namespace stutvds.Models.ClientDto;

public class ArticleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Topic { get; set; }
    public string Source { get; set; }
    public string Content { get; set; }
    public Language Language { get; set; }
    public AgeGroup AgeGroup { get; set; }
    public DateTime CreatedAt { get; set; }
}