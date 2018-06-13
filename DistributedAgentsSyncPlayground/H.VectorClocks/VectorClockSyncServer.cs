using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace H.VectorClocks
{
    public class VectorClockSyncServer<T>
    {
        #region Construct
        private bool isRunning = false;
        private readonly ConcurrentQueue<VectorClockNode<T>> syncRequestQueue = new ConcurrentQueue<VectorClockNode<T>>();
        private readonly ConcurrentDictionary<string, VectorClockNode<T>> nodeMesh = new ConcurrentDictionary<string, VectorClockNode<T>>();
        private readonly CancellationTokenSource chewTaskCancellation = new CancellationTokenSource();

        public VectorClockNode<T> Head { get; private set; } = null;

        private readonly ImAVectorClockConflictResolver<T> conflictResolver;

        public VectorClockSyncServer(ImAVectorClockConflictResolver<T> conflictResolver)
        {
            this.conflictResolver = conflictResolver;
        }

        public VectorClockSyncServer()
            : this(new ConflictMediators.SuperDumbConflictMediator<T>((a, b) => b))
        { }
        #endregion

        public virtual void Start()
        {
            if (isRunning) return;

            ChewRequestQueueAndTriggerAnotherChew();
            isRunning = true;
        }

        public virtual void Stop()
        {
            chewTaskCancellation.Cancel();
        }

        public virtual bool TryRegisterNode(VectorClockNode<T> node)
        {
            return nodeMesh.TryAdd(node.NodeID, node);
        }

        public virtual void QueueEvent(VectorClockNode<T> eventSource)
        {
            syncRequestQueue.Enqueue(eventSource);
        }

        private void ChewRequestQueue()
        {
            VectorClockNode<T> queueHead;
            if (!syncRequestQueue.TryDequeue(out queueHead)) return;
            Head = Head ?? queueHead;

            VectorClockSyncResult<T> syncResult = Head.Acknowledge(queueHead);

            if (syncResult.IsSuccessfull)
            {
                Head = syncResult.Solution;
                TryToAknowledgeOthers(Head);
                return;
            }

            syncResult = conflictResolver.ResolveConflict(syncResult);

            if (syncResult.IsSuccessfull)
            {
                Head = syncResult.Solution;
                TryToAknowledgeOthers(Head);
                return;
            }

            //Delegate conflict to node
            nodeMesh[queueHead.NodeID].Acknowledge(Head);
            //TryToAknowledgeOthers(Head);
        }

        private void ChewRequestQueueAndTriggerAnotherChew()
        {
            if (chewTaskCancellation.IsCancellationRequested)
            {
                chewTaskCancellation.Dispose();
                isRunning = false;
                return;
            }
            Task.Run(() => ChewRequestQueue(), chewTaskCancellation.Token).ContinueWith(x => ChewRequestQueueAndTriggerAnotherChew(), chewTaskCancellation.Token);
        }

        private void TryToAknowledgeOthers(VectorClockNode<T> head)
        {
            foreach (var node in nodeMesh)
            {
                node.Value.Acknowledge(head);
            }
        }

        public override string ToString()
        {
            if (!nodeMesh.Any()) return "[Zero Nodes]";
            return string.Join(" + ", nodeMesh.Select(x => x.Value.ToString()));
        }
    }
}
