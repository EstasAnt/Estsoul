using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectsTool : MonoBehaviour
{
    public string WithName;

    public GameObject toObject;

    [Button("ChangeObjects")]
    public bool Change;

    public void ChangeObjects()
    {
        foreach(GameObject gameObject in FindObjectsOfType<GameObject>())
        {
            if(gameObject.name == WithName)
            {
                GameObject copy = Instantiate(toObject, gameObject.transform.parent);
                copy.transform.position = gameObject.transform.position;
                DestroyImmediate(gameObject);
            }
        }
    }
}
