using UnityEngine;
using Vehicle.Data;

namespace Vehicle.VehicleEffectors
{
    public abstract class VehicleEffector : MonoBehaviour
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

        public virtual void Activate() => _isActivated = true;
        public virtual void DeActivate() => _isActivated = false;

        public abstract void ApplyEffect(ref float horizontalAxis, ref float verticalAxis, ref bool isBrake,
            ref float steerAngle, ref float forwardTorque,ref float backwardTorque);
    }
}