using System;

namespace stutvds.Views.Shared.Models;

public class IdeaBlockModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Title { get; init; } = "В чем идея:";
    public string Content { get; set; } = "";
}