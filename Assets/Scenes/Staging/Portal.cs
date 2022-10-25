using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, Staging.IStageElement
{
    private SpriteRenderer spriteRenderer;
    private bool visible = false;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        visible = spriteRenderer.enabled;
    }

    public void SetVisible(bool visible)
    {
        if (this.visible == visible) return;
        this.visible = visible;
        spriteRenderer.enabled = visible;
    }

    public void SetHighLight(bool visible)
    {
        //throw new System.NotImplementedException();
    }
}
