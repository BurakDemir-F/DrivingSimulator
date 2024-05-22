using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private AnimatableUI _title;
        [SerializeField] private AnimatableUI _playButton;
        [SerializeField] private AnimatableUI _quitBUtton;
        
        [SerializeField] private float _animationDuration;
        
        public event Action OnPlayGameAnimationsEnd;

        private List<AnimatableUI> _uiObjects;

        private int _animationEndCounter;
        private int AnimationEndCounter
        {
            get => _animationEndCounter;
            set
            {
                _animationEndCounter = value;
                if (_animationEndCounter >= 3)
                {
                    OnPlayGameAnimationsEnd?.Invoke();
                }
            }
        }
        
        private void Start()
        {
            _playButton.GetComponent<Button>().onClick.AddListener(PlayButtonClickedHandler);
            _quitBUtton.GetComponent<Button>().onClick.AddListener(QuitButtonClickedHandler);
            _uiObjects = new List<AnimatableUI>() { _title, _playButton, _quitBUtton };
            foreach (var animatableUI in _uiObjects)
            {
                animatableUI.OnTargetReached += UIElementAnimationEndHandler;
            }
        }

        private void OnDestroy()
        {
            _playButton.GetComponent<Button>().onClick.RemoveListener(PlayButtonClickedHandler);
            _quitBUtton.GetComponent<Button>().onClick.RemoveListener(QuitButtonClickedHandler);
            // foreach (var animatableUI in _uiObjects)
            // {
            //     animatableUI.OnTargetReached -= UIElementAnimationEndHandler;
            // }
        }

        private void PlayButtonClickedHandler()
        {
            foreach (var uiObject in _uiObjects)
            {
                var transformUpdater = uiObject.TransformUpdater;
                transformUpdater.SetPositionMethod(SetPositionMethod);
                transformUpdater.SetGetPositionMethod(GetPositionMethod);
                uiObject.MoveToTarget(_animationDuration);
            }
        }

        private void QuitButtonClickedHandler()
        {
            Application.Quit();
        }

        private void UIElementAnimationEndHandler()
        {
            AnimationEndCounter++;
        }

        private void SetPositionMethod(Transform transformToChange, Vector3 newPosition)
        {
            var rectTransform = transformToChange as RectTransform;
            rectTransform.anchoredPosition = newPosition;
        }
        
        private Vector3 GetPositionMethod(Transform transformToChange)
        {
            var rectTransform = transformToChange as RectTransform;
            return rectTransform.anchoredPosition;
        }
    }

    [System.Serializable]
    public class AnimatableUI
    {
        public UIBehaviour UIElement;
        public RectTransform Target;
        public TransformUpdater TransformUpdater;
        public event Action OnTargetReached;

        public void MoveToTarget(float duration)
        {
            TransformUpdater.OnPositionReached += TargetReachedHandler;
            TransformUpdater.MoveToPosition(UIElement.transform,Target.anchoredPosition,duration);
        }

        private void TargetReachedHandler()
        {
            OnTargetReached?.Invoke();
            TransformUpdater.OnPositionReached -= TargetReachedHandler;
        }

        public T GetComponent<T>() where T: Component
        {
            return UIElement.GetComponent<T>();
        }
    }
}