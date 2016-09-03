using System;
using System.Linq;
using System.Collections.Immutable;
using System.Windows;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ProjectSystem.Designers;
using Microsoft.VisualStudio.ProjectSystem.Utilities;
using System.ComponentModel.Composition;
using FatalRust.BuildSystem.Control;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.ProjectSystem;
using Bearded.Monads;

namespace FatalRust
{
    [Export(typeof(IProjectConfigurationsService))]
    [AppliesTo("ProjectConfigurationsFromRustupToolchains")]
    internal class ProjectToolchainConfigurationService : IProjectConfigurationsServiceInternal
    {
        public ProjectToolchainConfigurationService()
        {
            Console.WriteLine("created service!");
        }

        ProjectConfiguration IProjectConfigurationsService.SuggestedProjectConfiguration
        {
            get { return InternalGetSuggested(); }
        }
        
        ProjectConfiguration IProjectConfigurationsServiceInternal.GetSuggestedProjectConfiguration(bool queryActiveConfiguration)
        {
            return InternalGetSuggested();
        }

        ProjectConfiguration InternalGetSuggested()
        {
            return ProjectToolchainConfigurationHelper.GetConfigurations().Unify(
                    list => list[0],
                    error => { throw new Exception(error.ToString()); });
        }


        public event AsyncEventHandler<ProjectConfigurationChangeEventArgs> Added;
        public event AsyncEventHandler<ProjectConfigurationChangeEventArgs> Changed;
        public event AsyncEventHandler<ProjectConfigurationChangeEventArgs> Removed;

        Task IProjectConfigurationsService.AddKnownProjectConfigurationAsync(string name, IImmutableDictionary<string, string> dimensionNameValuePairs)
        {
            throw new NotImplementedException();
        }

        Task IProjectConfigurationsService.AddProjectConfigurationsByExistingDimensionAsync(string dimensionName, string dimensionValue, Func<IImmutableDictionary<string, string>, string> getConfigurationName)
        {
            throw new NotImplementedException();
        }

        Task IProjectConfigurationsService.CloneKnownProjectConfigurationAsync(string templateName, string cloneName)
        {
            throw new NotImplementedException();
        }

        Task IProjectConfigurationsService.CloneKnownProjectConfigurationAsync(string templateName, string cloneName, IImmutableDictionary<string, string> cloneMetadata)
        {
            throw new NotImplementedException();
        }

        Task IProjectConfigurationsService.CloneProjectConfigurationDimensionAsync(string dimensionName, string oldValue, string newValue)
        {
            throw new NotImplementedException();
        }

        Task<IImmutableSet<ProjectConfiguration>> IProjectConfigurationsService.GetKnownProjectConfigurationsAsync()
        {
            return Task.Run(() => ProjectToolchainConfigurationHelper.GetConfigurations()
                                    .Unify(
                                        list => (IImmutableSet<ProjectConfiguration>) list.ToImmutableHashSet(),
                                        error => { throw new Exception(error.ToString()); }));
        }

        Task<ProjectConfiguration> IProjectConfigurationsService.GetProjectConfigurationAsync(string name)
        {
            return Task.Run(() => ProjectToolchainConfigurationHelper.GetConfigurations()
                                    .Select(
                                        list => list.Where(conf => conf.Name == name).First())
                                    .Unify(
                                        conf => conf,
                                        error => { throw new Exception(error.ToString()); }));
        }

        Task IProjectConfigurationsService.RemoveKnownProjectConfigurationAsync(string name, bool removeConditionedElements)
        {
            throw new NotImplementedException();
        }

        Task IProjectConfigurationsService.RemoveProjectConfigurationDimensionAsync(string dimensionName, string dimensionValue, bool removeConditionedElements)
        {
            throw new NotImplementedException();
        }

        Task IProjectConfigurationsService.RenameProjectConfigurationDimensionAsync(string dimensionName, string oldValue, string newValue, bool renameConditionedElements)
        {
            throw new NotImplementedException();
        }
    }
}
