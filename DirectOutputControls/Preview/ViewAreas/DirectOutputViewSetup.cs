using DirectOutput.Cab;
using DirectOutput.General.Generic;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DirectOutputControls
{
    [Serializable]
    public class DirectOutputViewSetup 
    {
        public ExtList<DirectOutputViewArea> ViewAreas { get; private set; } = new ExtList<DirectOutputViewArea>();

        private Dictionary<DofConfigToolOutputEnum, List<DirectOutputViewAreaUpdatable>> ViewAreasDictionary = new Dictionary<DofConfigToolOutputEnum, List<DirectOutputViewAreaUpdatable>>();
        private List<DirectOutputViewArea> AllAreas = new List<DirectOutputViewArea>();

        public DirectOutputViewSetup()
        {
            ViewAreas.AfterInsert += ViewAreas_AfterInsert;
            ViewAreas.AfterRemove += ViewAreas_AfterRemove;
            ViewAreas.BeforeClear += ViewAreas_BeforeClear;
            ViewAreas.AfterSet += ViewAreas_AfterSet;
        }

        private void ViewAreas_BeforeClear(object sender, EventArgs e)
        {
        }

        private void ViewAreas_AfterSet(object sender, SetEventArgs<DirectOutputViewArea> e)
        {
            Init();
        }

        private void ViewAreas_AfterRemove(object sender, RemoveEventArgs<DirectOutputViewArea> e)
        {
            OnAreasRemoved(e.Item.GetAllAreas());
        }

        private void ViewAreas_AfterInsert(object sender, InsertEventArgs<DirectOutputViewArea> e)
        {
            OnAreasInserted(e.Item.GetAllAreas());
        }

        private void Children_AfterSet(object sender, SetEventArgs<DirectOutputViewArea> e)
        {
            OnAreasRemoved(e.OldItem.GetAllAreas());
            OnAreasInserted(e.NewItem.GetAllAreas());
        }

        internal void Cleanup()
        {
            foreach(var area in ViewAreas) {
                area.Cleanup();
            }
        }

        private void Children_AfterRemove(object sender, RemoveEventArgs<DirectOutputViewArea> e)
        {
            OnAreasRemoved(e.Item.GetAllAreas());
        }

        private void Children_AfterInsert(object sender, InsertEventArgs<DirectOutputViewArea> e)
        {
            OnAreasInserted(e.Item.GetAllAreas());
        }

        private void Children_BeforeClear(object sender, EventArgs e)
        {
            var children = sender as ExtList<DirectOutputViewArea>;
            foreach(var child in children) {
                RemoveChildrenCallbacks(child);
            }
        }

        private void AddChildrenCallbacks(DirectOutputViewArea area)
        {
            if (area != null) {
                area.Children.BeforeClear += Children_BeforeClear;
                area.Children.AfterInsert += Children_AfterInsert;
                area.Children.AfterRemove += Children_AfterRemove;
                area.Children.AfterSet += Children_AfterSet;
            }
        }

        private void RemoveChildrenCallbacks(DirectOutputViewArea area)
        {
            if (area != null) {
                area.Children.BeforeClear -= Children_BeforeClear;
                area.Children.AfterInsert -= Children_AfterInsert;
                area.Children.AfterRemove -= Children_AfterRemove;
                area.Children.AfterSet -= Children_AfterSet;
            }
        }

        private void OnAreasInserted(IEnumerable<DirectOutputViewArea> areas)
        {
            AllAreas.AddRange(areas);
            AssignToDictionary(areas.Where(A=>A is DirectOutputViewAreaUpdatable).Cast<DirectOutputViewAreaUpdatable>());
            foreach (var area in areas) {
                AddChildrenCallbacks(area);
            }
        }
        private void OnAreasRemoved(IEnumerable<DirectOutputViewArea> areas)
        {
            AllAreas.RemoveAll(A=>areas.Contains(A));
            UnassignFromDictionary(areas.Where(A => A is DirectOutputViewAreaUpdatable).Cast<DirectOutputViewAreaUpdatable>());
            foreach (var area in areas) {
                RemoveChildrenCallbacks(area);
            }
        }

        private void AssignToDictionary(IEnumerable<DirectOutputViewAreaUpdatable> areas)
        {
            foreach (var area in areas) {
                foreach(var output in area.DofOutputs) {
                    if (!ViewAreasDictionary.Keys.Contains(output)) {
                        ViewAreasDictionary[output] = new List<DirectOutputViewAreaUpdatable>();
                    }
                    ViewAreasDictionary[output].Add(area);
                }
            }
        }

        internal bool RemoveArea(DirectOutputViewArea area)
        {
            if (HasArea(area)) {
                OnAreasRemoved(area.GetAllAreas());
                if (ViewAreas.Contains(area)) {
                    ViewAreas.Remove(area);
                } else {
                    foreach (var a in ViewAreas) {
                        a.RemoveArea(area);
                    }
                }
                return true;
            }
            return false;
        }

        private void UnassignFromDictionary(IEnumerable<DirectOutputViewAreaUpdatable> areas)
        {
            foreach (var area in areas) {
                foreach (var output in area.DofOutputs) {
                    if (ViewAreasDictionary.Keys.Contains(output)) {
                        ViewAreasDictionary[output].RemoveAll(A => areas.Contains(A));
                    }
                }
            }
        }
        
        private void SetParent(DirectOutputViewArea Parent, DirectOutputViewArea Child)
        {
            Child.Parent = Parent;
        }

        public void Init()
        {
            AllAreas.Clear();
            ViewAreasDictionary.Clear();
            ParseHierarchy(SetParent);
            RectangleF baseRect = RectangleF.FromLTRB(0.0f, 0.0f, 1.0f, 1.0f);
            foreach (var area in ViewAreas) {
                area.ComputeGlobalDimensions(baseRect);
                OnAreasInserted(area.GetAllAreas());
            }
        }

        public void Init(DirectOutputViewSetup other)
        {
            AllAreas.Clear();
            ViewAreasDictionary.Clear();
            ViewAreas.Clear();
            ViewAreas.AddRange(other.ViewAreas);
            ParseHierarchy(SetParent);
            RectangleF baseRect = RectangleF.FromLTRB(0.0f, 0.0f, 1.0f, 1.0f);
            foreach (var area in ViewAreas) {
                area.ComputeGlobalDimensions(baseRect);
            }
        }

        public void ComputeAreaDimensions()
        {
            RectangleF baseRect = RectangleF.FromLTRB(0.0f, 0.0f, 1.0f, 1.0f);
            foreach (var area in ViewAreas) {
                area.ComputeGlobalDimensions(baseRect);
            }
        }

        public void Resize(RectangleF bounds)
        {
            foreach(var area in ViewAreas) {
                area.Resize(bounds);
            }
        }

        public bool HasArea(DirectOutputViewArea area) => AllAreas.Any(A => A == area);

        public void DisplayAreas(Graphics gr, Font f, SolidBrush br, Pen p)
        {
            foreach (var area in ViewAreas) {
                area.DisplayArea(gr, f, br, p);
            }
        }

        public void Display(Graphics gr, Font f, SolidBrush br)
        {
            foreach (var area in ViewAreas) {
                area.Display(gr, f, br);
            }
        }

        internal T[] GetViewAreas<T>(Func<DirectOutputViewArea, bool> Match = null, bool enabledOnly = true)
        {
            var areas = AllAreas.Where(V => (V is T) && (!enabledOnly || V.Enabled));
            if (Match != null) {
                areas = areas.Where(Match);
            }
            return areas.Cast<T>().ToArray();
        }

        internal DirectOutputViewArea[] HitTest(Point coords)
        {
            if (coords == null) return new DirectOutputViewArea[0];

            return AllAreas.Where(A => A.HitTest(coords)).ToArray();
        }

        internal string FindUniqueAreaName(string prefix)
        {
            int count = 0;
            var newName = $"{prefix} {count}";
            while (AllAreas.Any(A=>A.Name.Equals(newName, StringComparison.InvariantCultureIgnoreCase))){
                count++;
                newName = $"{prefix} {count}";
            }
            return newName;
        }

        public delegate void HierarchyItemFunc(DirectOutputViewArea Parent, DirectOutputViewArea Child);

        private static void ParseHierarchy(DirectOutputViewArea Parent, HierarchyItemFunc HierarchyFunc)
        {
            foreach(var child in Parent.Children) {
                HierarchyFunc.Invoke(Parent, child);
                ParseHierarchy(child, HierarchyFunc);
            }
        }

        public void ParseHierarchy(HierarchyItemFunc HierarchyFunc)
        {
            foreach(var area in ViewAreas) {
                HierarchyFunc.Invoke(null, area);
                ParseHierarchy(area, HierarchyFunc);
            }
        }
    }
}
