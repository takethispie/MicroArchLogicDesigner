using MicroArchLogicDesigner;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCPU;

public enum OpCode
{
    Move,
    MoveUpper,
    Add, 
    Subtract, 
    Multiply, 
    Divide,
}

public class Instruction
{
    private string instruction;
    private bool[] instructionPatternMask;

    public Instruction(OpCode code) {
        instructionPatternMask = new bool[5] { true, false, false, false, false };
        var opBin = code switch
        {
            OpCode.Add => "00010001",
            OpCode.Subtract => "00010010",
            OpCode.Multiply => "00010011",
            OpCode.Divide => "00010100",
            OpCode.Move => "00010101",
            OpCode.MoveUpper => "00010110",
            _ => "00000000"
        };
        instruction = opBin.ToString();
    }

    public Instruction Destination(int register)
    {
        if (register >= 16) throw new Exception($"incorrect index: {register}");
        if (instructionPatternMask is not [true, false, ..]) throw new Exception("incorrect position");
        instruction += new Value(register, 4).ToBin();
        instructionPatternMask = new[] { true, true, false, false, false };
        return this;
    }

    public Instruction Source(int register)
    {
        if (register >= 16) throw new Exception($"incorrect index: {register}");
        (instruction, instructionPatternMask) = instructionPatternMask switch
        {
            [true, false, ..] => throw new Exception("incorrect position"),
            [true, true, true, true, ..] => throw new Exception("already added all source"),
            [true, true, false, false, _] => (instruction + new Value(register, 4).ToBin(), new[] { true, true, true, false, false }),
            [true, true, true, false, _] => (instruction + new Value(register, 4).ToBin(), new[] { true, true, true, true, false }),
            _ => (instruction, instructionPatternMask)
        };
        return this;
    }

    public Instruction Constant(Value value)
    {
        if(value == null) throw new ArgumentNullException("value");
        if (value.ToBin().Length != 16) throw new ArgumentException($"Incorrect input of length {value.ToBin().Length}");
        if(instructionPatternMask is [true, true, false, ..]) {
            instruction += "0000";
            instructionPatternMask = new[] { true, true, true, false, false };
        }
        instruction += value.ToBin();
        instructionPatternMask = new[] { true, true, true, false, true };
        return this;
    }

    public string Build()
    {
        if (instruction.Length != 32) throw new Exception($"incorrect length: {instruction.Length}");
        if (instructionPatternMask is not [true, true, true, true, false] and not [ true, true, true, false, true]) 
            throw new Exception("incomplete instruction");
        return instruction;
    }
}
