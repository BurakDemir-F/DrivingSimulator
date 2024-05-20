using General;
using UnityEngine;

namespace Vehicle.VehicleCheckers
{
    public class CrashChecker : VehicleChecker
    {
        private int _wheelCountInTheAir;
        private bool _isCounting;
        private float _airCounter;
        public override void Check()
        {
            if(!_isActivated)
                return;
            
            var airWheels = _wheels.GetWheelsInTheAir(_vehicleData.AirLimit);
            var count = airWheels.Count;
            var crashDuration = _vehicleData.CrashDuration;
            
            if (!_isCounting)
            {
                _isCounting = true;
                return;
            }

            if (count < 2)
            {
                _isCounting = false;
                _airCounter = 0f;
            }
            
            if (count >= 2)
            {
                _airCounter += Time.deltaTime;
            }

            if (_airCounter >= crashDuration)
            {
                _eventBus.Publish(VehicleInfoType.Crashed, new CrashData());
                _isCounting = false;
                _airCounter = 0f;
            }
        }
    }
    
    public class CrashData : EventBusData{}
}