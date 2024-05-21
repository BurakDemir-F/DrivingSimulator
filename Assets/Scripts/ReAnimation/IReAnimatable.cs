using UnityEngine;

namespace ReAnimatable
{
    public interface IReAnimatable
    {
        void PrepareReanimation();
        void HandleReAnimationEnd();
        Vector3 GetPosition();
        Quaternion GetRotation();
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
    }
}