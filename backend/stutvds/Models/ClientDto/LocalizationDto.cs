using System.Collections.Generic;

namespace stutvds.Models.ClientDto;

public class LocalizationDto
{
    public string Version { get; set; }
    public string Lang { get; set; }
    public Dictionary<string,string> Translations { get; set; }
}