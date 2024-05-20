using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Vehicle.VehicleEffectors
{
    public class PreventTurnOver : VehicleEffector
    {
        private readonly List<WheelType> _wheelsInTheAir = new();
        private readonly List<WheelType> _leftAirWheels = new();
        private readonly List<WheelType> _rightAirWheels = new();

        public override void ApplyEffect(ref float horizontalAxis, ref float verticalAxis, ref bool isBrake,
            ref float steerAngle, ref float forwardTorque,ref float backwardTorque)
        {
            if (!_isActivated)
                return;

            TryPrevent();
        }

        private void TryPrevent()
        {
            var turnOverForce = _vehicleData.PreventTurnoverForce;

            foreach (var wheelType in GetAirSideWheels(_vehicleData.AirLimit))
            {
                var wheelCollider = _wheels.GetWheel(wheelType);
                var wheelDownVec = wheelCollider.transform.up * -1f;
                var forcePosition = wheelCollider.transform.position;
                var force = wheelDownVec.normalized * turnOverForce;
                _rb.AddForceAtPosition(force, forcePosition);
                ForceVisualizer.Instance.VisualizeForce(wheelCollider.transform, force, 2f);
            }
        }

        public IEnumerable<WheelType> GetAirSideWheels(float airLimit)
        {
            GetWheelsInTheAir(airLimit);
            if (_wheelsInTheAir.Count == 4)
                return GetHighestTwoWheel(_wheelsInTheAir);

            if (_wheelsInTheAir.Count == 3)
            {
                _leftAirWheels.Clear();
                _rightAirWheels.Clear();
                foreach (var wheelType in _wheelsInTheAir)
                {
                    var wheel = _wheels.GetWheel(wheelType);
                    var list = wheel.Side == WheelSide.Left ? _leftAirWheels : _rightAirWheels;
                    list.Add(wheelType);
                }

                return _leftAirWheels.Count > _rightAirWheels.Count ? _leftAirWheels : _rightAirWheels;
            }

            if (_wheelsInTheAir.Count == 2)
            {
                var firstWheel = _wheelsInTheAir[0];
                var secondWheel = _wheelsInTheAir[1];
                return _wheels.GetWheel(firstWheel).Side == _wheels.GetWheel(secondWheel).Side
                    ? _wheelsInTheAir
                    : new List<WheelType>() { _wheelsInTheAir[Random.Range(0, 2)] };
            }

            return _wheelsInTheAir;
        }

        public IEnumerable<WheelType> GetWheelsInTheAir(float airLimit)
        {
            _wheelsInTheAir.Clear();
            foreach (var (wheelType, wheel) in _wheels.WheelDictionary)
            {
                var wheelCollider = wheel.Collider;
                if (wheelCollider.isGrounded)
                    continue;

                var wheelPos = wheelCollider.transform.position;
                var wheelDown = (wheelCollider.transform.position + Vector3.down);

                if (!Physics.Raycast(wheelPos, wheelDown, out var hitInfo, airLimit))
                {
                    _wheelsInTheAir.Add(wheelType);
                }
            }

            return _wheelsInTheAir;
        }

        private List<WheelType> GetHighestTwoWheel(List<WheelType> airWheels)
        {
            var leftHeight = 0f;
            var rightHeight = 0f;
            
            foreach (var wheelType in airWheels)
            {
                var wheel = _wheels.GetWheel(wheelType);
                if (wheel.IsLeft())
                    leftHeight += wheel.Collider.transform.position.y;
                else if (wheel.IsRight())
                    rightHeight += wheel.Collider.transform.position.y;
            }
            
            leftHeight/=2f;
            rightHeight/=2f;

            return leftHeight > rightHeight ? _wheels.LeftWheels : _wheels.RightWheels;

        }
    }
}