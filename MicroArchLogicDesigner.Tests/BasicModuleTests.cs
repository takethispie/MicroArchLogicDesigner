using MicroArchLogicDesigner.Base;
using MicroArchLogicDesigner.InputOutput;
using Xunit;

namespace MicroArchLogicDesigner.Tests
{
    public class BasicModuleTests
    {
        

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
    }
}