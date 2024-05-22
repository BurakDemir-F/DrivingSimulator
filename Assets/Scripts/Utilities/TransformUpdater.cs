using System;
using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class TransformUpdater : MonoBehaviour, ITransformUpdater
    {
        public event Action OnPositionReached;
        public event Action OnRotationReached;
        private Coroutine _movementCor;
        private Coroutine _rotationCor;

        private bool _isMoving;
        private bool _isRotating;
        private Action<Transform,Vector3> _setPositionMethod;
        private Func<Transform,Vector3> _getPositionMethod;
        private Action<Transform,float> _setRotationMethod;
        private Func<Transform,Vector3> _getRotationMethod;

        private Transform _currentlyWorkingTransform;

        public void SetPositionMethod(Action<Transform,Vector3> setPositionMethod)
        {
            _setPositionMethod = setPositionMethod;
        }

        public void SetGetPositionMethod(Func<Transform,Vector3> getPositionMethod)
        {
            _getPositionMethod = getPositionMethod;
        }

        public void SetRotationMethod(Action<Transform,float> setRotationMethod)
        {
            _setRotationMethod = setRotationMethod;
        }

        public void SetGetRotationMethod(Func<Transform,Vector3> getRotationMethod)
        {
            _getRotationMethod = getRotationMethod;
        }


        public void SetPosition(Transform transformToChange,Vector3 position)
        {
            _setPositionMethod.Invoke(transformToChange,position);
        }
        

        public void MoveToPosition(Transform transformToChange,Vector3 position, float duration)
        {
            if (_isMoving)
                return;

            _isMoving = true;
            _movementCor = StartCoroutine(MovementCor(transformToChange,_getPositionMethod.Invoke(transformToChange), position, duration));
        }

        public void Rotate(Transform transformToChange,float angle)
        {
            _setRotationMethod.Invoke(transformToChange,angle);
        }

        public void StopTransformChanges()
        {
            StopAllCoroutines();
            _isMoving = false;
            _isRotating = false;
        }

        public void RotateContinuously(Transform transformToChange, float speedInAnglePerSecond, float duration = -1f)
        {
            if (_isRotating)
                return;

            _isRotating = true;
            _rotationCor =
                StartCoroutine(RotationCor(transformToChange,_getRotationMethod.Invoke(transformToChange).z, speedInAnglePerSecond, duration));
        }

        private IEnumerator MovementCor(Transform transformToChange,Vector3 currentPos, Vector3 targetPos, float duration)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                var newPos = Vector3.Lerp(currentPos, targetPos, elapsedTime / duration);
                SetPosition(transformToChange,newPos);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _isMoving = false;
            OnPositionReached?.Invoke();
        }

        private IEnumerator RotationCor(Transform transformToChange,float currentRotation, float speedInAnglePerSecond, float duration = -1f)
        {
            var rotateForever = duration == -1;
            if (rotateForever)
            {
                while (_isRotating)
                {
                    Rotate(transformToChange,speedInAnglePerSecond * Time.deltaTime);
                    yield return null;
                }

                OnRotationReached?.Invoke();
                _isRotating = false;
                yield break;
            }

            var elapsedTime = 0f;
            var rotatedAngle = 0f;
            while (elapsedTime < duration)
            {
                var rotation = speedInAnglePerSecond * Time.deltaTime;
                Rotate(transformToChange,rotation);
                elapsedTime += Time.deltaTime;
                rotatedAngle += rotation;
                Debug.Log($"rotation: {rotatedAngle}");
                yield return null;
            }

            OnRotationReached?.Invoke();
            _isRotating = false;
        }
    }
}