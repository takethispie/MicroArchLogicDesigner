namespace MicroArchLogicDesigner.BaseModules;

public class LogicGate : IModule
{
    public string Name { get; init; }
    public LogicType Type { get; init; }
    public Pin InputA { get; init; }
    public Pin InputB { get; init; }
    public Pin Result { get; init; }

    public LogicGate(string name, LogicType type)
    {
        Name = name;
        InputA = new Pin("inputA", 1, false, Name) { OnValue = OnInputChange };
        InputB = new Pin("inputB", 1, false, Name) { OnValue = OnInputChange };
        Result = new Pin("result", 1, true, Name);
        Type = type;
    }

    private Value typeSwitch(LogicType logicType) => logicType switch {
        LogicType.And => new Value(InputA.Buffer.Get() == 1 && InputB.Buffer.Get() == 1 ? 1 : 0),
        LogicType.Or => new Value(InputA.Buffer.Get() == 1 || InputB.Buffer.Get() == 1 ? 1 : 0),
        LogicType.XNor => new Value(InputA.Buffer.Get() == InputB.Buffer.Get() ? 1 : 0),
        _ => new Value(0)
    };

    private void OnInputChange(Value value) => Result.Set(typeSwitch(Type));
}

public enum LogicType
{
    And,
    Or,
    XNor
}
