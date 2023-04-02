using ExampleCPU;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace MicroArchLogicDesigner.Tests;

public class ExampleCPUModuleTests
{
    [Fact]
    public void Should_Decode_Two_Operand_Instruction()
    {
        var decoder = new InstructionDecoder("decoder1");
        decoder.InstructionIn.Receive(Value.FromBin("00000001010000010010000000000000"));
        Assert.Equal("00000001", decoder.OpOut.Buffer.ToBin());
        Assert.Equal("01", decoder.OpOut.Buffer.ToHex());

        Assert.Equal("0100", decoder.DestOut.Buffer.ToBin());
        Assert.Equal("4", decoder.DestOut.Buffer.ToHex());

        Assert.Equal("0001", decoder.First.Buffer.ToBin());
        Assert.Equal("1", decoder.First.Buffer.ToHex());

        Assert.Equal("0010", decoder.Second.Buffer.ToBin());
        Assert.Equal("2", decoder.Second.Buffer.ToHex());
    }

    [Fact]
    public void Should_Decode_Constant_Instruction()
    {
        var decoder = new InstructionDecoder("decoder1");
        decoder.InstructionIn.Receive(Value.FromBin("00000001010000010010000000000000"));
        Assert.Equal("00000001", decoder.OpOut.Buffer.ToBin());
        Assert.Equal("01", decoder.OpOut.Buffer.ToHex());

        Assert.Equal("0010000000000000", decoder.Constant.Buffer.ToBin());
        Assert.Equal("2000", decoder.Constant.Buffer.ToHex());
    }

    [Fact]
    public void Should_load_alu_op() 
    {
        var opDec = new OpDecoder("opDecoder");
        opDec.Input.Receive(Value.FromBin("00010011"));
        Assert.Equal("0011", opDec.AluOut.Buffer.ToBin());
        opDec.Input.Receive(Value.FromBin("00110011"));
        Assert.Equal("0000", opDec.AluOut.Buffer.ToBin());
        Assert.Equal("11", opDec.IOOut.Buffer.ToBin());
    }

    [Fact]
    public void Should_merge_two_data()
    {
        var InputA = new Pin("inputB", 32, false, "bl");
        var Constant = new Pin("constantIn", 16, false, "bla");
        InputA.Buffer.Set(8);
        Constant.Buffer.Set(8);
        var intermediate = InputA.Buffer.ToBin()[(InputA.Size / 2)..];
        var res = Value.FromBin(Constant.Buffer.ToBin() + intermediate);
        Assert.Equal("00000000000010000000000000001000", res.ToBin());
    }

    [Fact]
    public void Should_move_constant()
    {
        var cpu = new CPU();
        var program = new List<string> {
            "0".PadLeft(32, '0'),
            new Instruction(OpCode.Move).Destination(1).Constant(new Value(8, 16)).Build(),
            new Instruction(OpCode.MoveUpper).Destination(2).Source(1).Constant(new Value(8, 16)).Build()
        };
        cpu.LoadProgramBinaryStr(program.ToArray());
        cpu.ClockNext();
        Assert.Equal(8, cpu.AluOutProbe.Value.Buffer.Get());
        Assert.Equal(8, cpu.GetRegisterFileContent().ElementAt(1));
        cpu.FullClockCycle();
        //
        cpu.FullClockCycle();
        Assert.Equal("00080008", cpu.AluOutProbe.Value.Buffer.ToHex().ToLower());
    }
}
