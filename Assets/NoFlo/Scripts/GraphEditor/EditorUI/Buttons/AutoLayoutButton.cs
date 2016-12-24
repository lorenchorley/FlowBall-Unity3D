using NoFlo_Basic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoFloEditor {

    public class AutoLayoutButton : MonoBehaviour {

        public Graph Graph;
        public GraphEditor GraphEditor;
        Button Button;

        void Start() {
            Button = GetComponent<Button>();
            Button.onClick.AddListener(() => Layout());
        }

        public void SetGraph(Graph Graph) {
            this.Graph = Graph;
        }



        private class profile {
            public NoFlo_Basic.Component Component;
            public int? rank;
            public int checkedLevel = 0;
            public List<profile> forward;
            public List<profile> backward;

            public bool Checked(int currentLevel) {
                if (checkedLevel == currentLevel) {
                    return true;
                } else {
                    checkedLevel = currentLevel; // Mark as checked
                    return false;
                }
            }
        }

        List<profile> profiles;
        Dictionary<NoFlo_Basic.Component, profile> profilesByNode;
        public void Layout() {
            if (Graph == null)
                return;

            profiles = new List<profile>();
            profilesByNode = new Dictionary<NoFlo_Basic.Component, profile>();

            foreach (NoFlo_Basic.Component c in Graph.NodesByName.Values) {
                profile p = new profile() {
                    Component = c,
                };
                profilesByNode.Add(c, p);
                profiles.Add(p);
            }

            for (int i = 0; i < profiles.Count; i++) {
                profile p = profiles[i];
                p.forward = ForwardConnectedNodes(p.Component);
                p.backward = BackwardConnectedNodes(p.Component);
            }

            // 0 or 1, already done
            if (profiles.Count <= 1)
                return;

            int currentCheckedLevel = 1;

            // Check each profile only if they haven't been checked yet
            for (int i = 0; i < profiles.Count; i++) {
                profile p = profiles[i];

                if (p.Checked(currentCheckedLevel))
                    continue;

                // Set initial rank
                p.rank = 0;

                // Traverse the network, checking each profile.
                // Each forward connections adds one to the rank, each backward substracts one
                Traverse(p, currentCheckedLevel, true, (isForward, l, h) => {
                    h.rank = l.rank.Value + (isForward ? 1 : -1);
                });

            }

            currentCheckedLevel = 2;
            float hsep = GraphEditor.Layout.HorizontalSeparation;
            float vsep = GraphEditor.Layout.VerticalSeparation;
            List<int> OrderedRanks;
            Dictionary<int, List<profile>> profilesByRank = OrderProfilesByRank(out OrderedRanks);
            Dictionary<int, int> NumberOfNodesGraphedByRank = new Dictionary<int, int>();
            Func<profile, int, float> xPos = (p, o) => (p.rank.Value + o) * hsep;
            Func<float, int, float> yPos = (y, n) => y + n * vsep;

            foreach (int r in profilesByRank.Keys)
                NumberOfNodesGraphedByRank.Add(r, 0);

            // Position according to rank, for each sub graph
            for (int i = 0; i < OrderedRanks.Count; i++) {
                int rank = OrderedRanks[i];
                List<profile> profileOfRank = profilesByRank[rank];

                int rankOffset;
                if (rank < 0)
                    rankOffset = -rank;
                else
                    rankOffset = 0;

                for (int j = 0; j < profileOfRank.Count; j++) {
                    float currentY = rank * vsep; // TODO include full size of previous graph
                    profile p = profileOfRank[j];

                    if (p.Checked(currentCheckedLevel))
                        continue;

                    p.Component.MetadataPosition = new Vector3(xPos(p, rankOffset), yPos(currentY, 0));

                    Traverse(p, currentCheckedLevel, true, (isForward, l, h) => {
                        int number = NumberOfNodesGraphedByRank[h.rank.Value];
                        NumberOfNodesGraphedByRank[h.rank.Value] = number + 1;

                        h.Component.MetadataPosition = new Vector3(xPos(h, rankOffset), yPos(currentY, number));
                    });

                }

            }

            // Center the whole graph at the local zero for the panel
            GraphEditor.CenterGraph();

        }

        private void Traverse(profile p, int currentCheckedLevel, bool depthFirst, Action<bool, profile, profile> callback) {
            List<profile> uncheckedProfiles = null;
            if (depthFirst)
                uncheckedProfiles = new List<profile>();

            for (int i = 0; i < p.forward.Count; i++) {
                profile f = p.forward[i];

                if (f.Checked(currentCheckedLevel))
                    continue;

                callback.Invoke(true, p, f);

                if (depthFirst)
                    uncheckedProfiles.Add(f);
                else
                    Traverse(f, currentCheckedLevel, false, callback);

            }

            for (int i = 0; i < p.backward.Count; i++) {
                profile b = p.backward[i];

                if (b.Checked(currentCheckedLevel))
                    continue;

                callback.Invoke(false, p, b);

                if (depthFirst)
                    uncheckedProfiles.Add(b);
                else
                    Traverse(b, currentCheckedLevel, false, callback);

            }

            if (depthFirst)
                for (int i = 0; i < uncheckedProfiles.Count; i++)
                    Traverse(uncheckedProfiles[i], currentCheckedLevel, true, callback);
        }

        private Dictionary<int, List<profile>> OrderProfilesByRank(out List<int> OrderedInduces) {
            Dictionary<int, List<profile>> profilesByRank = new Dictionary<int, List<profile>>();

            List<profile> list;
            for (int i = 0; i < profiles.Count; i++) {
                profile p = profiles[i];

                if (!profilesByRank.TryGetValue(p.rank.Value, out list)) {
                    list = new List<profile>();
                    profilesByRank.Add(p.rank.Value, list);
                }

                list.Add(p);
            }

            OrderedInduces = new List<int>(profilesByRank.Keys);
            OrderedInduces.Sort();

            //for (int i = 0; i < OrderedInduces.Count; i++) {
            //    int index = OrderedInduces[i];
            //    yield return profilesByRank[index];
            //}

            return profilesByRank;
        }

        private List<profile> ForwardConnectedNodes(NoFlo_Basic.Component n) {
            List<profile> list = new List<profile>();
            foreach (OutPort p in n.Output.GetPorts()) {
                foreach (Edge e in p.Edges()) {
                    list.Add(profilesByNode[e.Target.Component]);
                }
            }
            return list;
        }

        private List<profile> BackwardConnectedNodes(NoFlo_Basic.Component n) {
            List<profile> list = new List<profile>();
            foreach (InPort p in n.Input.GetPorts()) {
                foreach (Edge e in p.Edges()) {
                    list.Add(profilesByNode[e.Source.Component]);
                }
            }
            return list;
        }

    }

}