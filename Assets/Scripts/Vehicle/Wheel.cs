using UnityEngine;

[System.Serializable]
public class Wheel : MonoBehaviour
{
    [SerializeField] public WheelCollider Collider;
    [SerializeField] public WheelSide Side;

    public bool IsLeft() => Side == WheelSide.Left;
    public bool IsRight() => Side == WheelSide.Right;
}