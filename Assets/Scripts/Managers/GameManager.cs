using System.Collections;
using FinishArea;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Vehicle;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MainScreen _mainScreen;
        [FormerlySerializedAs("_gameOverUI")] [SerializeField] private GameResultUI gameResultUI;
        [SerializeField] private VehicleController _vehicleController;
        [SerializeField] private GameCameras _gameCameras;
        [SerializeField] private int _allowedCrashCount;
        [SerializeField] private FinishAreaController _finishAreaController;
        [SerializeField] private string _gameWinText = "Game Win";
        [SerializeField] private string _gameOverText = "GameOver";

        private int _crashCounter;

        private void Awake()
        {
            _vehicleController.Construct();
            _gameCameras.Construct();
            _gameCameras.ActivateStartCam();
            
            _mainScreen.OnPlayGameAnimationsEnd += GameStartHandler;
            _vehicleController.OnCarCrashed += CarCrashedHandler;
            _vehicleController.OnReanimationEnd += ReAnimationEndHandler;
            _finishAreaController.OnEndCameraAreaEntered += EndCameraAreaEnteredHandler;
            _finishAreaController.OnGameWin += GameWinHandler;
        }

        private void OnDestroy()
        {
            _vehicleController.Destruct();
            _mainScreen.OnPlayGameAnimationsEnd -= GameStartHandler;
            _vehicleController.OnCarCrashed -= CarCrashedHandler;
            _vehicleController.OnReanimationEnd -= ReAnimationEndHandler;
            _finishAreaController.OnEndCameraAreaEntered -= EndCameraAreaEnteredHandler;
            _finishAreaController.OnGameWin -= GameWinHandler;
        }

        private void EndCameraAreaEnteredHandler()
        {
            _gameCameras.ActivateStopCam();
        }

        private void GameWinHandler()
        {
            StartCoroutine(GameEndCor(_gameWinText));
        }

        private void GameStartHandler()
        {
            _gameCameras.ActivateVehicleCam();
            _vehicleController.Activate();
        }

        private void CarCrashedHandler()
        {
            if (_crashCounter < _allowedCrashCount)
            {
                _vehicleController.ReAnimate();
                _crashCounter++;
            }
            else
            {
                StartCoroutine(GameEndCor(_gameOverText));
            }
        }

        private void ReAnimationEndHandler()
        {
            _vehicleController.Activate();
        }

        private IEnumerator GameEndCor(string result)
        {
            gameResultUI.Activate();
            gameResultUI.SetResult(result);
            yield return new WaitForSeconds(2f);
            LoadCurrentScene();
        }

        private void LoadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}