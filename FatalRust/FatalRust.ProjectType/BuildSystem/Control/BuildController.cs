using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;

namespace FatalRust.BuildSystem.Control
{
    [Export(typeof(IBuildController))]
    [AppliesTo("FatalRust")]
    public class BuildController : IBuildController
    {

        [Import]
        IProjectLockService ProjectLockService;

        [Import]
        UnconfiguredProject UncofiguredProject;
        

        async Task<String> IBuildController.Start()
        {
            var configuredProject = await UncofiguredProject.GetSuggestedConfiguredProjectAsync();

            using (var access = await ProjectLockService.WriteLockAsync())
            {
                Microsoft.Build.Evaluation.Project project = await access.GetProjectAsync(configuredProject);

                // party on it, respecting the type of lock you've acquired. 

                // If you're going to change the project in any way, 
                // check it out from SCC first:
                await access.CheckoutAsync(configuredProject.UnconfiguredProject.FullPath);

                String dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
                project.SetGlobalProperty("ExtensionPath", dllPath);
                project.Build(new OutputPaneLogger());

                await access.ReleaseAsync();
            }

            return "bla";
        }

        String IBuildController.Respond()
        {
            return "bla";
        }
    }
}
