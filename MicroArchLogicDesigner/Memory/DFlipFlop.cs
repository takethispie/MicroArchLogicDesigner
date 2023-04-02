using System.Drawing;

namespace MicroArchLogicDesigner.Memory;

public class DFlipFlop : Clockable, IModule
{
    public string Name { get; init; }
    public int Size { get; init; }
    public Pin Clock { get; init; }
    public Pin Input { get; init; }
    public Pin Output { get; init; }
    public Value Value { get; private set; }

    public DFlipFlop(string name, int size)
    {
        Name = name;
        Size = size;
        Clock = new Pin("clock", 1, false, name) { OnValue = ProcessClockEvent };
        Input = new Pin("input", size, false, name);
        Output = new Pin("output", size, true, name);
        Value = new Value(0);
    }

    public IEnumerable<string> GetInputNames() => new List<string> { "clock", "input", "output" };

    public override void OnFallingEdgeClock()
    {
    }

    public override void OnHighClock()
    {
    }

    public override void OnLowClock()
    {
    }

    public override void OnRisingEdgeClock()
    {
        Value value = new Value(Input.Buffer.Get());
        Value.Set(value.Get());
        Output.Set(value);
    }
}
