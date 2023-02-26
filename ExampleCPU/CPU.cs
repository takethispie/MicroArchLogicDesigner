using MicroArchLogicDesigner.BaseModules;

namespace ExampleCPU;
public class CPU
{
    private ClockGenerator clockGenerator;
    private Counter counter;
    private RandomAccessMemory programMemory;
    private RandomAccessMemory workingMemory;
    private ALU alu;
    private Comparator jumpComparator;
    private RegisterFile registerFile;
    private InstructionDecoder decoder;
    private OpDecoder opDecoder;
    private Multiplexer IOSwitch;
    private IOModule ioModule;

    public CPU() {
        counter = new Counter("program_counter", 32);
        programMemory = new RandomAccessMemory("program_memory", 32, 32);
        workingMemory = new RandomAccessMemory("working_memory", 32, 32);
        alu = new ALU("alu", 32);
        jumpComparator = new Comparator("jumpComparator", 32);
        registerFile = new RegisterFile("registerFile", 32, 16);
        decoder = new InstructionDecoder("decoder");
        clockGenerator= new ClockGenerator();
        opDecoder = new OpDecoder("opDecoder");
        IOSwitch = new Multiplexer("IOSwitch", 32, 4);
        ioModule = new IOModule("ioModule");

        clockGenerator.Output.ConnectTo(counter.Clock);
        clockGenerator.Output.ConnectTo(programMemory.ClockIn);
        clockGenerator.Output.ConnectTo(registerFile.Clock);
        clockGenerator.Output.ConnectTo(workingMemory.ClockIn);

        counter.Output.ConnectTo(programMemory.ReadAddress);
        programMemory.DataOut.ConnectTo(decoder.InstructionIn);

        decoder.First.ConnectTo(registerFile.ReadA);
        decoder.Second.ConnectTo(registerFile.ReadB);
        decoder.OpOut.ConnectTo(opDecoder.Input);
        opDecoder.AluOut.ConnectTo(alu.Control);

        registerFile.OutputA.ConnectTo(alu.InputA);
        registerFile.OutputB.ConnectTo(alu.InputB);
        registerFile.OutputA.ConnectTo(workingMemory.ReadAddress);
        registerFile.OutputB.ConnectTo(workingMemory.DataIn);

        alu.Result.ConnectTo(IOSwitch.Inputs[0]);
        workingMemory.DataOut.ConnectTo(IOSwitch.Inputs[1]);
        IOSwitch.Output.ConnectTo(registerFile.DataIn);


    }
}
