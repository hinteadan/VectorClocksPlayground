using System;
using System.Collections.Generic;
using System.Linq;

namespace H.VectorClocks
{
    public class VectorClockNode<T>
    {
        #region Construct
        private readonly Dictionary<string, VectorClockNodeVersion> revision;
        private readonly VectorClockNodeVersion selfVersion;

        public VectorClockNode(string nodeId, T payload, params VectorClockNodeVersion[] revision)
        {
            NodeID = nodeId ?? Guid.NewGuid().ToString();
            Payload = payload;
            this.revision = revision.ToDictionary(x => x.NodeID, x => x);
            if (!this.revision.ContainsKey(NodeID))
            {
                selfVersion = new VectorClockNodeVersion(NodeID, 0);
                this.revision.Add(NodeID, selfVersion);
            }
            selfVersion = selfVersion ?? this.revision[NodeID];
        }

        public VectorClockNode(string nodeId)
            : this(nodeId, default(T))
        { }
        #endregion


        public T Payload { get; private set; }
        public string NodeID { get; }
        public long Version => selfVersion.Version;
        public long VersionOf(string nodeId) => !revision.ContainsKey(nodeId) ? 0 : revision[nodeId].Version;
        public VectorClockNodeVersion[] Revision => revision.Values.ToArray();


        public virtual VectorClockSyncResult<T> Acknowledge(VectorClockNode<T> vectorClock)
        {
            vectorClock = NormalizeVectors(vectorClock);

            VectorClockNode<T> winner =
                this.IsDescendantOf(vectorClock) ? this :
                vectorClock.IsDescendantOf(this) ? vectorClock :
                null;

            if (winner != null)
            {
                Payload = winner.Payload;

                foreach (var version in Revision)
                {
                    while (version.Version < winner.VersionOf(version.NodeID)) version.Increment();
                }

                return VectorClockSyncResult<T>.Successfull(this, winner);
            }

            return VectorClockSyncResult<T>.Conflictual(this, new VectorClockConflict<T>(this, vectorClock));
        }
        public virtual VectorClockNode<T> Say(T payload)
        {
            Payload = payload;
            TrackEvent();
            return this;
        }


        private void TrackEvent() => selfVersion.Increment();
        private bool IsDescendantOf(VectorClockNode<T> otherVectorClock)
        {
            return otherVectorClock.Revision.All(a => this.revision[a.NodeID].Version >= a.Version);
        }
        private VectorClockNode<T> NormalizeVectors(VectorClockNode<T> vectorClock)
        {
            var newNodesForSelf = vectorClock.revision.Where(c => !this.revision.ContainsKey(c.Key)).Select(c => c.Value);
            var newNodesForOther = this.revision.Where(c => !vectorClock.revision.ContainsKey(c.Key)).Select(c => c.Value);
            foreach (var version in newNodesForSelf) this.revision.Add(version.NodeID, new VectorClockNodeVersion(version.NodeID, 0));
            foreach (var version in newNodesForOther) vectorClock.revision.Add(version.NodeID, new VectorClockNodeVersion(version.NodeID, 0));
            return vectorClock;
        }

        public override string ToString()
        {
            return $"{NodeID} ({string.Join(",", revision.Values.Select(v => v.Version))})";
        }
    }
}
