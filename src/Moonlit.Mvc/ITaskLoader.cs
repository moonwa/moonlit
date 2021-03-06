﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.Mvc
{
    public interface ITaskLoader
    {
        Tasks LoadTasks();
    }
    public interface IMessageLoader
    {
        Messages LoadMessages();
    }
    public interface INoticeLoader
    {
        Notices Load();
    }
}
