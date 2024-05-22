using System;
using System.Collections.Generic;
using General;
using Inputs;
using ReAnimatable;
using UnityEngine;
using Vehicle;
using Vehicle.Data;
using Vehicle.VehicleCheckers;
using Vehicle.VehicleEffectors;

public class VehicleEngine : MonoBehaviour, IReAnimatable
{
    [SerializeField] private VehicleSo _vehicleData;
    [SerializeField] private VehicleWheels _wheels;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private InputProvider _inputProvider;
    [SerializeField] private VehicleEventBus _eventBus;
    [SerializeField] private List<VehicleEffector> _effectors;
    [SerializeField] private List<VehicleChecker> _checkers;

    private bool _isActivated;
    
    public event Action OnCrash;

    public void Construct()
    {
        _wheels.Construct();
        foreach (var vehicleEffector in _effectors)
        {
            vehicleEffector.Initialize(_wheels,_rigidbody,_vehicleData,_eventBus);
        }
        
        foreach (var vehicleChecker in _checkers)
        {
            vehicleChecker.Initialize(_wheels,_rigidbody,_vehicleData,_eventBus);
        }
        
        _eventBus.Subscribe(VehicleInfoType.Crashed,OnCrashed);
        ApplyVehicleData();
    }

    public void Destruct()
    {
        DeActivate();
        _eventBus.UnSubscribe(VehicleInfoType.Crashed,OnCrashed);
    }

    public void ApplyVehicleData()
    {
        _rigidbody.mass = _vehicleData.Mass;
    }

    public void Activate()
    {
        foreach (var vehicleEffector in _effectors)
            vehicleEffector.Activate();
        
        foreach (var vehicleChecker in _checkers)
            vehicleChecker.Activate();

        _isActivated = true;
    }
    
    public void DeActivate()
    {
        foreach (var vehicleEffector in _effectors)
            vehicleEffector.DeActivate();
        
        foreach (var vehicleChecker in _checkers)
            vehicleChecker.DeActivate();

        _isActivated = false;
    }

    private void Update()
    {
        if(!_isActivated)
            return;
        
        foreach (var vehicleChecker in _checkers)
        {
            vehicleChecker.Check();
        }
    }

    private void FixedUpdate()
    {
        if(!_isActivated)
            return;
        
        var horizontalAxis = _inputProvider.HorizontalAxis;
        var verticalAxis = _inputProvider.VerticalAxis;
        var isBrake = _inputProvider.IsBrake;
        var steerAngle = _vehicleData.MaxReturnValue;
        var forwardTorque = _vehicleData.ForwardMotorTorque;
        var backwardTorque = _vehicleData.BackwardMotorTorque;
        
        foreach (var vehicleEffector in _effectors)
            vehicleEffector.ApplyEffect(ref horizontalAxis, ref verticalAxis, ref isBrake, ref steerAngle,
                ref forwardTorque, ref backwardTorque);
        
        steerAngle = horizontalAxis * _vehicleData.MaxReturnValue;
        var torque = verticalAxis > 0
            ? verticalAxis * forwardTorque
            : verticalAxis * backwardTorque;
        
        _wheels.GiveDirection(steerAngle);
        _wheels.ApplyTorque(torque);
        _wheels.ApplyBreak(isBrake ? _vehicleData.BrakeTorque : 0f);
    }

    private void OnCrashed(EventBusData data)
    {
        DeActivate();
        OnCrash?.Invoke();
        Debug.Log("crashed");
    }
    
    public void SetWheels(VehicleWheels wheels) => _wheels = wheels;

    public void SetRigidBody(Rigidbody rgBody) => _rigidbody = rgBody;
    public void SetVehicleData(VehicleSo vehicleData)
    {
        _vehicleData = vehicleData;
        _rigidbody.mass = vehicleData.Mass;
    }

    public void PrepareReanimation()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        _wheels.Deactivate();
    }

    public void HandleReAnimationEnd()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.gameObject.SetActive(true);
        _wheels.Activate();
    }

    public Vector3 GetPosition()
    {
        return _rigidbody.position;
    }

    public Quaternion GetRotation()
    {
        return _rigidbody.rotation;
    }

    public void SetPosition(Vector3 position)
    {
        _rigidbody.position = position;
    }

    public void SetRotation(Quaternion rotation)
    {
        _rigidbody.rotation = rotation;
    }
}