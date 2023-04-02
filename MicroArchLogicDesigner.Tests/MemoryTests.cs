using MicroArchLogicDesigner.Base;
using MicroArchLogicDesigner.Exceptions;
using MicroArchLogicDesigner.InputOutput;
using MicroArchLogicDesigner.Memory;

namespace MicroArchLogicDesigner.Tests;
public class MemoryTests
{
    [Fact]
    public void Should_correctly_route_outputs_and_inputs()
    {
        var clk = new ClockGenerator();
        var dff = new DFlipFlop("dflipflop1", 2);
        var probe = new Probe("probe", 2);
        clk.Output.ConnectTo(dff.Clock);
        dff.Output.ConnectTo(probe.Value);
        dff.Input.Set(new Value(10));
        clk.DoQuarterTick();
        Assert.Equal(10, probe.Value.Buffer.Get());
    }

    [Fact]
    public void Should_Fail_Connecting_Dff_To_Probe()
    {
        Assert.Throws<BitWidthMismatchException>(() =>
        {
            var clk = new ClockGenerator();
            var dff = new DFlipFlop("dflipflop1", 1);
            var probe = new Probe("probe", 2);
            clk.Output.ConnectTo(dff.Clock);
            dff.Output.ConnectTo(probe.Value);
        });
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
    public void Should_Write_And_Read_Same_Address()
    {
        var ram = new RandomAccessMemory("ram1", 16, 16);
        ram.WriteAddress.Receive(new Value(1));
        ram.DataIn.Receive(new Value(16));
        ram.WriteEnable.Receive(new Value(1));
        ram.ReadAddress.Receive(new Value(1));
        ram.ClockIn.Receive(new Value(1));
        Assert.Equal(16, ram.DataOut.Buffer.Get());
    }

    [Fact]
    public void Should_Write_and_read_register()
    {
        var registerFile = new RegisterFile("regFile1", 16, 16);
        registerFile.DataIn.Set(new Value(10));
        registerFile.Dest.Set(new Value(1));
        registerFile.Clock.Receive(new Value(1));
        registerFile.Clock.Receive(new Value(0));
        registerFile.ReadA.Receive(new Value(1));
        Assert.Equal(10, registerFile.OutputA.Buffer.Get());
        registerFile.ReadB.Receive(new Value(1));
        Assert.Equal(10, registerFile.OutputA.Buffer.Get());
        Assert.Equal(10, registerFile.OutputB.Buffer.Get());
    }
}
