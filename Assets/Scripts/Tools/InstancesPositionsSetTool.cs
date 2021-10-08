using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancesPositionsSetTool : MonoBehaviour
{
    [Button("ChangePlace")]
    public bool Place;

    public GameObject copiesParent, changingsParent;
    public string WithName;

    public void ChangePlace()
    {
        GameObject[] copies = new GameObject[copiesParent.transform.childCount], toChange = new GameObject[changingsParent.transform.childCount];
        for (int i = 0; i < copies.Length; i++) copies[i] = copiesParent.transform.GetChild(i).gameObject;
        for (int i = 0; i < toChange.Length; i++) toChange[i] = changingsParent.transform.GetChild(i).gameObject;
        for (int i =0,j=0;i<toChange.Length&j<copies.Length;i++)
        {
            if (toChange[i].name.Contains(WithName))
            {
                copies[j].transform.SetParent(toChange[i].transform.parent);
                copies[j].transform.position = toChange[i].transform.position;
                DestroyImmediate(toChange[i]);
                j++;
            }
        }
    }
}
