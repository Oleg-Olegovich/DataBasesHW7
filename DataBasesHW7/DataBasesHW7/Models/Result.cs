namespace DataBasesHW7.Models;

public class Result
{
    public string? Id { get; set; }
    
    public string? EventId { get; set; }
    
    public string? SportsmanId { get; set; }
    
    public string? Medal { get; set; }
    
    public double SportsmanResult { get; set; }

    public Sportsman Sportsman { get; set; }
    
    public Event Event { get; set; }
}