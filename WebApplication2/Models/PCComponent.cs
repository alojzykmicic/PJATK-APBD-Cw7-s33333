namespace WebApplication2.Models;

public class PCComponent
{
    public int PCId { get; set; }
    public string ComponentCode { get; set; }
    public int Amount { get; set; }

    public PC PC { get; set; }
    public Component Component { get; set; }
}