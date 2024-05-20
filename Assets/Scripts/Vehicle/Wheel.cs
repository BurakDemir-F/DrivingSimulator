using UnityEngine;

[System.Serializable]
public class Wheel : MonoBehaviour
{
    [SerializeField] public WheelCollider Collider;
    [SerializeField] public WheelSide Side;
}