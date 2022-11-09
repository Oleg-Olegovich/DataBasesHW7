namespace DataBasesHW7.Models;

public class Event
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? EventType { get; set; }

    public string? OlympiadId { get; set; }

    public int IsTeamEvent { get; set; }
        
    public int NumSportsmenInTeam { get; set; }
    
    public string? ResultNotedIn { get; set; }

    public int CountyId { get; set; }
    
    public ICollection<Result>? Results { get; set; }
        
    public Olympiad? Olympiad { get; set; }
}