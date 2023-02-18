namespace MicroArchLogicDesigner;

public interface IClockable
{
    string Name { get; init; }
    void ProcessClockEvent(ClockEvent clkEvent) {
        switch(clkEvent)
        {
            case ClockEvent.Low: OnLowClock(); break;
            case ClockEvent.Rising: OnRisingEdgeClock(); break;
            case ClockEvent.High: OnHighClock(); break;
            case ClockEvent.Falling: OnFallingEdgeClock(); break;
        }
    }

    void OnLowClock();
    void OnHighClock();
    void OnRisingEdgeClock();
    void OnFallingEdgeClock();
}
