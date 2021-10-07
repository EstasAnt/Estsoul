using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class SliderEnableButton : MonoBehaviour
    {
        public Slider Slider;
        public Graphic ElementGraphic;
        public float DefaultSliderValue;
        private Button _Button;

        private void Awake()
        {
            _Button = GetComponent<Button>();
        }
        private void Start()
        {
            if(_Button != null)
                _Button.onClick.AddListener(OnPressButton);
            if(Slider != null)
                Slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        
        private void OnSliderValueChanged(float val)
        {
            RefreshElement();
        }

        private void OnPressButton()
        {
            if (Slider.value > 0)
            {
                Slider.value = 0;
            }
            else
            {
                Slider.value = DefaultSliderValue;
            }
        }

        private void RefreshElement()
        {
            ElementGraphic.enabled = Slider.value > 0;
        }
        
        private void OnDestroy()
        {
            if(Slider != null)
                Slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            if(_Button != null)
                _Button.onClick.RemoveListener(OnPressButton);
        }
    }
}