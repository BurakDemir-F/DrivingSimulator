using System;
using ReAnimatable;
using UnityEngine;

namespace Vehicle
{
    public class VehicleController : MonoBehaviour
    {
        private ReAnimator _reAnimator;
        private VehicleEngine _engine;
        public Action OnCarCrashed;
        public Action OnReanimationEnd;

        public void Construct()
        {
            _engine = GetComponent<VehicleEngine>();
            _reAnimator = GetComponent<ReAnimator>();
            _engine.Construct();
            _reAnimator.SetReAnimatable(_engine);

            _engine.OnCrash += CarCrashHandler;
            _reAnimator.OnReanimationEnd += ReAnimationEndHandler;
        }

        public void Activate()
        {
            _engine.Activate();
            _reAnimator.Activate();
        }

        public void Destruct()
        {
            _engine.Destruct();
            _engine.OnCrash -= CarCrashHandler;
            _reAnimator.OnReanimationEnd -= ReAnimationEndHandler;
        }

        public void ReAnimate()
        {
            _reAnimator.ReAnimate();
        }

        private void CarCrashHandler()
        {
            OnCarCrashed?.Invoke();
        }

        private void ReAnimationEndHandler()
        {
            // _engine.Activate();
            OnReanimationEnd?.Invoke();
        }
    }
}