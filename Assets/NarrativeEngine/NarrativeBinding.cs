using UnityEngine;
using UnityEngine.WSA;

public interface IBoundtoNarrative
{
    bool BoundNodeTryActivate();
    bool BoundNodeIsActivated();
    bool BoundNodeIsActivable();
}

// Relates a Game Object in the hierarchy with a Narrative
public class NarrativeBinding : MonoBehaviour, IBoundtoNarrative
{
    private static NarrativeSequencer _narrativeSequencer;
    Staging.IStageElement stageBound;

    [SerializeField] bool Activable;
    [SerializeField] bool Activated;


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
        Activable = BoundNodeIsActivable();
        Activated = BoundNodeIsActivated();

        stageBound.SetVisibility( BoundNodeIsActivable() || BoundNodeIsActivated());
        stageBound.SetHighlight(BoundNodeIsActivable() && !BoundNodeIsActivated());  
    }

    private void OnMouseDown()
    {
        BoundNodeTryActivate();
    }
}
