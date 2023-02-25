using MicroArchLogicDesigner;

namespace ExampleCPU;

public class InstructionDecoder : IModule
{
    public string Name { get; init; }

    public Pin InstructionIn { get; init; }
    public Pin OpOut { get; init; }
    public Pin DestOut { get; init; }
    public Pin First { get; init; }
    public Pin Second { get; init; }
    public Pin Constant { get; init; }

    public InstructionDecoder(string name)
    {
        Name = name;
        InstructionIn = new Pin("instIn", 32, false, Name) { OnValue = OnInstructionChange };
        OpOut = new Pin("opOut", 8, true, Name);
        DestOut = new Pin("destOut", 4, true, Name);
        First = new Pin("firstOperand", 4, true, Name);
        Second = new Pin("secondOperand", 4, true, Name);
        Constant = new Pin("constantOut", 16, true, Name);
    }

    public void OnInstructionChange(Value value)
    {
        string binInst = value.ToBin().PadLeft(32, '0');
        OpOut.Set(Value.FromBin(binInst.Substring(0, 8)));
        DestOut.Set(Value.FromBin(binInst.Substring(8, 4)));
        First.Set(Value.FromBin(binInst.Substring(12, 4)));
        Second.Set(Value.FromBin(binInst.Substring(16, 4)));
        Constant.Set(Value.FromBin(binInst.Substring(16)));
    }
}
