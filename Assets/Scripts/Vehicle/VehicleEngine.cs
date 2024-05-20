using System;
using System.Collections.Generic;
using Inputs;
using UnityEngine;
using Vehicle.Data;
using Vehicle.VehicleEffectors;

public class VehicleEngine : MonoBehaviour
{
    [SerializeField] private VehicleSo _vehicleData;
    [SerializeField] private VehicleWheels _wheels;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private InputProvider _inputProvider;
    [SerializeField] private List<VehicleEffector> _effectors;

    public event Action OnCrash;
    
    private void Awake()
    {
        Construct();
        ApplyVehicleData();
    }

    private void OnDestroy()
    {
        Destruct();
    }

    public void ApplyVehicleData()
    {
        _rigidbody.mass = _vehicleData.Mass;
    }

    private void Construct()
    {
        _wheels.Construct();
        foreach (var vehicleEffector in _effectors)
        {
            vehicleEffector.Initialize(_wheels,_rigidbody,_vehicleData);
            vehicleEffector.Activate();
        }
    }

    private void Destruct()
    {
        foreach (var vehicleEffector in _effectors)
        {
            vehicleEffector.DeActivate();
            vehicleEffector.Destruct();
        }
    }

    private void FixedUpdate()
    {
        var horizontalAxis = _inputProvider.HorizontalAxis;
        var verticalAxis = _inputProvider.VerticalAxis;
        var isBrake = _inputProvider.IsBrake;
        var steerAngle = horizontalAxis * _vehicleData.MaxReturnValue;
        var torque = verticalAxis > 0
            ? verticalAxis * _vehicleData.ForwardMotorTorque
            : verticalAxis * _vehicleData.BackwardMotorTorque;
        _wheels.GiveDirection(steerAngle);
        _wheels.ApplyTorque(torque);
        _wheels.ApplyBreak(isBrake ? _vehicleData.BrakeTorque : 0f);
        
        foreach (var vehicleEffector in _effectors)
            vehicleEffector.ApplyEffect();
    }

    public void SetWheels(VehicleWheels wheels) => _wheels = wheels;

    public void SetRigidBody(Rigidbody rgBody) => _rigidbody = rgBody;
    public void SetVehicleData(VehicleSo vehicleData)
    {
        _vehicleData = vehicleData;
        _rigidbody.mass = vehicleData.Mass;
    }
}