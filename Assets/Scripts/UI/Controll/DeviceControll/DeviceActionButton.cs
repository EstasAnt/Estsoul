using System;
using KlimLib.SignalBus;
using UnityDI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Controll.DeviceControll
{
    public class DeviceActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public DevicePlayerActionType ActionType;

        [Dependency] private readonly SignalBus _SignalBus;
        
        private Button _Button;

        private bool _PointerDown;
        
        protected void Awake()
        {
            _Button = GetComponent<Button>();
            
        }

        protected virtual void Start()
        {
            ContainerHolder.Container.BuildUp(this);
        }

        protected virtual void Update()
        {
            if(_PointerDown)
                _SignalBus.FireSignal(new DeviceActionButtonPressSignal()
                {
                    DevicePlayerActionType = ActionType,
                    ButtonEventType = ButtonEventType.Hold,
                });
        }

        protected void OnDestroy()
        {
            _Button.onClick.RemoveAllListeners();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _SignalBus.FireSignal(new DeviceActionButtonPressSignal()
            {
                DevicePlayerActionType = ActionType,
                ButtonEventType = ButtonEventType.Press,
            });
            _PointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _SignalBus.FireSignal(new DeviceActionButtonPressSignal()
            {
                DevicePlayerActionType = ActionType,
                ButtonEventType = ButtonEventType.Release,
            });
            _PointerDown = false;
        }
    }
}