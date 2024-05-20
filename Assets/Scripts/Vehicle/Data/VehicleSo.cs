﻿using UnityEngine;
using UnityEngine.Serialization;

namespace Vehicle.Data
{
    [CreateAssetMenu(menuName = "ScriptableData/Vehicle", fileName = "Vehicle", order = 0)]
    public class VehicleSo : ScriptableObject
    {
        [SerializeField] private float _mass;
        [SerializeField] private float _forwardMotorTorque;
        [SerializeField] private float _backwardMotorTorque;
        [SerializeField] private float brakeTorque;
        [SerializeField] private float _maxReturnValue;
        [SerializeField] private float _preventTurnoverForce;
        [SerializeField] private float _airLimit;
        [SerializeField] private WheelFrictionValues _forwardFriction;
        [SerializeField] private WheelFrictionValues _sideWayFriction;
        
        public float Mass => _mass;
        public float ForwardMotorTorque => _forwardMotorTorque;
        public float BackwardMotorTorque => _backwardMotorTorque;
        public float MaxReturnValue => _maxReturnValue;
        public float PreventTurnoverForce => _preventTurnoverForce;
        public float AirLimit => _airLimit;
        public float BrakeTorque => brakeTorque;
        public WheelFrictionValues ForwardFriction => _forwardFriction;
        public WheelFrictionValues SideWayFriction => _sideWayFriction;
    }

    [System.Serializable]
    public class WheelFrictionValues
    {
        [Range(0f,20000f)][SerializeField] public float asymptoteValue;
        [Range(10000f,40000f)][SerializeField] public float extremumValue;
    }
}