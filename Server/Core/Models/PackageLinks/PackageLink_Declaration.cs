using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Connect.LanguagePackManager.Core.Models.PackageLinks
{

    [TableName("vw_Connect_LPM_PackageLinks")]
    [PrimaryKey("PackageLinkId", AutoIncrement = true)]
    [DataContract]
    [Scope("ModuleId")]                
    public partial class PackageLink  : PackageLinkBase 
    {

        #region .ctor
        public PackageLink()  : base() 
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public int? PortalID { get; set; }
        [DataMember]
        public string CreatedByUser { get; set; }
        [DataMember]
        public string ModifiedByUser { get; set; }
        #endregion

        #region Methods
        public PackageLinkBase GetPackageLinkBase()
        {
            PackageLinkBase res = new PackageLinkBase();
             res.PackageLinkId = PackageLinkId;
             res.ModuleId = ModuleId;
             res.Name = Name;
             res.OrgName = OrgName;
             res.RepoName = RepoName;
             res.AssetRegex = AssetRegex;
             res.LastChecked = LastChecked;
             res.LastDownloadedVersion = LastDownloadedVersion;
             res.IsResourcesRepo = IsResourcesRepo;
            res.CreatedByUserID = CreatedByUserID;
            res.CreatedOnDate = CreatedOnDate;
            res.LastModifiedByUserID = LastModifiedByUserID;
            res.LastModifiedOnDate = LastModifiedOnDate;
            return res;
        }
        public PackageLink Clone()
        {
            PackageLink res = new PackageLink();
            res.PackageLinkId = PackageLinkId;
            res.ModuleId = ModuleId;
            res.Name = Name;
            res.OrgName = OrgName;
            res.RepoName = RepoName;
            res.AssetRegex = AssetRegex;
            res.LastChecked = LastChecked;
            res.LastDownloadedVersion = LastDownloadedVersion;
            res.IsResourcesRepo = IsResourcesRepo;
            res.PortalID = PortalID;
            res.CreatedByUser = CreatedByUser;
            res.ModifiedByUser = ModifiedByUser;
            res.CreatedByUserID = CreatedByUserID;
            res.CreatedOnDate = CreatedOnDate;
            res.LastModifiedByUserID = LastModifiedByUserID;
            res.LastModifiedOnDate = LastModifiedOnDate;
            return res;
        }
        #endregion

    }
}
