using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyComponentsTool : MonoBehaviour
{
    public GameObject FromObject;
    private GameObject ToObject;

    [Button("CopyComponents")]
    public bool Copy;


    private void OnValidate()
    {
        ToObject = gameObject;
    }

    public void CopyComponents()
    {
        if(FromObject == null)
            return;
        if(ToObject == null)
            return;
        var componentsToCopy = FromObject.GetComponents<Behaviour>();
        foreach (var original in componentsToCopy)
        {
            if(original is Transform)
                continue;
            CopyComponent(original, ToObject);
        }
    }
    
    private void CopyComponent(Component original, GameObject destination)
    {
        UnityEditorInternal.ComponentUtility.CopyComponent(original);
        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(destination);
    }
}
