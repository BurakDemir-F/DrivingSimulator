using System;
using UnityEngine;

namespace Vehicle
{
    public class VehicleTire : MonoBehaviour
    {
        [SerializeField] private WheelCollider _collider;
        
        private void Update()
        {
            _collider.GetWorldPose(out var position,out var rotation);
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}