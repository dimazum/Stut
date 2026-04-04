using System.Collections.Generic;

namespace stutvds.Models.ClientDto;

public class LocalizationDto
{
    public Dictionary<string,string> Translations { get; set; }
    public string Lang { get; set; }
}