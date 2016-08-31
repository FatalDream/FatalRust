using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FatalRust.Build
{
    public class CargoBuildFactory : ITaskFactory
    {
        string ITaskFactory.FactoryName => "FatalRust.Build.CargoBuildFactory";

        Type ITaskFactory.TaskType => typeof(CargoBuild);

        void ITaskFactory.CleanupTask(ITask task)
        {
        }

        ITask ITaskFactory.CreateTask(IBuildEngine taskFactoryLoggingHost)
        {
            return new CargoBuild();
        }

        TaskPropertyInfo[] ITaskFactory.GetTaskParameters()
        {
            return new TaskPropertyInfo[0];
        }

        bool ITaskFactory.Initialize(string taskName, IDictionary<string, TaskPropertyInfo> parameterGroup, string taskBody, IBuildEngine taskFactoryLoggingHost)
        {
            return true;
        }
    }
}
