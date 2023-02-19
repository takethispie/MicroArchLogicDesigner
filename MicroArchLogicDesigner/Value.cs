namespace MicroArchLogicDesigner;

public class Value
{
    private int value { get; set; }

    public Value(int value) { this.value = value; }
    public Value(string value) { this.value = int.Parse(value); }
    public Value(ClockEvent clkEvent) { this.value = FromClockEvent(clkEvent); }

    public override string ToString() => value.ToString();

    public ClockEvent ToClockEvent() => value switch {
        0 => ClockEvent.Low, 
        1 => ClockEvent.Rising, 
        2 => ClockEvent.High, 
        3 => ClockEvent.Falling,
        _ => ClockEvent.Low
    };

    public void Set(int value) => this.value = value;
    
    public int Get() => value;

    private int FromClockEvent(ClockEvent clkEvent) => clkEvent switch
    {
        ClockEvent.Low => 0,
        ClockEvent.Rising => 1,
        ClockEvent.High => 2,
        ClockEvent.Falling => 3,
        _ => 0
    };

    public static Value operator +(Value A, Value B) => new Value(A.Get() + B.Get());
    public static Value operator -(Value A, Value B) => new Value(A.Get() - B.Get());
    public static Value operator *(Value A, Value B) => new Value(A.Get() * B.Get());
    public static Value operator /(Value A, Value B) => new Value(A.Get() / B.Get());
}
