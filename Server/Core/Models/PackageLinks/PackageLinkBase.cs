using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;
using Connect.LanguagePackManager.Core.Data;

namespace Connect.LanguagePackManager.Core.Models.PackageLinks
{
    [TableName("Connect_LPM_PackageLinks")]
    [PrimaryKey("PackageLinkId", AutoIncrement = true)]
    [DataContract]
    [Scope("ModuleId")]
    public partial class PackageLinkBase  : AuditableEntity 
    {

        #region .ctor
        public PackageLinkBase()
        {
            PackageLinkId = -1;
        }
        #endregion

        #region Properties
        [DataMember]
        public int PackageLinkId { get; set; }
        [DataMember]
        public int ModuleId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string OrgName { get; set; }
        [DataMember]
        public string RepoName { get; set; }
        [DataMember]
        public string AssetRegex { get; set; }
        [DataMember]
        public DateTime? LastChecked { get; set; }
        [DataMember]
        public string LastDownloadedVersion { get; set; }
        [DataMember]
        public bool IsResourcesRepo { get; set; }
        #endregion

        #region Methods
        public void ReadPackageLinkBase(PackageLinkBase packageLink)
        {
            PackageLinkId = packageLink.PackageLinkId;
            ModuleId = packageLink.ModuleId;
            Name = packageLink.Name;
            OrgName = packageLink.OrgName;
            RepoName = packageLink.RepoName;
            AssetRegex = packageLink.AssetRegex;
            LastChecked = packageLink.LastChecked;
            LastDownloadedVersion = packageLink.LastDownloadedVersion;
            IsResourcesRepo = packageLink.IsResourcesRepo;
        }
        #endregion

    }
}



