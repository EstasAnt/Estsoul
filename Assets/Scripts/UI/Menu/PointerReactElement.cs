using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class PointerReactElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public List<GameObject> Elements;

        private void OnEnable()
        {
            Elements.ForEach(_ => _.SetActive(false));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Elements.ForEach(_ => _.SetActive(true));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Elements.ForEach(_ => _.SetActive(false));
        }
    }
}