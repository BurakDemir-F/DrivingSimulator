using System;
using UnityEngine;

namespace Utilities
{
    public interface ITransformUpdater
    {
        event Action OnPositionReached;
        event Action OnRotationReached;
        void SetPosition(Transform transformToChange,Vector3 position);
        void MoveToPosition(Transform transformToMove,Vector3 position, float duration);
        void Rotate(Transform transformToChange,float angle);
        void RotateContinuously(Transform transformToChange,float speedInAngle, float duration = -1f);
        void StopTransformChanges();
        void SetPositionMethod(Action<Transform,Vector3> setPositionMethod);
        void SetGetPositionMethod(Func<Transform,Vector3> getPositionMethod);
        void SetRotationMethod(Action<Transform,float> setRotationMethod);
        void SetGetRotationMethod(Func<Transform,Vector3> getRotationMethod);
    }
}