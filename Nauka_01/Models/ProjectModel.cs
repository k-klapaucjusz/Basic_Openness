using System.IO;

namespace Basic_Openness.Models
{
    public class ProjectModel
    {
        #region properties

        public string Name
        {
            get;
            set;
        }

        public DirectoryInfo TargetDirectory
        {
            get;
            set;
        }

        #endregion // properties
    }
}
