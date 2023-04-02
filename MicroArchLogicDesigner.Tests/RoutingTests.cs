using MicroArchLogicDesigner.Routing;

namespace MicroArchLogicDesigner.Tests;

public class RoutingTests
{
    [Fact]
    public void Should_Create_Multiplexer_With_Right_Input_Count()
    {
        var multi = new Multiplexer("multi1", 16, 5);
        Assert.Equal(3, multi.Control.Size);

        multi = new Multiplexer("multi1", 16, 10);
        Assert.Equal(4, multi.Control.Size);
    }

    [Fact]
    public void Should_Route_multiplexer()
    {
        var multi = new Multiplexer("multi1", 16, 8);
        multi.Inputs[0].Receive(new Value(10));
        multi.Inputs[2].Receive(new Value(20));
        multi.Control.Receive(new Value(2));
        Assert.Equal(20, multi.Output.Buffer.Get());

        multi.Control.Receive(new Value(0));
        Assert.Equal(10, multi.Output.Buffer.Get());

    }

    [Fact]
    public void Should_Route_demultiplexer()
    {
        var multi = new Demultiplexer("demulti1", 16, 8);
        multi.Input.Receive(new Value(10));
        multi.Control.Receive(new Value(1));
        Assert.Equal(10, multi.Output[1].Buffer.Get());
        multi.Control.Receive(new Value(2));
        Assert.Equal(0, multi.Output[1].Buffer.Get());
        Assert.Equal(10, multi.Output[2].Buffer.Get());
        multi.Input.Receive(new Value(20));
        Assert.Equal(20, multi.Output[2].Buffer.Get());
    }
}
