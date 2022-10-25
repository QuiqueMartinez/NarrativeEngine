using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Npc : MonoBehaviour, Staging.IStageElement
{
    private TextMesh text;
    private bool visible = false;
    private SpriteRenderer spriteRenderer;
    private IEnumerable<Transform> children;

    private void Start()
    {
        text = GetComponentInChildren<TextMesh> ();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        children = GetComponentsInChildren<Transform>().Skip(1);
        visible = spriteRenderer.enabled;
    }
    public void SetText(string dialog)
    {
        text.text = dialog;
    }

    public void SetVisible(bool visible)
    {
        if (this.visible == visible) return;
        this.visible = visible;
        spriteRenderer.enabled = visible;
        foreach (var t in children)
        {
            t.gameObject.SetActive(visible);
        }
    }

    public void SetHighLight(bool visible)
    {
        foreach (var t in children)
        {
            t.gameObject.SetActive(visible);
        }
    }
}
