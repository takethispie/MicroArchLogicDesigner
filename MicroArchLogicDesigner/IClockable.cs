namespace MicroArchLogicDesigner;

public abstract class Clockable
{
    public void ProcessClockEvent(Value value) {
        switch(value.ToClockEvent())
        {
            case ClockEvent.Low: OnLowClock(); break;
            case ClockEvent.Rising: OnRisingEdgeClock(); break;
            case ClockEvent.High: OnHighClock(); break;
            case ClockEvent.Falling: OnFallingEdgeClock(); break;
        }
    }

    public abstract void OnLowClock();
    public abstract void OnHighClock();
    public abstract void OnRisingEdgeClock();
    public abstract void OnFallingEdgeClock();
}
