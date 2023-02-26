using ExampleCPU;

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
    public void Should_count()
    {
        var counter = new Counter("counter", 32);
        Assert.Equal(0, counter.Output.Buffer.Get());
        counter.Clock.Receive(Value.One());
        Assert.Equal(1, counter.Output.Buffer.Get());
    }

    [Fact]
    public void Should_load()
    {
        var counter = new Counter("counter", 32);
        Assert.Equal(0, counter.Output.Buffer.Get());
        counter.Data.Set(new Value(10));
        counter.Load.Set(Value.One());
        counter.Clock.Receive(Value.One());
        Assert.Equal(10, counter.Output.Buffer.Get());
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
    public void Should_move_constant()
    {
        var cpu = new CPU();
        var program = new List<string> { 
            "00010101000100000000000000001000",
            "00010101000100000000000000001000"
        };
        cpu.LoadProgramBinaryStr(program.ToArray());
        cpu.ClockNext();
        Assert.Equal(8, cpu.AluOutProbe.Value.Buffer.Get());
    }
}
