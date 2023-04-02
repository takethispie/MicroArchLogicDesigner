namespace MicroArchLogicDesigner.Routing;

public class Multiplexer : IModule
{
    public string Name { get; init; }
    public List<Pin> Inputs { get; private set; }
    public Pin Output { get; init; }
    public Pin Control { get; init; }
    public int Width { get; init; }

    public Multiplexer(string name, int width, int inputNumber)
    {
        Name = name;
        Width = width;
        Inputs = new List<Pin>();
        var binSize = Convert.ToString(inputNumber - 1, 2).Length;
        foreach (int i in Enumerable.Range(0, inputNumber))
            Inputs.Add(new Pin("input" + i, width, false, Name)
            {
                OnValue = (val) => OnValueChange(val, i)
            });
        Control = new Pin("control", binSize, false, Name, true) { OnValue = OnSelectedChange };
        Output = new Pin("output", width, true, Name, true);
    }

    private void OnSelectedChange(Value value)
    {
        var selected = value.Get();
        if (selected > Inputs.Count) throw new IndexOutOfRangeException("");
        Output.Set(new Value(Inputs[selected].Buffer.Get()));
    }

    private void OnValueChange(Value value, int inputId)
    {
        var selected = Control.Buffer.Get();
        if (selected == inputId) Output.Set(value);
    }
}
