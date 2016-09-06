using Bearded.Monads;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace FatalRust
{
    [Export(typeof(IProjectConfigurationsService))]
    [AppliesTo("ProjectConfigurationsFromRustupToolchains")]
    internal class ProjectToolchainConfigurationService : IProjectConfigurationsServiceInternal
    {
        [Import(AllowDefault = true)]
        public Lazy<IActiveConfiguredProjectProvider> ActiveConfiguredProjectProvider { get; set; }

        private int number = 0;
        List<ProjectConfiguration> Configurations;

        public ProjectToolchainConfigurationService()
        {
            Console.WriteLine("created service!");
        }

        ProjectConfiguration IProjectConfigurationsService.SuggestedProjectConfiguration
        {
            get { return InternalGetSuggested(true); }
        }
        
        ProjectConfiguration IProjectConfigurationsServiceInternal.GetSuggestedProjectConfiguration(bool queryActiveConfiguration)
        {
            return InternalGetSuggested(queryActiveConfiguration);
        }

        ProjectConfiguration InternalGetSuggested(bool queryActiveConfiguration)
        {
            return ActiveConfiguredProjectProvider.AsOption()
                .SelectMany(provider => provider.Value.ActiveConfiguredProject.AsOption())
                .Select(project => project.ProjectConfiguration)
                .Where(_ => queryActiveConfiguration)

                .Else(() =>
                {
                    UpdateConfigurations();
                    return Configurations[0];
                });
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
            UpdateConfigurations();
            
            return Task.FromResult((IImmutableSet<ProjectConfiguration>)Configurations.ToImmutableHashSet());
        }

        void UpdateConfigurations()
        {

            Configurations = ProjectToolchainConfigurationHelper.GetConfigurations()
                                    .Select(list => list.Select(toolchain => ProjectToolchainConfigurationHelper.MakeConfiguration("Debug", toolchain)))
                                    .Unify(
                                        list => list.ToList(),
                                        //(IImmutableSet<ProjectConfiguration>)(new ProjectConfiguration[0]).ToImmutableHashSet(),
                                        error => { throw new Exception(error.ToString()); });
        }

        Task<ProjectConfiguration> IProjectConfigurationsService.GetProjectConfigurationAsync(string name)
        {
            try
            {
                var conf = Configurations.Where(config => config.Name == name).First();
                return Task.FromResult(conf);
            }
            catch (Exception e)
            {
                throw new System.Collections.Generic.KeyNotFoundException(e.Message);
            }
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
