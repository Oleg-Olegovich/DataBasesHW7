using DataBasesHW7.Enums;

namespace DataBasesHW7.Models;

public class Sportsman
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? CountryId { get; set; }

    public Genders Gender { get; set; }

    public DateTime BirthDate { get; set; }

    public ICollection<Result>? Results { get; set; }
}