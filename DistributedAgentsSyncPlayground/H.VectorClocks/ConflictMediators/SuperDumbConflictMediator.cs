using System;

namespace H.VectorClocks.ConflictMediators
{
    public class SuperDumbConflictMediator<T> : ImAVectorClockConflictResolver<T>
    {
        private readonly Func<T, T, T> mediator;

        public SuperDumbConflictMediator(Func<T, T, T> mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public VectorClockSyncResult<T> ResolveConflict(VectorClockSyncResult<T> conflict)
        {
            if (conflict.IsSuccessfull) throw new InvalidOperationException($"{nameof(conflict)} is already resolved");

            return conflict.ResolveConflict(mediator.Invoke(conflict.Conflict.NodeA.Payload, conflict.Conflict.NodeB.Payload));
        }
    }
}
