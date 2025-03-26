using UnityEngine;
using UnityEngine.Events;
using NarrativeEngine;

// This class controls the lifecycle of the Narrative Model instance.

public class NarrativeSequencer : MonoBehaviour
{
    [HideInInspector] public UnityEvent PlotLoaded = new UnityEvent();

    NarrativeModel narrativeModel;

    private void Awake()
    {
        BuildNarrativeModel();
    }

    private void Start()
    {
        narrativeModel.LoadPlot("Act1");
        PlotLoaded.Invoke();
    }

    void BuildNarrativeModel()
    {
        narrativeModel = new NarrativeModel();

        // Construye narrativa

        narrativeModel.AddPlot("Act1")
        .AddNode("Cuadrado")
        .AddNode("Triangulo", "Cuadrado")
        .AddNode("Circulo", "Triangulo")
        .AddNode("Azul", "Circulo");

        narrativeModel.AddPlot("Act2")
        .AddNode("Cuadrado")
        .AddNode("Rombo")
        .AddNode("Circulo", "Cuadrado")
        .AddNode("Triangulo", "Cuadrado")
        .AddNode("Azul", "Circulo,Triangulo,Rombo", ActivationTypes.all)
        .AddNode("Verde", "Triangulo,Circulo", ActivationTypes.any);

        narrativeModel.AddPlot("Act3")
        .AddMilestone("Cuadrado")
        .AddMilestone("Rombo")
        .AddNode("Triangulo", "Cuadrado")
        .AddNode("Circulo", "Rombo")
        .AddNode("Azul", "Circulo")
        .AddNode("Verde", "Triangulo", ActivationTypes.any);
    }
  
    private void NextPlot()
    {
        // Iterate across the three acts
        switch (narrativeModel.plotPointer.GetPlotLabel())
        {
            case "Act1": narrativeModel.LoadPlot("Act2"); break;
            case "Act2": narrativeModel.LoadPlot("Act3"); break;
            case "Act3": narrativeModel.LoadPlot("Act1"); break;
        }
        PlotLoaded.Invoke();
    }
    public bool TryActivateNode(string node)
    {
        var activated = narrativeModel.TryActivateNode(node, out bool isEnding);
        if (isEnding) NextPlot();
        return activated;
    }
    internal bool IsNodeActivable(string node) => narrativeModel.IsNodeActivable(node);
    internal bool IsNodeActivated(string node) => narrativeModel.IsNodeActivated(node);
    internal string GetCurrentPlotLabel() =>  narrativeModel.plotPointer.GetPlotLabel();
}