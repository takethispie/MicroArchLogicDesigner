using MicroArchLogicDesigner;

namespace ExampleCPU;
public class OpDecoder : IModule
{
    public string Name { get; init; }

    public Pin Input { get; init; }
    public Pin AluOut { get; init; }
    public Pin BranchJumpOut { get; init; }
    public Pin IOOut { get; init; }

    public OpDecoder(string name)
    {
        Name = name;
        Input = new Pin("input", 8, false, name) { OnValue = OnOpChange };
        AluOut = new Pin("aluOut", 4, true, name);
        BranchJumpOut = new Pin("branch_jump_out", 1, true, name);
        IOOut = new Pin("input_output_out", 2, true, name);
    }

    public void OnOpChange(Value value)
    {
        AluOut.Set(Value.Zero(4));
        BranchJumpOut.Set(Value.Zero(1));
        IOOut.Set(Value.Zero(2));
        ReadOnlySpan<char> opBin = value.ToBin();
        if(opBin is ['0','0','0','1', ..])
            AluOut.Set(Value.FromBin(opBin.ToString().Substring(4)));
        else if(opBin is ['0', '0', '1', '0', ..])
            BranchJumpOut.Set(Value.FromBin(opBin.ToString().Substring(7)));
        else if (opBin is ['0', '0', '1', '1', ..])
            IOOut.Set(Value.FromBin(opBin.ToString().Substring(6)));
    }
}
