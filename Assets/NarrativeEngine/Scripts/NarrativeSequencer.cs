using UnityEngine;
using UnityEngine.Events;
using NarrativeEngine;

/// <summary>
/// This class controls the lifecycles of the Narrative Model instances.
/// </summary>
public class NarrativeSequencer : MonoBehaviour
{
    public UnityEvent PlotLoaded = new UnityEvent();

    NarrativeModel narrativeModel;

    private void Awake()
    {
        narrativeModel = new NarrativeModel();

        // Construye narrativa

        narrativeModel.AddPlot("Straight")
        .AddNode("Cuadrado")
        .AddNode("Triangulo", "Cuadrado")
        .AddNode("Circulo", "Triangulo")
        .AddNode("Azul", "Circulo");

        narrativeModel.AddPlot("Predicate")
        .AddNode("Cuadrado")
        .AddNode("Rombo")
        .AddNode("Circulo", "Cuadrado")
        .AddNode("Triangulo", "Cuadrado")
        .AddNode("Azul", "Circulo,Triangulo,Rombo", ActivationTypes.all)
        .AddNode("Verde", "Triangulo,Circulo", ActivationTypes.any);

        narrativeModel.AddPlot("Milestone")
        .AddMilestone("Cuadrado")
        .AddMilestone("Rombo")
        .AddNode("Triangulo", "Cuadrado")
        .AddNode("Circulo", "Rombo")
        .AddNode("Azul", "Circulo")
        .AddNode("Verde", "Triangulo", ActivationTypes.any);

    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        narrativeModel.LoadPlot("Straight");
        PlotLoaded.Invoke();
    }

    private void Next()
    {
        switch (narrativeModel.plotPointer.GetPlotLabel())
        {
            case "Straight": narrativeModel.LoadPlot("Predicate"); break;
            case "Predicate": narrativeModel.LoadPlot("Milestone"); break;
            case "Milestone": narrativeModel.LoadPlot("Straight"); break;
        }
        PlotLoaded.Invoke();
    }

    public bool TryActivateNode(string node)
    {
        var activated = narrativeModel.TryActivateNode(node, out bool isEnding);
        if (isEnding) Next();
        return activated;
    }
    internal bool IsNodeActivable(string node) => narrativeModel.IsNodeActivable(node);
    internal bool IsNodeActivated(string node) => narrativeModel.IsNodeActivated(node);
    internal string GetCurrentPlotLabel() =>  narrativeModel.plotPointer.GetPlotLabel();

}
