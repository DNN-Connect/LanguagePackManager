using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;
using Connect.LanguagePackManager.Core.Data;

namespace Connect.LanguagePackManager.Core.Models.ResourceFiles
{
    [TableName("Connect_LPM_ResourceFiles")]
    [PrimaryKey("ResourceFileId", AutoIncrement = true)]
    [DataContract]
    public partial class ResourceFile     {

        #region .ctor
        public ResourceFile()
        {
            ResourceFileId = -1;
        }
        #endregion

        #region Properties
        [DataMember]
        public int ResourceFileId { get; set; }
        [DataMember]
        public int PackageId { get; set; }
        [DataMember]
        public string FilePath { get; set; }
        #endregion

        #region Methods
        public void ReadResourceFile(ResourceFile resourceFile)
        {
            if (resourceFile.ResourceFileId > -1)
                ResourceFileId = resourceFile.ResourceFileId;

            if (resourceFile.PackageId > -1)
                PackageId = resourceFile.PackageId;

            if (!String.IsNullOrEmpty(resourceFile.FilePath))
                FilePath = resourceFile.FilePath;

        }
        #endregion

    }
}



