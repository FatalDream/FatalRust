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
        private TaskPropertyInfo[] _parameters;

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
            _parameters = _parameters ?? typeof(CargoBuild)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(prop =>
                    new TaskPropertyInfo(
                        prop.Name,
                        prop.PropertyType,
                        prop.GetCustomAttributes().OfType<OutputAttribute>().Any(),
                        prop.GetCustomAttributes().OfType<RequiredAttribute>().Any()))
                .ToArray();
            return _parameters;
        }

        bool ITaskFactory.Initialize(string taskName, IDictionary<string, TaskPropertyInfo> parameterGroup, string taskBody, IBuildEngine taskFactoryLoggingHost)
        {
            return true;
        }
    }
}
