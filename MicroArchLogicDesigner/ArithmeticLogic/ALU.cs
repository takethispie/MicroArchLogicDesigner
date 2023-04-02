namespace MicroArchLogicDesigner.ArithmeticLogic;

public class ALU : IModule
{
    public string Name { get; init; }
    public Pin Control { get; init; }
    public Pin InputA { get; init; }
    public Pin InputB { get; init; }
    public Pin Result { get; init; }
    public Pin Constant { get; init; }

    public ALU(string name, int width)
    {
        Name = name;
        Control = new Pin("controler", 4, false, Name) { OnValue = OnControlChange };
        InputA = new Pin("inputA", width, false, Name) { OnValue = OnInputAChange };
        InputB = new Pin("inputB", width, false, Name) { OnValue = OnInputBChange };
        Constant = new Pin("constantIn", width / 2, false, Name) { OnValue = OnConstantChange };
        Result = new Pin("result", width, true, Name);
    }

    private void OnConstantChange(Value value) => Result.Set(OpSwitch(Control.Buffer));


    private Value OpSwitch(Value value)
    {
        return value.Get() switch
        {
            1 => InputA.Buffer + InputB.Buffer,
            2 => InputA.Buffer - InputB.Buffer,
            3 => InputA.Buffer * InputB.Buffer,
            4 => InputA.Buffer / InputB.Buffer,
            5 => Value.FromBin(Constant.Buffer.ToBin().PadLeft(InputA.Size, '0')),
            6 => Value.FromBin(Constant.Buffer.ToBin() + InputA.Buffer.ToBin()[(InputA.Size / 2)..]),
            _ => Value.FromBin(Constant.Buffer.ToBin().PadLeft(InputA.Size, '0')),
        };
    }

    private void OnControlChange(Value value) => Result.Set(OpSwitch(value));

    private void OnInputAChange(Value value) => Result.Set(OpSwitch(Control.Buffer));

    private void OnInputBChange(Value value) => Result.Set(OpSwitch(Control.Buffer));

}