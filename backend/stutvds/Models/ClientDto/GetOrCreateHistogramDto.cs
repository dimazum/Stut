namespace stutvds.Models.ClientDto;

public class GetOrCreateHistogramDto
{
    public string Name { get; set; }
    public string InitText { get; set; }
    public bool SaveToDb { get; set; }
}