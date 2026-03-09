using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StopStatAuth_6_0.Entities.Base;

namespace stutvds.DAL.Entities;

public class Histogram: Entity
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public ICollection<CharItem> Chars { get; set; } = new List<CharItem>();
}

public class CharItem:  Entity
{
    public string Char { get; set; } = string.Empty;
    public float Air { get; set; }
    public int Order { get; set; }
    public int HistogramId { get; set; }
    [ForeignKey("HistogramId")]
    public Histogram Histogram { get; set; } = null!;
}