namespace MicroArchLogicDesigner.Base;
public class ClockGenerator : IModule
{
    public string Name { get; init; }
    public Pin Output { get; init; }

    public ClockGenerator()
    {
        Name = "Clock";
        Output = new Pin("output", 1, true, Name);
    }

    public void Reset() => Output.Set(new Value(ClockEvent.Low));

    public void DoQuarterTick() => Output.Set(NextTickState());

    private Value NextTickState() => Output.Buffer.Get() switch
    {
        0 => new Value(1),
        1 => new Value(2),
        2 => new Value(3),
        3 => new Value(0),
        var v => throw new IndexOutOfRangeException("unknown tick state: " + v.ToString())
    };
}
