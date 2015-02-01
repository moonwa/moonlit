namespace Moonlit.Diagnostics
{
    /// <summary>
    /// 性能测试类
    /// </summary>
    public class Profiler
    {
        private System.Timers.Timer _timer = new System.Timers.Timer(1000);
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get { return _timer.Enabled; } }
        private int _currentFps = 0;

        /// <summary>
        /// 每秒侦数
        /// </summary>
        public int Fps { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Profiler()
        {
            _timer.Elapsed += OnTimer;
        }

        void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (this)
            {
                Fps = _currentFps;
                _currentFps = 0;
            }
        }
        /// <summary>
        /// 开始记时
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
            _currentFps = 0;
        }
        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="fps"></param>
        public void Record(int fps)
        {
            lock (this)
            {
                _currentFps += fps;
            }
        }
    }
}