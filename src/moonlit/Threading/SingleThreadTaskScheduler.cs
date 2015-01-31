using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlit.Threading
{
    public class SingleThreadTaskScheduler : TaskScheduler
    {
        private static SingleThreadTaskScheduler _current;
        private static readonly object Locker = new object();
        private readonly Queue<Task> _tasks = new Queue<Task>();

        private readonly Thread _workThread;

        private bool _running = true;
        public SingleThreadTaskScheduler()
        {
            _workThread = new Thread(OnWork);
            _workThread.Start();
        }

        public new static SingleThreadTaskScheduler Current
        {
            get
            {
                lock (Locker)
                {
                    return _current ?? (_current = new SingleThreadTaskScheduler());
                }
            }
        }

        private void OnWork()
        {
            while (_running)
            {
                Task task = DequeueTask();
                if (task != null)
                {
                    TryExecuteTask(task);
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }

        private Task DequeueTask()
        {
            Task task;
            lock (_tasks)
            {
                task = _tasks.Count == 0 ? null : _tasks.Dequeue();
            }
            return task;
        }

        protected override void QueueTask(Task task)
        {
            lock (_tasks)
            {
                _tasks.Enqueue(task);
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }


        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_tasks)
            {
                return _tasks.ToArray();
            }
        }

        public void Stop()
        {
            _running = false;
        }
    }
}