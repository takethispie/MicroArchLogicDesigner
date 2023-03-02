using MicroArchLogicDesigner;
using MicroArchLogicDesigner.BaseModules;
using System.Collections.ObjectModel;

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

    public Probe AluOutProbe { get; private set; }
    public bool EndOfProgram { get; private set; }
    public bool Running { get; private set; }

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
        AluOutProbe = new Probe("alu_out_probe", 32);

        clockGenerator.Output.ConnectTo(counter.Clock);
        clockGenerator.Output.ConnectTo(programMemory.ClockIn);
        clockGenerator.Output.ConnectTo(registerFile.Clock);
        clockGenerator.Output.ConnectTo(workingMemory.ClockIn);

        counter.Output.ConnectTo(programMemory.ReadAddress);
        programMemory.DataOut.ConnectTo(decoder.InstructionIn);

        decoder.First.ConnectTo(registerFile.ReadA);
        decoder.Second.ConnectTo(registerFile.ReadB);
        decoder.OpOut.ConnectTo(opDecoder.Input);
        decoder.Constant.ConnectTo(alu.Constant);
        decoder.DestOut.ConnectTo(registerFile.Dest);
        opDecoder.AluOut.ConnectTo(alu.Control);
        opDecoder.IOOut.ConnectTo(IOSwitch.Control);

        registerFile.OutputA.ConnectTo(alu.InputA);
        registerFile.OutputB.ConnectTo(alu.InputB);
        registerFile.OutputA.ConnectTo(jumpComparator.InputA);
        registerFile.OutputB.ConnectTo(jumpComparator.InputB);
        registerFile.OutputA.ConnectTo(workingMemory.ReadAddress);
        registerFile.OutputB.ConnectTo(workingMemory.DataIn);

        alu.Result.ConnectTo(IOSwitch.Inputs[0]);
        alu.Result.ConnectTo(AluOutProbe.Value);
        workingMemory.DataOut.ConnectTo(IOSwitch.Inputs[1]);
        IOSwitch.Output.ConnectTo(registerFile.DataIn);
    }

    public void LoadHexProgram(string[] program) => programMemory.LoadData(program.Select(x => Value.FromHex(x).Get()).ToArray());

    public void LoadProgram(int[] program) => programMemory.LoadData(program);

    public void LoadProgramBinaryStr(string[] program) => programMemory.LoadData(program.Select(x => Value.FromBin(x).Get()).ToArray());

    public void ClockNext() => clockGenerator.DoQuarterTick();

    public ReadOnlyCollection<int> GetRegisterFileContent() => registerFile.GetContent();
}
