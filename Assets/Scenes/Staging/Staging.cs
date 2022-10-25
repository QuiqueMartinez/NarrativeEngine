﻿using UnityEngine;

// Class that takes control of the stage
public class Staging : MonoBehaviour
{
    public interface IStageElement
    {
        public void SetVisible(bool visible);
        public void SetHighLight(bool visible);
    }

    public TextMesh BannerText;
    public Npc Square;
    public Npc Circle;
    public Npc Triangle;
    public Npc Diamond;

    NarrativeSequencer narrativeSequencer;

    const string blue = "<b><color=blue>Blue</color></b>";
    const string green = "<b><color=green>Green</color></b>";

    void OnEnable()
    {
        narrativeSequencer = FindObjectOfType<NarrativeSequencer>();
        narrativeSequencer.PlotLoaded.AddListener(OnPlotLoaded);
    }

    private void OnPlotLoaded()
    {
        switch (narrativeSequencer.GetCurrentPlotLabel())
        {
            case "Straight":
                SetupScene1();
                break;
            case "Predicate":
                SetupScene2();
                break;
            case "Milestone":
                SetupScene3();
                break;
        }
    }

    public void SetupScene1()
    {
        BannerText.text = "Act 1:\n Little by little one goes far.";
        Square.SetText("Talk to me");
        Triangle.SetText("Talk to me");
        Circle.SetText("Talk to me to\n open the " + blue + " portal");
    }
    public void SetupScene2()
    {
        BannerText.text = "Act 2:\nCuriosity killed the cat.";
        Square.SetText("Talk to me");
        Triangle.SetText("Talk to me to\n open the " + green + " portal ...");
        Circle.SetText("... I can open open \nthe " + green + " portal as well");
        Diamond.SetText("I'll tell you a secret:\ntalk to all of us to\nopen the " + blue + " portal.");
    }
    public void SetupScene3()
    {
        BannerText.text = "Act 3:\n Sit on the fence.";
        Square.SetText("Talk to me \nto kill the evil ♦ \n and release ▲");
        Diamond.SetText("No, talk to me \nto kill the evil ■\n and release ●");
        Triangle.SetText("Thanks!. Talk to me to\n open the " + green + " portal.");
        Circle.SetText("Thanks!. Talk to me to\n open the " + blue + " portal.");
    }
}
