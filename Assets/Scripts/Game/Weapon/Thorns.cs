using System;
using System.Collections;
using System.Collections.Generic;
using Character.Health;
using UnityEngine;

public class Thorns : MonoBehaviour
{
    public float Dmg;
    public float DmgCooldown;
    public LayerMask LayerMask;

    private Dictionary<IDamageable, Coroutine> _DamageablesCoroutines = new Dictionary<IDamageable, Coroutine>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!Layers.Masks.LayerInMask(LayerMask, other.gameObject.layer))
            return;
        var dmgbl = other.GetComponent<IDamageable>();
        if(dmgbl == null)
            return;
        if (!_DamageablesCoroutines.ContainsKey(dmgbl))
        {
            var cor = StartCoroutine(DmgRoutine(dmgbl));
            _DamageablesCoroutines.Add(dmgbl, cor);
        }
    }

    private IEnumerator DmgRoutine(IDamageable dmgbl)
    {
        yield return null;
        while (dmgbl != null && _DamageablesCoroutines.ContainsKey(dmgbl))
        {
            dmgbl.ApplyDamage(new Damage(null, dmgbl, Dmg));
            yield return new WaitForSeconds(DmgCooldown);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!Layers.Masks.LayerInMask(LayerMask, other.gameObject.layer))
            return;
        var dmgbl = other.GetComponent<IDamageable>();
        if(dmgbl == null)
            return;
        if(_DamageablesCoroutines.ContainsKey(dmgbl))
            _DamageablesCoroutines.Remove(dmgbl);
    }
}
