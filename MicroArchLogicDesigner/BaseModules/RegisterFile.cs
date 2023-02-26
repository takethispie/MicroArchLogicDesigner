namespace MicroArchLogicDesigner.BaseModules;

public class RegisterFile : Clockable, IModule
{
    public string Name { get; init; }
    public Pin ReadA { get; init; }
    public Pin ReadB { get; init; }
    public Pin Dest { get; init; }
    public Pin DataIn { get; init; }
    public Pin OutputA { get; init; }
    public Pin OutputB { get; init; }
    public Pin Clock { get; init; }

    private int[] data; 

    public RegisterFile(string name, int width, int registerCount)
    {
        var binSize = Convert.ToString(registerCount - 1, 2).Length;
        Name = name;
        ReadA = new Pin("readA", binSize, false, Name) { OnValue = OnInputAControlChange };
        ReadB = new Pin("readB", binSize, false, Name) { OnValue = OnInputBControlChange };
        Dest = new Pin("dest", binSize, false, Name);
        DataIn = new Pin("data", width, false, Name) { OnValue = OnDataInchange };
        Clock = new Pin("clock", 1, false, Name) { OnValue = onClock };
        OutputA = new Pin("outputA", width, true, Name);
        OutputB = new Pin("outputB", width, true, Name);
        data = new int[binSize];
        data[0] = 0;
    }

    private void onClock(Value value) => ProcessClockEvent(value);

    public override void OnLowClock() { }

    public override void OnHighClock() { }

    public void OnInputAControlChange(Value value) => OutputA.Set(new Value(data[ReadA.Buffer.Get()]));

    public void OnInputBControlChange(Value value) => OutputB.Set(new Value(data[ReadA.Buffer.Get()]));

    public void OnDataInchange(Value value)
    {
        if (Clock.Buffer.Get() != 1 || Dest.Buffer.Get() == 0 || Dest.Buffer.Get() >= data.Length) return;
        data[Dest.Buffer.Get()] = DataIn.Buffer.Get();
        OutputA.Set(new Value(data[ReadA.Buffer.Get()]));
        OutputB.Set(new Value(data[ReadA.Buffer.Get()]));
    }

    public override void OnRisingEdgeClock() {
        if(Dest.Buffer.Get() > 0)
            data[Dest.Buffer.Get()] = DataIn.Buffer.Get();
        OutputA.Set(new Value(data[ReadA.Buffer.Get()]));
        OutputB.Set(new Value(data[ReadA.Buffer.Get()]));
    }

    public override void OnFallingEdgeClock() { }
}
