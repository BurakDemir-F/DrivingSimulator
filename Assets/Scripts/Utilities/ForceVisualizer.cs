using System;
using UnityEngine;

namespace Utilities
{
    public class ForceVisualizer : MonoBehaviour
    {
        private static ForceVisualizer _instance;

        public static ForceVisualizer Instance
        {
            get
            {
                if (_instance)
                    return _instance;

                _instance = GameObject.FindObjectOfType<ForceVisualizer>();

                if (_instance) 
                    return _instance;

                _instance = new GameObject("ForceVisualizer").AddComponent<ForceVisualizer>();
                return _instance;
            }

            private set { _instance = value; }
        }

        private Color _lineColor = Color.blue;
        private bool _isDrawing;
        private float _drawingDuration;
        private float _drawingCounter;
        private Transform _forceTransform;
        private Vector3 _forceVec;
        public void VisualizeForce(Transform forceTransform, Vector3 forceVec, float drawingDuration)
        {
            if(_isDrawing)
                return;
            _isDrawing = true;
            _drawingDuration = drawingDuration;
            _drawingCounter = 0f;
            _forceTransform = forceTransform;
            _forceVec = forceVec;
        }

        private void FixedUpdate()
        {
            if(!_isDrawing)
                return;

            if (_drawingDuration <= _drawingCounter)
            {
                _isDrawing = false;
                _drawingCounter = 0f;
                _forceTransform = null;
                return;
            }

            _drawingCounter += Time.fixedDeltaTime;
            Debug.DrawLine(_forceTransform.position,_forceTransform.position + _forceVec,_lineColor);
        }
    }
}