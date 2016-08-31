using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio;

namespace FatalRust.BuildSystem.Control
{
    public class OutputPaneLogger : Microsoft.Build.Utilities.Logger
    {
        private int warnings = 0;
        private int errors = 0;
        

        public override void Initialize(IEventSource eventSource)
        {
            eventSource.BuildStarted += (s, e) => WriteMessage(e.Message);
            eventSource.BuildFinished += (s, e) => WriteMessage(e.Message);
            eventSource.AnyEventRaised += (s, e) => WriteMessage("some event: " + e.Message);
            eventSource.WarningRaised += (s, e) => ++warnings;
            eventSource.ErrorRaised += (s, e) => ++errors;
            eventSource.BuildFinished += (s, e) =>
            {
                Console.WriteLine(errors == 0 ? "Build succeeded." : "Build failed.");
                Console.WriteLine(String.Format("{0} Warning(s)", warnings));
                Console.WriteLine(String.Format("{0} Error(s)", errors));
            };

        }

        private void WriteMessage(String msg)
        {
            var joinableTaskFactoryInstance = ThreadHelper.JoinableTaskFactory;

            joinableTaskFactoryInstance.Run(async delegate
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
                Guid generalPaneGuid = VSConstants.GUID_OutWindowDebugPane; // P.S. There's also the GUID_OutWindowDebugPane available.
                IVsOutputWindowPane generalPane;
                outWindow.GetPane(ref generalPaneGuid, out generalPane);

                generalPane.OutputString(msg + "\n");
                generalPane.Activate(); // Brings this pane into view
            });
        }
    }
}
