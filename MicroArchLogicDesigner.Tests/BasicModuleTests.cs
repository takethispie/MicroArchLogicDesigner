using MicroArchLogicDesigner.BaseModules;
using Xunit;

namespace MicroArchLogicDesigner.Tests
{
    public class BasicModuleTests
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
            Assert.Throws<Exception>(() =>
            {
                var clk = new ClockGenerator();
                var dff = new DFlipFlop("dflipflop1", 1);
                var probe = new Probe("probe", 2);
                clk.Output.ConnectTo(dff.Clock);
                dff.Output.ConnectTo(probe.Value);
            });
        }

        [Fact]
        public void Should_correctly_Add_Two_Constants()
        {
            var adder = new Adder("adder1", 16);
            //use receive for any asynchronous module
            adder.InputA.Receive(new Value(5));
            adder.InputB.Receive(new Value(5));
            Assert.Equal(10, adder.Result.Buffer.Get());
        }
    }
}