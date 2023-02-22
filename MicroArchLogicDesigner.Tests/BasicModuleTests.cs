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

        [Fact]
        public void Should_Do_Logic_Correctly()
        {
            var gate = new LogicGate("lg1", LogicType.And);
            gate.InputA.Receive(new Value(1));
            gate.InputB.Receive(new Value(1));
            Assert.Equal(1, gate.Result.Buffer.Get());

            gate = new LogicGate("lg1", LogicType.And);
            gate.InputA.Receive(new Value(0));
            gate.InputB.Receive(new Value(1));
            Assert.Equal(0, gate.Result.Buffer.Get());

            gate = new LogicGate("lg1", LogicType.Or);
            gate.InputA.Receive(new Value(1));
            gate.InputB.Receive(new Value(0));
            Assert.Equal(1, gate.Result.Buffer.Get());

            gate = new LogicGate("lg1", LogicType.Or);
            gate.InputA.Receive(new Value(0));
            gate.InputB.Receive(new Value(0));
            Assert.Equal(0, gate.Result.Buffer.Get());
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
            Assert.Equal(10,registerFile.OutputB.Buffer.Get()); 
        }
    }
}