using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NarrativeEngine
{

    // Runs the diferent plots and narrative units
    public enum ActivationTypes
    {
        root, // Root Node
        any,  // Activated by any parent nodes
        all,  // Activated by all parent nodes
    }

    #region Narrative Model
    public class NarrativeModel
    {
        List<Plot> _narrative = new List<Plot>();

        public Plot plotPointer { get; private set; }

        public Plot AddPlot(string plotLabel)
        {
            var plot = new Plot(plotLabel);
            _narrative.Add(plot);
            return plot;
        }

        public bool IsNodeActivable(string nodelabel)
        {
            var node = plotPointer.PlotNarrativeUnits.SingleOrDefault(n => n.m_name == nodelabel);
            return (node != null) && node.IsActivable();
        }

        public bool IsNodeActivated(string nodelabel)
        {
            var node = plotPointer.PlotNarrativeUnits.SingleOrDefault(n => n.m_name == nodelabel);
            return (node != null) && node.Activated();
        }

        public bool TryActivateNode(string nodelabel, out bool isEnding)
        {
            var node = plotPointer.PlotNarrativeUnits.SingleOrDefault(n => n.m_name == nodelabel);

            isEnding = (node != null) && node.IsEnding();


            if (node is Milestone)
            {
                foreach (var n in plotPointer.PlotNarrativeUnits)
                {
                    if (n is Milestone && n.isChildOf(node) == false)
                    {
                        (n as Milestone).ForceDeactivate();
                    }
                }
            }
            return (node != null) && node.TryActivate();
        }

        public NarrativeUnit GetNodeByLabel(string label) => plotPointer.PlotNarrativeUnits.SingleOrDefault(n => n.m_name == label);

        public void LoadPlot(string plotLabel, bool reset = true)
        {
            plotPointer = _narrative.Single(p => p.GetPlotLabel() == plotLabel);
            if (reset == true)
            {
                plotPointer.Reset();
            }
        }
    }
    #endregion Model

    #region Plot
    public class Plot
    {
        internal List<NarrativeUnit> PlotNarrativeUnits = new List<NarrativeUnit>();

        internal Plot(string label) => PlotNarrativeUnits.Add(new Root(label));

        Plot AppendNarrativeUnit(string label, string parentLabels, ActivationTypes activationType)
        {
            PlotNarrativeUnits.Add(new NarrativeUnit(label, ParseParents(parentLabels), activationType));
            return this;
        }

        Plot AppendMilestone(string label, string parentLabels, ActivationTypes activationType)
        {

            PlotNarrativeUnits.Add(new Milestone(label, ParseParents(parentLabels), activationType));
            return this;
        }

        public Plot AddNode(string label, string parentLabels = null, ActivationTypes activationType = ActivationTypes.any)
        => AppendNarrativeUnit(label, parentLabels, activationType);

        internal Plot AddMilestone(string label, string parentLabels = null, ActivationTypes activationType = ActivationTypes.any)
        => AppendMilestone(label, parentLabels, activationType);

        internal void Reset()
        {
            foreach (var nu in PlotNarrativeUnits)
            {
                nu.Reset();
            }
        }

        internal string GetPlotLabel() => PlotNarrativeUnits.First().m_name;

        NarrativeUnit[] ParseParents(string parentList)
        {
            var parents = new List<NarrativeUnit>();
            if (parentList == null) parents.Add(PlotNarrativeUnits.First());
            else
            {
                var sParentLabels = parentList.Split(',');
                parents.AddRange(PlotNarrativeUnits.Where(nu => sParentLabels.Contains(nu.m_name)));
            }
            // Mark all parents as non ending.
            foreach (var p in parents)
            {
                p.MakeNonEnding();
            }
            return parents.ToArray();
        }
    }
    #endregion
    
    #region Narrative Units
    public class NarrativeUnit 
    {
        // Unique label of the node
        public readonly string m_name;

        // It is required that a node has one or more paths to the root node through its parents.
        readonly protected NarrativeUnit[] parents;

        readonly protected ActivationTypes activationType;

        protected bool activated = false;

        internal bool Activated() => activated;



        // A node that triggers when the story has finished. Is set automatically and 
        // unset when other node declares this as parent.
        protected bool _isEndingNode = true;
       
        internal bool IsEnding() => _isEndingNode;
        internal void MakeNonEnding() => _isEndingNode = false;

        internal virtual bool IsActivable()
        {
            if (activated) return false;
            switch (activationType)
            {
                case ActivationTypes.any: return parents.Any(n => n.Activated());
                case ActivationTypes.all: return parents.All(n => n.Activated());
            }
            return false;
        }

        internal virtual bool TryActivate()
        {
            if (activated) return false;
            activated = IsActivable();
            return activated;
        }

        internal virtual void Reset() => activated = false;

        public NarrativeUnit(string name, NarrativeUnit[] parents, ActivationTypes activation)
        {
            m_name = name;
            activationType = activation;
            this.parents = parents;
        }

        public bool isChildOf(NarrativeUnit nu)
        {
            if (parents == null) return false;
            if (nu.Equals(this)) return true;
            foreach (var p in parents)
            {
                if (p.parents == null) continue;

                if (nu.Equals(p))
                {
                    return true;
                }
                else
                {
                    if (p.isChildOf(nu)) return true; ;
                }
            }
            return false;
        }

        public override bool Equals(Object obj)
        {
            NarrativeUnit other = obj as NarrativeUnit;
            if (other == null)
                return false;
            else
                return other.m_name == m_name;
        }

        public override int GetHashCode()
        {
            return 1904378486 + EqualityComparer<string>.Default.GetHashCode(m_name);
        }
    }

    public class Root : NarrativeUnit
    {
        public Root(string label) : base(label, null, ActivationTypes.root)
        {
            activated = true;
        }
        internal override void Reset()
        {
            activated = true;
        }
    }

    public class Milestone : NarrativeUnit
    {
        bool forcedDeactivate = false;

        public void ForceDeactivate() => forcedDeactivate = true;

        public Milestone(string label, NarrativeUnit[] parents, ActivationTypes activation) : base(label, parents, activation)
        { 
        }

        internal override bool IsActivable() => !forcedDeactivate&&base.IsActivable();

        internal override void Reset()
        {
            base.Reset();
            forcedDeactivate = false;
        }
    }
    #endregion
}
