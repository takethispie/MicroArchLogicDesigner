namespace MicroArchLogicDesigner.BaseModules;
public class Probe : IModule
{
    public Pin Value { get; set; }

    public string Name { get; init; }

    public Probe(string name, int size) {
        Name = name;
        Value = new Pin("value", size, false, Name);
    }
}
