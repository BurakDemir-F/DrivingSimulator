using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Managers
{
    public class GameCameras : MonoBehaviour
    {
        [SerializeField] private int _lowPriority;
        [SerializeField] private int _highPriority;

        [SerializeField] private CinemachineVirtualCamera _startCam;
        [SerializeField] private CinemachineVirtualCamera _GameEndCame;
        [SerializeField] private CinemachineVirtualCamera _vehicleCam;

        private List<CinemachineVirtualCamera> _virtualCameras;
        
        public void Construct()
        {
            _virtualCameras = new List<CinemachineVirtualCamera>() { _startCam, _GameEndCame, _vehicleCam };
        }
        
        
        public void ActivateStartCam()
        {
            SetLowAllCameras();
            _startCam.Priority = _highPriority;
        }
        
        public void ActivateVehicleCam()
        {
            SetLowAllCameras();
            _vehicleCam.Priority = _highPriority;
        }
        
        public void ActivateStopCam()
        {
            SetLowAllCameras();
            _GameEndCame.Priority = _highPriority;
        }

        private void SetLowAllCameras()
        {
            foreach (var virtualCamera in _virtualCameras)
            {
                virtualCamera.Priority = _lowPriority;
            }
        }
    }
}