using UnityEngine;
using NarrativeEngine;
/// <summary>
/// Relates a Game Object with a Narrative Unit
/// </summary>
public class NarrativeBinding : MonoBehaviour, IBoundtoNarrative
{
    private static NarrativeSequencer _narrativeSequencer;

    Staging.IStageElement stageBound; 

    void Start()
    {
        if (_narrativeSequencer==null)
        _narrativeSequencer = FindObjectOfType<NarrativeSequencer>();
        stageBound = GetComponent<Staging.IStageElement>();
    }

    public bool BoundNodeTryActivate() => _narrativeSequencer.TryActivateNode(gameObject.name);
    public bool BoundNodeIsActivable() => _narrativeSequencer.IsNodeActivable(gameObject.name);
    public bool BoundNodeIsActivated() => _narrativeSequencer.IsNodeActivated(gameObject.name);

    public void Update()
    {
        stageBound.SetVisible( BoundNodeIsActivable() || BoundNodeIsActivated());
        stageBound.SetHighLight(BoundNodeIsActivable() && !BoundNodeIsActivated());  
    }

    private void OnMouseDown()
    {
        BoundNodeTryActivate();
    }
}
