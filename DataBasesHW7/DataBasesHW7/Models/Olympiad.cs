using DataBasesHW7.Enums;

namespace DataBasesHW7.Models;

public class Olympiad
{
    public string? Id { get; set; }
    
    public string? CountryId { get; set; }

    public string? City { get; set; }
    
    public int Year { get; set; }

    public DateTime StartDate { get; set; }
        
    public DateTime EndDate { get; set; }
        
    public Seasons Season { get; set; }
    
    public Country Country { get; set; }
        
    public ICollection<Event> Events { get; set; }
}