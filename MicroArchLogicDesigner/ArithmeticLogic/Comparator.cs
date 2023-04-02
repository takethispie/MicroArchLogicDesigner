namespace MicroArchLogicDesigner.ArithmeticLogic;

public class Comparator : IModule
{
    public string Name { get; init; }
    public Pin InputA { get; init; }
    public Pin InputB { get; init; }
    public Pin LowerThan { get; init; }
    public Pin Equal { get; init; }
    public Pin GreaterThan { get; init; }

    public Comparator(string name, int width)
    {
        Name = name;
        InputA = new Pin("inputA", width, false, Name) { OnValue = OnInputAChange };
        InputB = new Pin("inputB", width, false, Name) { OnValue = OnInputBChange };
        LowerThan = new Pin("lowerThan", 1, true, Name);
        Equal = new Pin("Equal", 1, true, Name);
        GreaterThan = new Pin("greaterThan", 1, true, Name);
    }

    private void OnInputAChange(Value value)
    {
        LowerThan.Set(new Value(value.Get() < InputB.Buffer.Get() ? 1 : 0));
        Equal.Set(new Value(value.Get() == InputB.Buffer.Get() ? 1 : 0));
        GreaterThan.Set(new Value(value.Get() > InputB.Buffer.Get() ? 1 : 0));
    }

    private void OnInputBChange(Value value)
    {
        LowerThan.Set(new Value(InputA.Buffer.Get() < value.Get() ? 1 : 0));
        Equal.Set(new Value(InputA.Buffer.Get() == value.Get() ? 1 : 0));
        GreaterThan.Set(new Value(InputA.Buffer.Get() > value.Get() ? 1 : 0));
    }
}
