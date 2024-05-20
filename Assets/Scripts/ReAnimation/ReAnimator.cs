using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReAnimatable
{
    public class ReAnimator : MonoBehaviour
    {
        [SerializeField] private float _reAnimateDuration = 1f;
        [SerializeField] private float _updateSnapShotDuration = .1f;
        [SerializeField] private int _initialSnapShotAmount = 20;
        [SerializeField] private VehicleEngine _vehicle;
        private IReAnimatable _reAnimatable;
        private LinkedList<TransformSnapShot> _snapShots = new();
        private bool _isActivated;
        private bool _isAnimating;
        private float _updateSnapShotCounter;

        private TransformSnapShot _from;
        private TransformSnapShot _to;
        private float _fromToCounter;

        public event Action OnReanimationEnd;

        public void SetReAnimatable(IReAnimatable reAnimatable)
        {
            _reAnimatable = reAnimatable;
        }

        private void Awake()
        {
            SetReAnimatable(_vehicle);
            Activate();
        }

        public void Activate()
        {
            _vehicle.OnCrash += OnVehicleCrashed;
            _isActivated = true;
        }

        public void Deactivate()
        {
            _vehicle.OnCrash -= OnVehicleCrashed;
            _isActivated = false;
        }

        public void ReAnimate()
        {
            _isAnimating = true;
        }

        private void OnVehicleCrashed()
        {
            ReAnimate();
        }

        private void Update()
        {
            if (!_isActivated)
                return;

            if (!_isAnimating)
            {
                if(_updateSnapShotCounter >= _updateSnapShotDuration)
                {
                    UpdateSnapShots();
                    _updateSnapShotCounter = 0f;
                }

                _updateSnapShotCounter += Time.deltaTime;
            }
            else PlaySnapShots();
        }

        private void UpdateSnapShots()
        {
            var animTransform = _reAnimatable.ReAnimateTransform;
            _snapShots.AddFirst(new TransformSnapShot()
                { Position = animTransform.position, Rotation = animTransform.rotation, IsConstructed = true });
            Debug.Log($"new item added to snapshots, count:{_snapShots.Count}");
            
            if(_snapShots.Count >= _initialSnapShotAmount)
            {
                _snapShots.RemoveLast();
                Debug.Log($"item removed, count:{_snapShots.Count}");
            }
        }

        private void PlaySnapShots()
        {
            if (_snapShots.Count < 3)
                return;

            var speed = GetTotalDistance() / _reAnimateDuration;
            if (!_from.IsConstructed)
            {
                _reAnimatable.PrepareReanimation();
                _from = _snapShots.First.Value;
                _to = _snapShots.First.Next.Value;
            }
            else
            {
                var fromToDistance = Vector3.Distance(_from.Position, _to.Position);
                var fromToDuration = fromToDistance / speed;
                _fromToCounter += Time.deltaTime;
                var animTransform = _reAnimatable.ReAnimateTransform;

                if (_fromToCounter >= fromToDuration)
                {
                    if (_to == _snapShots.Last.Value)
                    {
                        animTransform.position = _to.Position;
                        _isAnimating = false;
                        _fromToCounter = 0f;
                        _snapShots.Clear();
                        OnReanimationEnd?.Invoke();
                        _reAnimatable.HandleReAnimationEnd();
                        return;
                    }

                    //animTransform.position = _to.Position;
                    _from = _to;
                    _to = _snapShots.Find(_to).Next.Value;
                    _fromToCounter = 0f;
                }
                else
                {
                    var lerpValue = _fromToCounter / fromToDuration;
                    animTransform.position = Vector3.Lerp(_from.Position, _to.Position, lerpValue);
                    animTransform.rotation = Quaternion.Lerp(_from.Rotation, _to.Rotation, lerpValue);
                }
            }
        }

        private float GetTotalDistance()
        {
            var totalDistance = 0f;
            var current = _snapShots.First;
            var last = _snapShots.Last;
            while (current != last && current != null)
            {
                var next = current.Next;
                var distance = Vector3.Distance(current.Value.Position, next.Value.Position);
                totalDistance += distance;
                current = current.Next;
            }

            return totalDistance;
        }
    }

    public struct TransformSnapShot
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public bool IsConstructed { get; set; }

        public static bool operator ==(TransformSnapShot first, TransformSnapShot second)
        {
            if (!first.IsConstructed || !second.IsConstructed)
                return false;

            return first.Position == second.Position && first.Rotation == second.Rotation;
        }

        public static bool operator !=(TransformSnapShot first, TransformSnapShot second)
        {
            return !(first == second);
        }
    }
}