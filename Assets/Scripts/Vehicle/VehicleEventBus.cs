using General;

namespace Vehicle
{
    public class VehicleEventBus : EventBus<VehicleInfoType>
    {
        
    }

    public enum VehicleInfoType
    {
        None,
        MovementStarted,
        Stopped,
        Crashed,
        WheelOnAir
    }
}