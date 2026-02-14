using System.Collections.Generic;

namespace stutvds.Models.ClientDto;

public class HistogramDto
{
    public string Name { get; set; }
    public List<CharDto> Chars { get; set; }
}

public class CharDto
{
    public int Id { get; set; }
    public string Char { get; set; }
    public float Air { get; set; }
}
