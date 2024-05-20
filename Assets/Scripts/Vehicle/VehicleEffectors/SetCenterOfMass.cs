using UnityEngine;

namespace Vehicle.VehicleEffectors
{
    public class SetCenterOfMass : VehicleEffector
    {
        [SerializeField] private Vector3 _centerOfMass = Vector3.up * -1f;
        public override void Activate()
        {
            base.Activate();
            _rb.centerOfMass = _centerOfMass;
        }

        public override void ApplyEffect(ref float horizontalAxis, ref float verticalAxis, ref bool isBrake,
            ref float steerAngle, ref float forwardTorque,ref float backwardTorque)
        {
            
        }
    }
}