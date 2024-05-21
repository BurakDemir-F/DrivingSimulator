using System;
using Cinemachine;
using UnityEngine;

namespace Utilities
{
    public class CameraChanger : MonoBehaviour
    {
        [SerializeField] private Camera _cinemachineMain;
        [SerializeField] private CinemachineVirtualCamera _virtualCam;
        [SerializeField] private Camera _simpleFollower;

        private void Update()
        {
            var cPressed = Input.GetKeyDown(KeyCode.C);
            if(!cPressed)
                return;
            
            _cinemachineMain.gameObject.SetActive(!_cinemachineMain.gameObject.activeSelf);
            _virtualCam.gameObject.SetActive(_cinemachineMain.gameObject.activeSelf);
            
            _simpleFollower.gameObject.SetActive(!_cinemachineMain.gameObject.activeSelf);
        }
    }
}