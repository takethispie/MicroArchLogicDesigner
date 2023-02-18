namespace MicroArchLogicDesigner;

public interface IConsumer : IModule
{
    public string Name { get; init; }
    public void Receive(IBusData busData);
    public IEnumerable<string> GetInputNames();
    public IEnumerable<(string source, string target)> InputConfigs { get; }
    public void AddInputRouting(Pin sourceModuleOutput, Pin input);
}

