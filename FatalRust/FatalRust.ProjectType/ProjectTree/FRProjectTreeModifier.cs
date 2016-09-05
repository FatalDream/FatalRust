using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Reflection;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Utilities;
using Microsoft.VisualStudio.ProjectSystem.Designers;
using Microsoft.VisualStudio.ProjectSystem.Utilities.Designers;

namespace FatalRust.ProjectTree
{
    [Export(typeof(IProjectTreeModifier))]
    [AppliesTo(MyUnconfiguredProject.UniqueCapability)]
    // TODO: Consider removing the OrderPrecendence attribute as it typically should not be needed when creating a new project type. It may be needed when customizing an existing project type to override the default behavior (e.g. the default C# implementation).
    [OrderPrecedence(1000)]
    internal class FRProjectTreeModifier : IProjectTreeModifier
    {
        public FRProjectTreeModifier()
        {
            Console.WriteLine("what?");
        }

        public IProjectTree ApplyModifications(IProjectTree tree, IProjectTreeProvider projectTreeProvider)
        {
            // Only set the icon for the root project node.  We could choose to set different icons for nodes based
            // on various criteria, not just Capabilities, if we wished.
            if (tree.Capabilities.Contains(ProjectTreeCapabilities.ProjectRoot))
            {
                tree = tree.SetIcon(KnownMonikers.JSProjectNode.ToProjectSystemType());
            }

            return tree;
        }
    }
}