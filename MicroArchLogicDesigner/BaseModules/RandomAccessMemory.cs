﻿namespace MicroArchLogicDesigner.BaseModules;
public class RandomAccessMemory : Clockable, IModule
{
    public string Name { get; init; }
    public Pin ReadAddress { get; init; }
    public Pin WriteAddress { get; init; }
    public Pin DataOut { get; init; }
    public Pin DataIn { get; init; }
    public Pin ClockIn { get; init; }
    public Pin WriteEnable { get; init; }
    private int[] data;

    public RandomAccessMemory(string name, int dataWidth, int adressWidth)
    {
        if (dataWidth > 64 || adressWidth > 64) throw new ArgumentOutOfRangeException();
        Name = name;
        ReadAddress = new Pin("readAddrIn", adressWidth, false, Name);
        WriteAddress = new Pin("writeAddrIn", adressWidth, false, Name);
        DataIn = new Pin("dataIn", dataWidth, false, Name);
        DataOut = new Pin("dataOut", dataWidth, true, Name);
        ClockIn = new Pin("clockIn", 1, false, Name) { OnValue = ProcessClockEvent };
        WriteEnable = new Pin("writeEnable", 1, false, Name);
        data = new int[2^dataWidth];
    }

    public override void OnLowClock() { }
    public override void OnHighClock() { }
    public override void OnRisingEdgeClock() {
        if(WriteEnable.Buffer.Get() == 1)
            data[WriteAddress.Buffer.Get()] = DataIn.Buffer.Get();
        DataOut.Set(new Value(data[ReadAddress.Buffer.Get()]));
    }
    public override void OnFallingEdgeClock() { }
}