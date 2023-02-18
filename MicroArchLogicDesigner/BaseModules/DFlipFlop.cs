using System.Drawing;

namespace MicroArchLogicDesigner.BaseModules;

public class DFlipFlop : IClockable, IFeeder, IConsumer
{
    public string Name { get; init; }
    public IList<IConsumer> Consumers { get; init; }
    public Dictionary<string, IEnumerable<string>> OutputsConfig { get; init; }
    public int Size { get; init; }
    public IEnumerable<(string source, string target)> InputConfigs { get; private set; }
    public Pin Clock { get; init; }
    public Pin Input { get; init; }
    public Pin Output { get; init; }
    public Value Value { get; private set; }

    public DFlipFlop(string name, int size)
    {
        Name = name;
        Size = size;
        Consumers = new List<IConsumer>();
        OutputsConfig = new Dictionary<string, IEnumerable<string>>();
        InputConfigs = new List<(string source, string target)>();
        Clock = new Pin("clock", 1, false, name);
        Input = new Pin("input", size, false, name);
        Output = new Pin("output", size, true, name);
        Value = new Value(0);
    }

    public void AddInputRouting(Pin sourceModuleOutput, Pin input)
    {
        if (sourceModuleOutput.Size != input.Size) throw new ArgumentException("width do not match");
        InputConfigs = input switch
        {
            _ when sourceModuleOutput.Size == Clock.Size && input.Name == Clock.Name
            && !InputConfigs.Any(x => x.source == sourceModuleOutput.Name && x.target == Clock.Name)
                => InputConfigs.Append((sourceModuleOutput.Name, Clock.Name)),
            _ when sourceModuleOutput.Size == Input.Size && input.Name == Input.Name
            && !InputConfigs.Any(x => x.source == sourceModuleOutput.Name && x.target == Input.Name)
                => InputConfigs.Append((sourceModuleOutput.Name, Input.Name)),
            _ => InputConfigs
        };

    }

    public IEnumerable<string> GetInputNames() => new List<string> { "clock", "input", "output" };

    public void OnFallingEdgeClock()
    {
    }

    public void OnHighClock()
    {
    }

    public void OnLowClock()
    {
    }

    public void OnRisingEdgeClock()
    {
        if (!Consumers.Any()) return;
    }

    public void Receive(IBusData busData)
    {
        busData.
    }
}
