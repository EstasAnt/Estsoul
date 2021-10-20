using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform BorderTransform;
    public RectTransform StickTransform;
    public CanvasGroup HideableCanvasGroup;
    public bool HideOnPointerUp;
    public float MaxDistance;
    public bool UseBorderSizeMaxDist;
    [Range(0, 0.5f)]
    public float DeathZone;

    private float DistanceToBorder;
    
    public Vector2 Axis
    {
        get
        {
            var stickVector = StickTransform.position.ToVector2() - BorderTransform.position.ToVector2();
            var axis = stickVector / DistanceToBorder;
            if (axis.x < DeathZone)
                axis = new Vector2(0, axis.y);
            if (axis.y < DeathZone)
                axis = new Vector2(axis.x, 0);
            if (axis.x > 1 - DeathZone)
                axis = new Vector2(1, axis.y);
            if (axis.y > 1 - DeathZone)
                axis = new Vector2(axis.x, 1);
            return axis;
        }
    }

    private IEnumerator Start()
    {
        DistanceToBorder = BorderTransform.rect.width / 2f;
        if (UseBorderSizeMaxDist)
        {
            MaxDistance = DistanceToBorder;
        }

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            Debug.LogError($"x:{Axis.x}, y:{Axis.y}");
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        MoveStick(eventData.position);
        SetAlpha(1f);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        MoveStick(BorderTransform.position);
        SetAlpha(0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveStick(eventData.position);
    }

    private void MoveStick(Vector2 pointPosition)
    {
        var targetVector = pointPosition - BorderTransform.position.ToVector2();
        var magnitude = targetVector.magnitude;
        if (magnitude == 0)
        {
            StickTransform.position = BorderTransform.position;
            return;
        }
        var normalizedVector = targetVector / magnitude;
        StickTransform.position = BorderTransform.position.ToVector2() + normalizedVector * Mathf.Clamp(magnitude, 0, MaxDistance);
    }

    public void SetAlpha(float alpha)
    {
        if(HideOnPointerUp)
            HideableCanvasGroup.alpha = alpha;
    }
}
