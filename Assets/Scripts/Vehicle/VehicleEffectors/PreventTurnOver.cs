using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Vehicle.VehicleEffectors
{
    public class PreventTurnOver : VehicleEffector
    {
        private List<WheelCollider> _wheelsInTheAir = new();
        public override void ApplyEffect()
        {
            if(!_isActivated)
                return;
            
            TryPrevent();
        }
        
        private void TryPrevent()
        {
            var turnOverForce = _vehicleData.PreventTurnoverForce;
            
            foreach (var wheelCollider in GetWheelsOnTheAir())
            {
                var wheelDownVec = wheelCollider.transform.up * -1f;
                var forcePosition = wheelCollider.transform.position;
                var force = wheelDownVec.normalized * turnOverForce;
                _rb.AddForceAtPosition(force,forcePosition);
                ForceVisualizer.Instance.VisualizeForce(wheelCollider.transform,force,2f);
                Debug.Log("applying force!!!");
            }
        }

        private List<WheelCollider> GetWheelsOnTheAir()
        {
            _wheelsInTheAir.Clear();
            var wheelTypes = _wheels.GetAirSideWheels(_vehicleData.AirLimit);
            foreach (var wheelType in wheelTypes)
            {
                _wheelsInTheAir.Add(_wheels.GetWheel(wheelType).Collider);
            }

            return _wheelsInTheAir;
        }
    }
}