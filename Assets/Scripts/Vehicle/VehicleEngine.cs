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
    [field:SerializeField] public Transform ReAnimateTransform { get; private set; }
    [SerializeField] private List<Collider> _collliders;

    public event Action OnCrash;
    
    private void Awake()
    {
        Construct();
        ApplyVehicleData();
        Activate();
    }

    private void OnDestroy()
    {
        DeActivate();
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
            vehicleEffector.Initialize(_wheels,_rigidbody,_vehicleData,_eventBus);
        }
        
        foreach (var vehicleChecker in _checkers)
        {
            vehicleChecker.Initialize(_wheels,_rigidbody,_vehicleData,_eventBus);
        }
        
        _eventBus.Subscribe(VehicleInfoType.Crashed,OnCrashed);
    }

    private void Destruct()
    {
        _eventBus.UnSubscribe(VehicleInfoType.Crashed,OnCrashed);
        foreach (var vehicleEffector in _effectors)
        {
            vehicleEffector.DeActivate();
            vehicleEffector.Destruct();
        }
    }

    private void Activate()
    {
        foreach (var vehicleEffector in _effectors)
            vehicleEffector.Activate();
        
        foreach (var vehicleChecker in _checkers)
            vehicleChecker.Activate();
    }
    
    private void DeActivate()
    {
        foreach (var vehicleEffector in _effectors)
            vehicleEffector.DeActivate();
        
        foreach (var vehicleChecker in _checkers)
            vehicleChecker.DeActivate();
    }

    private void Update()
    {
        foreach (var vehicleChecker in _checkers)
        {
            vehicleChecker.Check();
        }
    }

    private void FixedUpdate()
    {
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
        foreach (var col in _collliders)
        {
            col.enabled = false;
        }
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        _wheels.Deactivate();
    }

    public void HandleReAnimationEnd()
    {
        foreach (var col in _collliders)
        {
            col.enabled = true;
        }
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.gameObject.SetActive(true);
        _wheels.Activate();
    }
}