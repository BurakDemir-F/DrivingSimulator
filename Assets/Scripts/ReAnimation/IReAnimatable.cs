using UnityEngine;

namespace ReAnimatable
{
    public interface IReAnimatable
    {
        void PrepareReanimation();
        void HandleReAnimationEnd();
        Transform ReAnimateTransform { get; } 
    }
}