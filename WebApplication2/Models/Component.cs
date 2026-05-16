namespace WebApplication2.Models;

public class Component
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ComponentManufacturersID { get; set; }
    public int ComponentTypesId { get; set; }

    public ComponentManufacturer Manufacturer { get; set; }
    public ComponentType Type { get; set; }
    public ICollection<PCComponent> PcComponents { get; set; } = new List<PCComponent>();
}