namespace MicroArchLogicDesigner.BaseModules;

public class Demultiplexer : IModule
{
    public string Name { get; init; }
    public Pin Input { get; init; }
    public List<Pin> Output { get; private set; }

    public Pin Control { get; init; }
    public int Width { get; init; }

    public Demultiplexer(string name, int width, int outputNumber)
    {
        Name = name;
        Width = width;
        Output = new List<Pin>();
        var binSize = new Value(outputNumber).ToBin().Length;
        foreach (int i in Enumerable.Range(0, outputNumber))
        {
            Output.Add(new Pin("output" + i, width, true, Name));
        }
        Control = new Pin("control", binSize, false, Name) { OnValue = OnSelectedChange };
        Input = new Pin("input", width, false, Name) { OnValue = OnValueChange };
    }

    private void OnSelectedChange(Value value)
    {
        var selected = value.Get();
        if (selected > Output.Count) throw new IndexOutOfRangeException("");
        Output.ForEach(x => x.Set(new Value(0)));
        Output[selected].Set(Input.Buffer);
    }

    private void OnValueChange(Value value)
    {
        var selected = Control.Buffer.Get();
        if (selected > Output.Count) throw new IndexOutOfRangeException("");
        Output.ForEach(x => x.Set(new Value(0)));
        Output[selected].Set(value);
    }
}
