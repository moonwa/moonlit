using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Moonlit.Mvc.Maintenance.Domains;
using Newtonsoft.Json;

namespace Moonlit.Mvc.Maintenance.Daemons
{
    public class SystemJobDaemon : IDaemon
    {
        private Thread _workThread;
        private bool _isWorking = false;
        private ILog _log = LogManager.GetLogger("SystemJobDaemon");
        public SystemJobDaemon()
        {
        }
        #region Implementation of IDaemon

        public void Start()
        {
            if (_isWorking)
            {
                throw new InvalidOperationException("SystemJobDaemon already in running");
            }
            _isWorking = true;
            _workThread = new Thread(OnWork);
            _workThread.Start();
        }

        private void OnWork()
        {
            while (_isWorking)
            {
                try
                {
                    using (var db = new MaintDbContext())
                    {
                        foreach (var job in db.SystemJobs.Where(x => x.StartTime < DateTime.Now && x.Status == SystemJobStatus.Init).OrderBy(x => x.StartTime).Take(10).ToList())
                        {
                            try
                            {
                                job.ExecuteTime = DateTime.Now;
                                var handlerType = Type.GetType(job.HandlerType, true, true);
                                ISystemJobHandler handler = (ISystemJobHandler)JsonConvert.DeserializeObject(job.HandlerData, handlerType);
                                job.Result = handler.Execute();
                                job.Status = SystemJobStatus.Success;
                            }
                            catch (Exception ex)
                            {
                                ex = ex.Trim();
                                job.Result = ex.ToString();
                                job.Status = SystemJobStatus.Error;
                            }
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex = ex.Trim();
                    _log.Error(ex);
                }
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            if (!_isWorking)
            {
                throw new InvalidOperationException("SystemJobDaemon not in running");
            }
            _isWorking = false;
        }

        #endregion
    }
}
