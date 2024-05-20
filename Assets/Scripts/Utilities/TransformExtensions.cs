using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class TransformExtensions
    {
        private static Stack<Transform> _children = new();
        private static Stack<Transform> _innerChildren = new();

        public static List<KeyValuePair<TResult,Transform>> TraverseTransformTree<TResult>(this Transform transform, 
            Predicate<Transform> transformPredicate,
            Func<Transform, TResult> resultFunc)
        {
            var organizedTransforms = new List<KeyValuePair<TResult, Transform>>();
            var selectedTransforms = transform.TraverseTransformTree(transformPredicate);
            foreach (var selectedTransform in selectedTransforms)
            {
                organizedTransforms.Add(new KeyValuePair<TResult, Transform>(resultFunc.Invoke(selectedTransform),
                    selectedTransform));
            }

            return organizedTransforms;
        }
        
        public static List<Transform> TraverseTransformTree(this Transform transform,
            Predicate<Transform> transformPredicate)
        {
            var selectedTransforms = new List<Transform>();
            _children.Clear();
            _innerChildren.Clear();
            
            TryUpdateTransformList(transform);
            foreach (Transform childTransform in transform)
            {
                _children.Push(childTransform);
                TryUpdateTransformList(childTransform);
            }
            
            while (_children.Count > 0)
            {
                var currentChild = _children.Pop();
                foreach (Transform child in currentChild)
                {
                    _innerChildren.Push(child);
                }

                while (_innerChildren.Count > 0)
                {
                    var innerChild = _innerChildren.Pop();
                    TryUpdateTransformList(innerChild);
                    _children.Push(innerChild);
                }
                
            }

            void TryUpdateTransformList(Transform transformToCheck)
            {
                if (transformPredicate.Invoke(transformToCheck))
                {
                    selectedTransforms.Add(transformToCheck);
                }
            }

            return selectedTransforms;
        }
    }
}