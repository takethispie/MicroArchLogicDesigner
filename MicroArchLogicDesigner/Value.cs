namespace MicroArchLogicDesigner;

public class Value
{
    private int value { get; init; }
    public override string ToString() => value.ToString();

    public Value(int value) { this.value = value; }
    public Value(string value) { this.value = int.Parse(value); }
}
