using System;
using System.Linq;

namespace DistributedAgentsSyncPlayground
{
    public class VectorClockSyncResult<T>
    {
        private VectorClockSyncResult(VectorClockNode<T> currentNode, VectorClockNode<T> solution, VectorClockConflict<T> conflict)
        {
            CurrentNode = currentNode;
            Solution = solution;
            Conflict = conflict;
        }

        public VectorClockNode<T> CurrentNode { get; }
        public VectorClockNode<T> Solution { get; private set; }
        public VectorClockConflict<T> Conflict { get; }

        public bool IsSuccessfull => Solution != null;

        public VectorClockSyncResult<T> ResolveConflict(T solution)
        {
            Solution = GenerateSolution(solution);
            return this;
        }

        private VectorClockNode<T> GenerateSolution(T solution)
        {
            var allNodeIds = Conflict.NodeA.Revision.Select(x => x.NodeID).Union(Conflict.NodeB.Revision.Select(x => x.NodeID));

            return new VectorClockNode<T>(
                CurrentNode.NodeID,
                solution,
                allNodeIds.Select(x => new VectorClockNodeVersion(x, Math.Max(Conflict.NodeA.VersionOf(x), Conflict.NodeB.VersionOf(x)))).ToArray()
                );
        }

        public static VectorClockSyncResult<T> Successfull(VectorClockNode<T> currentNode, VectorClockNode<T> solution)
            => new VectorClockSyncResult<T>(currentNode, solution, null);

        public static VectorClockSyncResult<T> Conflictual(VectorClockNode<T> currentNode, VectorClockConflict<T> conflict)
            => new VectorClockSyncResult<T>(currentNode, null, conflict);

        public override string ToString()
        {
            if (IsSuccessfull) return $"Sync OK, winner: {Solution}";

            return $"Sync Conflict: {Conflict.NodeA} vs. {Conflict.NodeB}";
        }
    }
}
