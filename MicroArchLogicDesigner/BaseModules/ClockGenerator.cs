namespace MicroArchLogicDesigner.BaseModules;
public class ClockGenerator : IFeeder
{
    public string Name { get; init; }
    public IList<IConsumer> Consumers { get; init; }
    public Dictionary<string, IEnumerable<string>> OutputsConfig { get; init; }

    public ClockGenerator()
    {
        Name = "Clock";
        OutputsConfig = new Dictionary<string, IEnumerable<string>>();
        Consumers= new List<IConsumer>();
    }
}
