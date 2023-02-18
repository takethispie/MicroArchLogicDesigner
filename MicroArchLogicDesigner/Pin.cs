namespace MicroArchLogicDesigner;

public class Pin
{
    public bool IsOutput { get; init; }
    public string Name { get; init; }
    public int Size { get; init; }
    public Value Buffer { get; private set; }
    public IEnumerable<Pin> Targets { get; private set; }
    public Action<Value>? OnValue { get; init; }
    private bool free;

    public Pin(string name, int size, bool isoutput, string parentName)
    {
        Name = parentName + "-" + name;
        Size = size;
        IsOutput = isoutput;
        Buffer = new Value(0);
        Targets = new List<Pin>();
        free = true;
    }

    public void ConnectTo(Pin target)
    {
        if (!target.free) throw new Exception("target pin: " + target.Name + " already has a connection");
        if (target.IsOutput) throw new Exception("cant connect to an output pin, from " + Name + " to " + target.Name);
        if (Targets.Any(x => x.Name == target.Name)) throw new Exception(target.Name + " Already connected !");
        if(Size != target.Size) throw new Exception("output: " + Name + " doesnt match " + target.Name + " bit width");
        Targets = Targets.Append(target);
        target.free = false;
    }

    public void DisconnectFrom(Pin target)
    {
        if(!Targets.Any(x => x.Name == target.Name)) throw new Exception("this pin is not connected to targeted pin");
        target.free = true;
        Targets = Targets.Where(x => x.Name != target.Name).ToList();
    }

    public void Set(Value value)
    {
        Buffer = value;
        if(IsOutput && Targets != null) Targets.ToList().ForEach(target => target.Receive(value));
    }

    public void Receive(Value value)
    {
        if (OnValue != null) OnValue(value); 
        Buffer = value;
    }
}

