using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
[ExecuteAlways]
public class FWCam : MonoBehaviour
{
    public float fullWidthUnits = 14;

    void Start()
    {
        UpdateSize();
    }

#if UNITY_EDITOR
    private void Update()
    {
        UpdateSize();
    }
#endif

    private void UpdateSize()
    {
        // Force fixed width
        float ratio = (float)Screen.height / (float)Screen.width;
        var vc = GetComponent<CinemachineVirtualCamera>();
        vc.m_Lens.OrthographicSize = (float)fullWidthUnits * ratio / 2.0f;
    }
}
