using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Npc : MonoBehaviour, Staging.IStageElement
{
    private TextMesh text;
    
    private bool visible = false;
    private bool autoScale = false;

    private SpriteRenderer spriteRenderer;
    private IEnumerable<Transform> children;

    private void Start()
    {
        text = GetComponentInChildren<TextMesh> ();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        children = GetComponentsInChildren<Transform>().Skip(1);
        visible = spriteRenderer.enabled;
    }

    void Update() 
    { 
        // Used for highlighning the npc
        if (autoScale)
        {
            transform.localScale = Vector3.one * (1 + 0.01f * Mathf.Sin(10 * Time.time));
        }
    }

    public void SetText(string dialog)
    {
        text.text = dialog;
    }

    public void SetVisibility(bool visibility)
    {
        if (this.visible == visibility) return;
        this.visible = visibility;
        spriteRenderer.enabled = visibility;
        foreach (var t in children)
        {
            t.gameObject.SetActive(visibility);
        }
    }
 
    public void SetHighlight(bool highlight)
    {
        if (this.autoScale == highlight) return;
        this.autoScale = highlight;
    }
}
