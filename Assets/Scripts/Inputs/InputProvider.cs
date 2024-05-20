using System;
using UnityEngine;

namespace Inputs
{
    public class InputProvider : MonoBehaviour, IInputProvider
    {
        public float HorizontalAxis { get; private set; }
        public float VerticalAxis { get; private set; }
        public bool IsBrake { get; private set; }
        
        private void Update()
        {
            HorizontalAxis = Input.GetAxis("Horizontal");
            VerticalAxis = Input.GetAxis("Vertical");
            IsBrake = Input.GetKey(KeyCode.Space);
        }
    }

    public interface IInputProvider
    {
        float HorizontalAxis { get; }
        float VerticalAxis { get; }
    }
}