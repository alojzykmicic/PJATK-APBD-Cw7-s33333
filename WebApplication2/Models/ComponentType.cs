namespace WebApplication2.Models;

public class ComponentType
{
    public int Id { get; set; }
    public string Abbreviation { get; set; }
    public string Name { get; set; }
    
    public ICollection<Component> Components { get; set; } = new List<Component>();
}