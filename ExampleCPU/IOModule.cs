using MicroArchLogicDesigner;

namespace ExampleCPU;
public class IOModule : IModule
{
    public string Name { get; init; }

    public IOModule(string name) {
        Name = name;
    }
}
