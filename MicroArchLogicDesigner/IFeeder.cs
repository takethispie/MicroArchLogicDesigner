namespace MicroArchLogicDesigner;

public interface IFeeder : IModule
{
    public string Name { get; init; }
    public IList<IConsumer> Consumers { get; init; }
    public Dictionary<string, IEnumerable<string>> OutputsConfig { get; init; }

    public void AddOutputRouting(string moduleName, IEnumerable<string> connectedOutput)
    {
        if (!Consumers.Any(x => x.Name == moduleName)) throw new ArgumentException("module is not in the list of consumers !");
        OutputsConfig.Add(moduleName, connectedOutput);
    }
}
