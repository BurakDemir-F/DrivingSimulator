using UI;
using UnityEngine;
using Vehicle;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MainScreen _mainScreen;
        [SerializeField] private VehicleController _vehicleController;
        [SerializeField] private GameCameras _gameCameras;
        [SerializeField] private int _allowedCrashCount;

        private int _crashCounter;

        private void Awake()
        {
            _vehicleController.Construct();
            _gameCameras.Construct();
            _gameCameras.ActivateStartCam();
            
            _mainScreen.OnPlayGameAnimationsEnd += GameStartHandler;
            _vehicleController.OnCarCrashed += CarCrashedHandler;
            _vehicleController.OnReanimationEnd += ReAnimationEndHandler;
        }

        private void OnDestroy()
        {
            _vehicleController.Destruct();
            _mainScreen.OnPlayGameAnimationsEnd -= GameStartHandler;
            _vehicleController.OnCarCrashed -= CarCrashedHandler;
            _vehicleController.OnReanimationEnd -= ReAnimationEndHandler;
        }

        private void GameStartHandler()
        {
            _gameCameras.ActivateVehicleCam();
        }

        private void CarCrashedHandler()
        {
            if (++_crashCounter <= _allowedCrashCount)
            {
                _vehicleController.ReAnimate();
            }
        }

        private void ReAnimationEndHandler()
        {
            _vehicleController.Activate();
        }
    }
}