using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderElement : MonoBehaviour
    {
        public Slider Slider;
        public TMP_Text ValueText;
        public Vector2 MinMaxValues;
        public bool visualizeAsInt;

        public Action<float> OnValueChanged;
        
        public void Construct()
        {
            Slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void Destruct()
        {
            Slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        public void SetValue(float value)
        {
            var min = MinMaxValues.x;
            var max = MinMaxValues.y;
            var diff = max - min;
            if (value > max)
                value = max;

            if (value < min)
                value = min;

            var sliderValue = (value - min) / diff;
            if (sliderValue == Slider.value)
            {
                OnSliderValueChanged(sliderValue);
            }
            Slider.value = sliderValue;
        }
        
        private void OnSliderValueChanged(float value)
        {
            var min = MinMaxValues.x;
            var max = MinMaxValues.y;

            var diff = max - min;
            var resultValue = min + diff * value;
            SetValueText(resultValue);
            OnValueChanged?.Invoke(resultValue);
        }

        private void SetValueText(float value)
        {
            if (visualizeAsInt)
            {
                value = Mathf.FloorToInt(value);
            }
            ValueText.SetText($"{value}");
        }
        
    }
}