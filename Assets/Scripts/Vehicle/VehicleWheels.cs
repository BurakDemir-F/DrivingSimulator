using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class VehicleWheels : MonoBehaviour
{ 
    [field:SerializeField] public Wheel FrontLeft { get; set; }
    [field:SerializeField] public Wheel FrontRight{ get; set; }
    [field: SerializeField] public Wheel BackLeft{ get; set; }
    [field: SerializeField] public Wheel BackRight{ get; set; }

    private List<WheelType> _wheelsInTheAir = new();
    private Dictionary<WheelType, Wheel> _wheelDict;
    private List<WheelType> _leftAirWheels = new();
    private List<WheelType> _rightAirWheels = new();
    private List<WheelType> _randomSideWheels = new();
    public void Construct()
    {
        _wheelDict = new Dictionary<WheelType, Wheel>(4);
        _wheelDict.Add(WheelType.Fl,FrontLeft);
        _wheelDict.Add(WheelType.Fr,FrontRight);
        _wheelDict.Add(WheelType.Bl,BackLeft);
        _wheelDict.Add(WheelType.Br,BackRight);
    } 
    
    public void GiveDirection(float direction)
    {
        FrontLeft.Collider.steerAngle = direction;
        FrontRight.Collider.steerAngle = direction;
    }
    public void ApplyTorque(float torque)
    {
        BackLeft.Collider.motorTorque = torque;
        BackRight.Collider.motorTorque = torque;
        // FrontLeft.Collider.motorTorque = torque;
        // FrontRight.Collider.motorTorque = torque;
    }

    public void ApplyBreak(float brakeTorque)
    {
        BackLeft.Collider.brakeTorque = brakeTorque;
        BackRight.Collider.brakeTorque = brakeTorque;
        FrontLeft.Collider.brakeTorque = brakeTorque;
        FrontRight.Collider.brakeTorque = brakeTorque;
    }

    public Wheel GetWheel(WheelType type) => _wheelDict[type];

    public IEnumerable<WheelType> GetWheelsInTheAir(float airLimit)
    {
        _wheelsInTheAir.Clear();
        foreach (var (wheelType, wheel) in _wheelDict)
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

    public IEnumerable<WheelType> GetAirSideWheels(float airLimit)
    {
        GetWheelsInTheAir(airLimit);
        if (_wheelsInTheAir.Count == 4)
        {
            _randomSideWheels.Clear();
            var randomSide = Random.Range(0, 100) % 2 == 0 ? WheelSide.Left : WheelSide.Right;
            foreach (var wheelType in _wheelsInTheAir)
            {
                var wheel = GetWheel(wheelType);
                if (wheel.Side == randomSide)
                {
                    _randomSideWheels.Add(wheelType);
                }
            }

            return _randomSideWheels;
        }

        if (_wheelsInTheAir.Count == 3)
        {
             _leftAirWheels.Clear();
             _rightAirWheels.Clear();
            foreach (var wheelType in _wheelsInTheAir)
            {
                var wheel = GetWheel(wheelType);
                var list = wheel.Side == WheelSide.Left ? _leftAirWheels : _rightAirWheels;
                list.Add(wheelType);
            }

            return _leftAirWheels.Count > _rightAirWheels.Count ? _leftAirWheels : _rightAirWheels;
        }

        if (_wheelsInTheAir.Count == 2)
        {
            var firstWheel = _wheelsInTheAir[0];
            var secondWheel = _wheelsInTheAir[1];
            return GetWheel(firstWheel).Side == GetWheel(secondWheel).Side
                ? _wheelsInTheAir
                : new List<WheelType>() { _wheelsInTheAir[Random.Range(0, 2)] };
        }

        return _wheelsInTheAir;
    }

    private void OnDrawGizmosSelected()
    {
        if(_wheelDict == null)
            return;
        var airSideWheels = GetAirSideWheels(0.5f);
        Gizmos.color = Color.yellow;
        foreach (var airSideWheel in airSideWheels)
        {
            var wheel = GetWheel(airSideWheel);
            Gizmos.DrawSphere(wheel.Collider.transform.position,1f);
        }
    }
}

public enum WheelSide
{
    None,Left,Right
}

public enum WheelType
{
    None,
    Fl,
    Fr,
    Bl,
    Br
}