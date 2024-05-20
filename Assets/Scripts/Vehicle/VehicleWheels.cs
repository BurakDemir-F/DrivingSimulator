﻿using System;
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

    private Dictionary<WheelType, Wheel> _wheelDict;
    public Dictionary<WheelType, Wheel> WheelDictionary => _wheelDict;
    public List<WheelType> RightWheels { get; private set; }
    public List<WheelType> LeftWheels { get; private set; }
    public void Construct()
    {
        _wheelDict = new Dictionary<WheelType, Wheel>(4);
        _wheelDict.Add(WheelType.Fl,FrontLeft);
        _wheelDict.Add(WheelType.Fr,FrontRight);
        _wheelDict.Add(WheelType.Bl,BackLeft);
        _wheelDict.Add(WheelType.Br,BackRight);

        RightWheels = new List<WheelType>() { WheelType.Br, WheelType.Fr };
        LeftWheels = new List<WheelType>() { WheelType.Bl, WheelType.Fl };
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