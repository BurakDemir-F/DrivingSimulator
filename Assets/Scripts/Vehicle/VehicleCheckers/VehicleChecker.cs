using UnityEngine;
using Vehicle.Data;

namespace Vehicle.VehicleCheckers
{
    public abstract class VehicleChecker : MonoBehaviour
    {
        protected VehicleWheels _wheels;
        protected Rigidbody _rb;
        protected VehicleSo _vehicleData;
        protected bool _isActivated;
        protected VehicleEventBus _eventBus;

        public virtual void Initialize(VehicleWheels wheels, Rigidbody rb, VehicleSo vehicleData,VehicleEventBus eventBus)
        {
            _wheels = wheels;
            _rb = rb;
            _vehicleData = vehicleData;
            _eventBus = eventBus;
        }

        public virtual void Destruct()
        {
            
        }

        public virtual void Activate()
        {
            _isActivated = true;
        }

        public virtual void Deactivate()
        {
            _isActivated = false;
        }

        public abstract void Check();
    }
}