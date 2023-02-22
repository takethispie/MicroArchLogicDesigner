namespace MicroArchLogicDesigner;

public class Value
{
    private int value { get; set; }

    public Value(int value) { this.value = value; }
    public Value(string value) { this.value = int.Parse(value); }
    public Value(ClockEvent clkEvent) { value = FromClockEvent(clkEvent); }

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
    public string ToHex() => ToHex(value);
    public string ToHex(int val) => val.ToString("X");
    public string ToBin() => Convert.ToString(value, 2);
    public string ToBin(int val) => Convert.ToString(val, 2);


    private int FromClockEvent(ClockEvent clkEvent) => clkEvent switch
    {
        ClockEvent.Low => 0,
        ClockEvent.Rising => 1,
        ClockEvent.High => 2,
        ClockEvent.Falling => 3,
        _ => 0
    };

    public static Value operator +(Value A, Value B) => new(A.Get() + B.Get());
    public static Value operator -(Value A, Value B) => new(A.Get() - B.Get());
    public static Value operator *(Value A, Value B) => new(A.Get() * B.Get());
    public static Value operator /(Value A, Value B) => new(A.Get() / B.Get());

    public static Value FromHex(string value) => new(Convert.ToInt32(value, 16));
}
