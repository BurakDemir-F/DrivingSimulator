using System;
using UnityEngine;

namespace TargetPointer
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField] private Transform _targetArea;
        [SerializeField] private Transform _vehiclePos;

        private Vector3 _followOffset;
        private void Start()
        {
            _followOffset = transform.position - _vehiclePos.position;
        }

        private void Update()
        {
            transform.position = new Vector3(_vehiclePos.position.x,0f,_vehiclePos.position.z) + _followOffset;
            var distanceVec = _targetArea.position - transform.position;
            var rotation = Quaternion.LookRotation(distanceVec, transform.up);
            transform.rotation = rotation;
        }
    }
}