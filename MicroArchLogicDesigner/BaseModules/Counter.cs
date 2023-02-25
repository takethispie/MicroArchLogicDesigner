using MicroArchLogicDesigner;

namespace ExampleCPU;
public class Counter : Clockable, IModule
{
    public string Name { get; init; }

    public Pin Clock { get; init; }
    public Pin Data { get; init; }
    public Pin Load { get; init; }
    public Pin Output { get; init; }
    public Pin EndOfCounter { get; init; }
    public Value Value { get; init; }

    private int width;
    private int maxCount;

    public Counter(string name, int width) {
        if(width > 64) throw new ArgumentOutOfRangeException("width is higher than 64 bit");
        Name = name;
        this.width = width;
        maxCount= 2^width;
        Clock = new Pin("clock", 1, false, name) { OnValue = ProcessClockEvent };
        Data = new Pin("data", width, false, name);
        Load = new Pin("load", 1, false, name);
        Output = new Pin("output", width, true, name);
        EndOfCounter = new Pin("end", 1, true, name);
        Value = Value.Zero();
    }

    public override void OnLowClock() { }
    public override void OnHighClock() { }
    public override void OnRisingEdgeClock() {
        EndOfCounter.Set(Value.Zero());
        if (Load.Buffer.Get() == 1) {
            Value.Set(Data.Buffer.Get());
            Output.Set(Data.Buffer);
        } else {
            if(Value.Get() == maxCount) {
                Value.Set(0);
                Output.Set(Value.Zero());
                EndOfCounter.Set(Value.One());
            } else {
                Value.Increment();
                Output.Set(Value);
            }
        }
    }
    public override void OnFallingEdgeClock() { }
}
