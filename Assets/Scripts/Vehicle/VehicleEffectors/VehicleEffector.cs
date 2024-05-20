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

        public virtual void Initialize(VehicleWheels wheels, Rigidbody rb, VehicleSo vehicleData)
        {
            _wheels = wheels;
            _rb = rb;
            _vehicleData = vehicleData;
        }

        public virtual void Destruct()
        {
            
        }

        public virtual void Activate() => _isActivated = true;
        public virtual void DeActivate() => _isActivated = false;

        public abstract void ApplyEffect();
    }
}