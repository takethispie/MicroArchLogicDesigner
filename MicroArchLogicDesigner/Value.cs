namespace MicroArchLogicDesigner;

public class Value
{
    private int value { get; set; }
    private int width { get; set; }

    public Value(int value, int width = 32) { this.value = value; this.width = width; }
    public Value(string value, int width) { this.value = int.Parse(value); this.width = width; }
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
    public string ToHex() => value.ToString("X").PadLeft(width / 4, '0');
    public string ToBin() => Convert.ToString(value, 2).PadLeft(width, '0');
    public void Increment () => value++;
    public void Decrement () => value--;


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

    public static Value FromHex(string value) => new(Convert.ToInt32(value, 16), value.Length * 4);
    public static Value FromBin(string value) => new(Convert.ToInt32(value, 2), value.Length);

    public static Value One() => new(1);
    public static Value Zero() => new(0);
}
