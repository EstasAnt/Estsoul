using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Controll.DeviceControll
{
    public class SimpleJoystickShower : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
    {
        public SimpleJoystick Joystick;

        public void OnPointerUp(PointerEventData eventData)
        {
            Joystick.transform.position = transform.position;
            Joystick.SetAlpha(0f);
            Joystick.OnPointerUp(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Joystick.transform.position = eventData.position;
            Joystick.SetAlpha(1f);
            Joystick.OnPointerDown(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Joystick.OnDrag(eventData);
        }
    }
}