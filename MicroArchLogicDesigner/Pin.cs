namespace MicroArchLogicDesigner;

public class Pin
{
    public bool IsOutput { get; init; }
    public string Name { get; init; }
    public int Size { get; init; }
    public Value Buffer { get; set; }

    public Pin(string name, int size, bool isoutput, string parentName)
    {
        Name = parentName + "-" + name;
        Size = size;
        IsOutput = isoutput;
        Buffer = new Value(0);

    }
}

