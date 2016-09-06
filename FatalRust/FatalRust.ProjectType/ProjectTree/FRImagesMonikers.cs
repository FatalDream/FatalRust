using System;
using Microsoft.VisualStudio.Imaging.Interop;

namespace FatalRust.ProjectTree
{
    public static class FRImagesMonikers
    {
        private static readonly Guid ManifestGuid = new Guid("f8e0123c-a167-406b-a0e7-944ed021a7e3");

        private const int ProjectIcon = 0;

        public static ImageMoniker ProjectIconImageMoniker
        {
            get
            {
                return new ImageMoniker { Guid = ManifestGuid, Id = ProjectIcon };
            }
        }
    }
}
