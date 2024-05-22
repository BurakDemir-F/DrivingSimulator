using System;
using UnityEngine;

namespace FinishArea
{
    public class VehicleEnterTrigger : MonoBehaviour
    {
        public event Action OnVehicleEntered;
        [SerializeField] private string _vehicleTag = "vehicle";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("vehicle"))
            {
                OnVehicleEntered?.Invoke();
            }
        }
    }
}