namespace MicroArchLogicDesigner.BaseModules;

public class ALU : IModule
{
    public string Name { get; init; }
    public Pin Control { get; init; }
    public Pin InputA { get; init; }
    public Pin InputB { get; init; }
    public Pin Result { get; init; }
    
    public ALU(string name, int width) { 
        Name = name;
        Control = new Pin("controler", 4, false, Name) { OnValue = OnControlChange };
        InputA = new Pin("inputA", width, false, Name) { OnValue = OnInputAChange };
        InputB = new Pin("inputB", width, false, Name) { OnValue = OnInputBChange };
        Result = new Pin("result", width, true, Name);
    }

    private Value OpSwitch(Value value)
    {
        return value.Get() switch {
            1 => InputA.Buffer + InputB.Buffer,
            2 => InputA.Buffer - InputB.Buffer,
            3 => InputA.Buffer * InputB.Buffer,
            4 => InputA.Buffer / InputB.Buffer,
            _ => InputA.Buffer
        };
    }

    private void OnControlChange(Value value) => Result.Set(OpSwitch(value));

    private void OnInputAChange(Value value) => Result.Set(value + InputB.Buffer);

    private void OnInputBChange(Value value) => Result.Set(InputA.Buffer + value);
}
