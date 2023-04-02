namespace MicroArchLogicDesigner.ArithmeticLogic;

public class Adder : IModule
{
    public string Name { get; init; }
    public int Width { get; }
    public Pin InputA { get; init; }
    public Pin InputB { get; init; }
    public Pin Result { get; init; }

    public Adder(string name, int width)
    {
        Name = name;
        Width = width;
        InputA = new Pin("inputA", width, false, Name) { OnValue = OnInputAChange };
        InputB = new Pin("inputB", width, false, Name) { OnValue = OnInputBChange };
        Result = new Pin("result", width, true, Name);
    }

    private void OnInputAChange(Value value)
    {
        var result = value + InputB.Buffer;
        Result.Set(result);
    }

    private void OnInputBChange(Value value)
    {
        var result = InputA.Buffer + value;
        Result.Set(result);
    }
}
