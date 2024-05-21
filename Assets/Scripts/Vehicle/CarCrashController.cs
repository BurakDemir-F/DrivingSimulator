using System;
using ReAnimatable;
using UnityEngine;

namespace Vehicle
{
    public class CarCrashController : MonoBehaviour
    {
        private ReAnimator _reAnimator;
        private VehicleEngine _engine;

        private void Awake()
        {
            _engine = GetComponent<VehicleEngine>();
            _reAnimator = GetComponent<ReAnimator>();
            _reAnimator.SetReAnimatable(_engine);
            _reAnimator.Activate();

            _engine.OnCrash += CarCrashHandler;
            _reAnimator.OnReanimationEnd += ReAnimationEndHandler;
        }

        private void OnDestroy()
        {
            _engine.OnCrash -= CarCrashHandler;
            _reAnimator.OnReanimationEnd -= ReAnimationEndHandler;
        }

        private void CarCrashHandler()
        {
            _reAnimator.ReAnimate();
        }

        private void ReAnimationEndHandler()
        {
            _engine.Activate();
        }
    }
}