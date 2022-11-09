namespace DataBasesHW7.Models;

public class Country
{
    public string? Name { get; set; }
    
    public string? Id { get; set; }
    
    public int AreaSqkm { get; set; }
    
    public int Population { get; set; }
        
    public ICollection<Olympiad>? Olympiads { get; set; }
}