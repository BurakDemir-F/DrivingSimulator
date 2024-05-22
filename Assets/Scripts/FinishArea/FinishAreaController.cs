using System;
using UnityEngine;

namespace FinishArea
{
    public class FinishAreaController : MonoBehaviour
    {
        [SerializeField] private VehicleEnterTrigger _cameraTrigger;
        [SerializeField] private VehicleEnterTrigger _winTrigger;

        public event Action OnEndCameraAreaEntered;
        public event Action OnGameWin;

        private void Awake()
        {
            _cameraTrigger.OnVehicleEntered += HandleCameraAreaEntered;
            _winTrigger.OnVehicleEntered += HandleGameWin;
        }

        private void HandleCameraAreaEntered()
        {
            OnEndCameraAreaEntered?.Invoke();
        }

        private void HandleGameWin()
        {
            OnGameWin?.Invoke();
        }
    }
}