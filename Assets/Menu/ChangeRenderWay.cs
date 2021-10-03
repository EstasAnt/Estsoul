using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeRenderWay : MonoBehaviour
{
    void Start()
    {
        foreach(SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            Image image=spriteRenderer.gameObject.AddComponent<Image>();
            image.sprite = spriteRenderer.sprite;
            image.material = spriteRenderer.material;
            Destroy(spriteRenderer);
        }
    }
}
