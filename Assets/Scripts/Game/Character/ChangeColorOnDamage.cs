using System;
using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

public class ChangeColorOnDamage : MonoBehaviour
{
    public Color DamagedColor;

    public float AnimationTime;
    public AnimationCurve AnimationCurve;
    
    private IDamageable _Idamageable;
    private Renderer _Renderer;

    private Color _DefaultColor;
    
    private void Awake()
    {
        _Idamageable = GetComponentInParent<IDamageable>();
        _Renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _Idamageable.OnDamage += OnDamage;
        _DefaultColor = _Renderer.material.color;
    }

    private void OnDamage(IDamageable arg1, Damage arg2)
    {
        if(arg2.Amount <= 0)
            return;
        StopAllCoroutines();
        StartCoroutine(ChangeColorRoutine());
    }

    private IEnumerator ChangeColorRoutine()
    {
        var lastDamageTime = Time.timeSinceLevelLoad;
        var cof = 0f;
        while (cof < 1f)
        {
            cof = Mathf.InverseLerp(0, AnimationTime, Time.timeSinceLevelLoad - lastDamageTime);
            var curveCof = AnimationCurve.Evaluate(cof);
            _Renderer.material.color = Color.Lerp(_DefaultColor, DamagedColor, curveCof);
            yield return null;
        }
        _Renderer.material.color = _DefaultColor;
    }
    
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         OnDamage(null, null);
    //     }
    // }
}
