using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;
using KlimLib.SignalBus;
using DebugTools;

#if FAST_SKIP_ENABLED
public class TPPointsList : MonoBehaviour
{
    [Dependency] private SignalBus _signalBus;

    public MonoBehaviour character;

    public List<GameObject> points = new List<GameObject>();

    public int currentPoint = 0;

    private void Awake()
    {
        ContainerHolder.Container.BuildUp(this);
        ContainerHolder.Container.RegisterInstance(this);
        _signalBus.FireSignal(new SceneChangedSignal());
    }

    public GameObject GetNext()
    {
        currentPoint++;
        if (currentPoint >= points.Count) currentPoint = 0;
        return points[currentPoint];
    }

    public Vector2 GetNextV2()
    {
        return GetNext().transform.position;
    }

    public void OnDestroy()
    {
        _signalBus.FireSignal(new SceneChangedSignal());
    }
}
#endif