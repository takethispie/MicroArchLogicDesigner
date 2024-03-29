﻿using MicroArchLogicDesigner.Exceptions;

namespace MicroArchLogicDesigner;

public class Pin
{
    public bool IsOutput { get; init; }
    public string Name { get; init; }
    public int Size { get; init; }
    public Value Buffer { get; private set; }
    public IEnumerable<Pin> Targets { get; private set; }
    public Action<Value>? OnValue { get; init; }
    private bool free, propagateSameValue;

    public Pin(string name, int size, bool isoutput, string parentName, bool propagate = false)
    {
        Name = parentName + "-" + name;
        Size = size;
        IsOutput = isoutput;
        Buffer = new Value(0, size);
        Targets = new List<Pin>();
        free = true;
        propagateSameValue = propagate;
    }

    /// <summary>
    /// connect pin to input pin of another module
    /// </summary>
    /// <param name="target">target input pin of another module</param>
    /// <exception cref="DuplicatedInputsOnPinException"></exception>
    /// <exception cref="ConnectingInputToOutputPinException"></exception>
    /// <exception cref="Exception"></exception>
    /// <exception cref="BitWidthMismatchException"></exception>
    public void ConnectTo(Pin target)
    {
        if (!target.free) throw new DuplicatedInputsOnPinException("target pin: " + target.Name + " already has a connection");
        if (target.IsOutput) throw new ConnectingInputToOutputPinException("cant connect to an output pin, from " + Name + " to " + target.Name);
        if (Targets.Any(x => x.Name == target.Name)) throw new Exception(target.Name + " Already connected !");
        if(Size != target.Size) throw new BitWidthMismatchException("output: " + Name + " doesnt match " + target.Name + " bit width");
        Targets = Targets.Append(target);
        target.free = false;
    }

    /// <summary>
    /// disconnect pin from target input pin of another module
    /// </summary>
    /// <param name="target">target pin to disconnect from</param>
    /// <exception cref="NoPinConnectionException"></exception>
    public void DisconnectFrom(Pin target)
    {
        if(!Targets.Any(x => x.Name == target.Name)) throw new NoPinConnectionException("this pin is not connected to targeted pin");
        target.free = true;
        Targets = Targets.Where(x => x.Name != target.Name).ToList();
    }

    /// <summary>
    /// change value, used for sync input or sync/async outputs 
    /// </summary>
    /// <param name="value">the new value of the pin</param>
    /// <param name="propagateSameValue">if true while propagate value even if it has not changed</param>
    public void Set(Value value)
    {
        if (!propagateSameValue && Buffer.Get() == value.Get()) return;
        Buffer = value;
        if(IsOutput && Targets != null) Targets.ToList().ForEach(target => target.Receive(value));
    }

    /// <summary>
    /// change value, used for async inputs 
    /// </summary>
    /// <param name="value">the new value of the pin</param>
    public void Receive(Value value)
    {
        if(!propagateSameValue && Buffer.Get() == value.Get()) return; 
        Buffer = value;
        if (OnValue != null) OnValue(value); 
    }
}

