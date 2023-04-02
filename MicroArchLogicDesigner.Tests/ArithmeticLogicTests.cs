using MicroArchLogicDesigner.ArithmeticLogic;

namespace MicroArchLogicDesigner.Tests;

public class ArithmeticLogicTests
{
    [Fact]
    public void Should_correctly_Add_Two_Constants()
    {
        var adder = new Adder("adder1", 16);
        //use receive for any asynchronous input
        adder.InputA.Receive(new Value(5));
        adder.InputB.Receive(new Value(5));
        Assert.Equal(10, adder.Result.Buffer.Get());
    }

    [Fact]
    public void ALU_Should_Work()
    {
        var alu = new ALU("alu1", 16);
        alu.InputA.Receive(new Value(10));
        alu.InputB.Receive(new Value(5));
        alu.Control.Receive(new Value(2));
        Assert.Equal(5, alu.Result.Buffer.Get());

        alu.InputA.Receive(new Value(3));
        alu.InputB.Receive(new Value(2));
        alu.Control.Receive(new Value(3));
        Assert.Equal(6, alu.Result.Buffer.Get());

        alu.InputA.Receive(new Value(12));
        alu.InputB.Receive(new Value(2));
        alu.Control.Receive(new Value(4));
        Assert.Equal(6, alu.Result.Buffer.Get());
    }

    [Fact]
    public void Should_Compare_Correctly()
    {
        var comp = new Comparator("comparator1", 16);

        comp.InputA.Receive(new Value(2));
        comp.InputB.Receive(new Value(1));
        Assert.Equal(1, comp.GreaterThan.Buffer.Get());
        Assert.Equal(0, comp.Equal.Buffer.Get());
        Assert.Equal(0, comp.LowerThan.Buffer.Get());

        comp.InputA.Receive(new Value(2));
        comp.InputB.Receive(new Value(2));
        Assert.Equal(0, comp.GreaterThan.Buffer.Get());
        Assert.Equal(1, comp.Equal.Buffer.Get());
        Assert.Equal(0, comp.LowerThan.Buffer.Get());

        comp.InputA.Receive(new Value(1));
        comp.InputB.Receive(new Value(2));
        Assert.Equal(0, comp.GreaterThan.Buffer.Get());
        Assert.Equal(0, comp.Equal.Buffer.Get());
        Assert.Equal(1, comp.LowerThan.Buffer.Get());
    }
}
