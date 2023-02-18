namespace MicroArchLogicDesigner;
public interface IBusData
{
    public int Size { get; init; }
    public Value Message { get; init; }
    public BusDataTypes Type { get; init; }
}
