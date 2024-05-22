using System;
using Managers;
using UnityEngine;
using Vehicle.Data;

namespace UI
{
    public class PropertiesScreen : MonoBehaviour
    {
        [SerializeField] private VehicleSo _vehicleData;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private SliderElement _fTorqueSlider;
        [SerializeField] private SliderElement _bTorqueSlider;
        [SerializeField] private SliderElement _brakeTorqueSlider;
        [SerializeField] private SliderElement _preventTurnOverForce;
        [SerializeField] private SliderElement _backToLifeCount;

        private void Awake()
        {
            _fTorqueSlider.Construct();
            _bTorqueSlider.Construct();
            _brakeTorqueSlider.Construct();
            _preventTurnOverForce.Construct();
            _backToLifeCount.Construct();
            
            _fTorqueSlider.OnValueChanged += FTorqueChanged;
            _bTorqueSlider.OnValueChanged += BTorqueChanged;
            _brakeTorqueSlider.OnValueChanged += BrakeTorqueChanged;
            _preventTurnOverForce.OnValueChanged += TurnOverForceChanged;
            _backToLifeCount.OnValueChanged += BackToLifeCountChanged;
            SetInitialValues();
        }

        private void OnDestroy()
        {
            _fTorqueSlider.Destruct();
            _bTorqueSlider.Destruct();
            _brakeTorqueSlider.Destruct();
            _preventTurnOverForce.Destruct();
            _backToLifeCount.Destruct();
            
            _fTorqueSlider.OnValueChanged -= FTorqueChanged;
            _bTorqueSlider.OnValueChanged -= BTorqueChanged;
            _brakeTorqueSlider.OnValueChanged -= BrakeTorqueChanged;
            _preventTurnOverForce.OnValueChanged -= TurnOverForceChanged;
            _backToLifeCount.OnValueChanged = BackToLifeCountChanged;
        }

        private void SetInitialValues()
        {
            _fTorqueSlider.SetValue(_vehicleData.ForwardMotorTorque);
            _bTorqueSlider.SetValue(_vehicleData.BackwardMotorTorque);
            _brakeTorqueSlider.SetValue(_vehicleData.BrakeTorque);
            _preventTurnOverForce.SetValue(_vehicleData.PreventTurnoverForce);
            _backToLifeCount.SetValue(_gameManager.GetAllowedCrashCount());
        }
        
        private void FTorqueChanged(float value)
        {
            _vehicleData.ForwardMotorTorque = value;
        }
        private void BTorqueChanged(float value)
        {
            _vehicleData.BackwardMotorTorque = value;
        }
        private void BrakeTorqueChanged(float value)
        {
            _vehicleData.BrakeTorque = value;
        }
        private void TurnOverForceChanged(float value)
        {
            _vehicleData.PreventTurnoverForce = value;
        }
        private void BackToLifeCountChanged(float value)
        {
            _gameManager.SetAllowedCrashCount(Mathf.FloorToInt(value));
        }
    }
}