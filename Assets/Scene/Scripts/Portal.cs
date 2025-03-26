using UnityEngine;

public class Portal : MonoBehaviour, Staging.IStageElement
{
    private SpriteRenderer spriteRenderer;
    private bool visible = false;
    private bool autoRotate = false;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        visible = spriteRenderer.enabled;
    }

    void Update()
    {
        if (autoRotate)
        {
            transform.eulerAngles = transform.eulerAngles - 40 * Vector3.forward * Time.deltaTime;
        }
    }


    public void SetVisibility(bool visibility)
    {
        if (this.visible == visibility) return;
        this.visible = visibility;
        spriteRenderer.enabled = visibility;
    }

    public void SetHighlight(bool highlight)
    {
        if (this.autoRotate == highlight) return;
        this.autoRotate = highlight;
    }
}
