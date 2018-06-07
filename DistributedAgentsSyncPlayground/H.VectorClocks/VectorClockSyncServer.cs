using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace H.VectorClocks
{
    public class VectorClockSyncServer<T>
    {
        private readonly ImAVectorClockConflictResolver<T> conflictResolver = new ConflictMediators.GenericConflictMediator<T>((a, b) => b);
        private readonly ConcurrentQueue<VectorClockNode<T>> syncRequestQueue = new ConcurrentQueue<VectorClockNode<T>>();
        private readonly ConcurrentDictionary<string, VectorClockNode<T>> nodeMesh = new ConcurrentDictionary<string, VectorClockNode<T>>();
        private VectorClockNode<T> head = null;
        private readonly CancellationTokenSource chewTaskCancellation = new CancellationTokenSource();

        public void Start()
        {
            ChewRequestQueueAndTriggerAnotherChew();
        }

        public void Stop()
        {
            chewTaskCancellation.Cancel();
        }

        public bool TryRegisterNode(VectorClockNode<T> node)
        {
            return nodeMesh.TryAdd(node.NodeID, node);
        }

        public void QueueEvent(VectorClockNode<T> eventSource)
        {
            syncRequestQueue.Enqueue(eventSource);
        }

        private void ChewRequestQueue()
        {
            VectorClockNode<T> queueHead;
            if (!syncRequestQueue.TryDequeue(out queueHead)) return;
            head = head ?? queueHead;

            VectorClockSyncResult<T> syncResult = head.Acknowledge(queueHead);

            if (syncResult.IsSuccessfull)
            {
                head = syncResult.Solution;
                TryToAknowledgeOthers(head);
                return;
            }

            syncResult = conflictResolver.ResolveConflict(syncResult);

            if (syncResult.IsSuccessfull)
            {
                head = syncResult.Solution;
                TryToAknowledgeOthers(head);
                return;
            }

            //Delegate conflict to node
            queueHead.Acknowledge(head);
        }

        private void ChewRequestQueueAndTriggerAnotherChew()
        {
            if (chewTaskCancellation.IsCancellationRequested)
            {
                chewTaskCancellation.Dispose();
                return;
            }
            Task.Run(() => ChewRequestQueue(), chewTaskCancellation.Token).ContinueWith(x => ChewRequestQueueAndTriggerAnotherChew(), chewTaskCancellation.Token);
        }

        private void TryToAknowledgeOthers(VectorClockNode<T> head)
        {
            foreach (var node in nodeMesh.Where(x => x.Key != head.NodeID))
            {
                node.Value.Acknowledge(head);
            }
        }
    }
}
