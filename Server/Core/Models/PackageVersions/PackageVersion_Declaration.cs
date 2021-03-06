using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Connect.LanguagePackManager.Core.Models.PackageVersions
{

    [TableName("vw_Connect_LPM_PackageVersions")]
    [PrimaryKey("PackageVersionId", AutoIncrement = true)]
    [DataContract]
    public partial class PackageVersion  : PackageVersionBase 
    {

        #region .ctor
        public PackageVersion()  : base() 
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public string PackageName { get; set; }
        [DataMember]
        public string FriendlyName { get; set; }
        [DataMember]
        public string PackageType { get; set; }
        [DataMember]
        public string PackageLinkName { get; set; }
        [DataMember]
        public DateTime? LastChecked { get; set; }
        [DataMember]
        public int ModuleId { get; set; }
        [DataMember]
        public int? PortalID { get; set; }
        #endregion

        #region Methods
        public PackageVersionBase GetPackageVersionBase()
        {
            PackageVersionBase res = new PackageVersionBase();
             res.PackageVersionId = PackageVersionId;
             res.PackageId = PackageId;
             res.ContainedInPackageVersionId = ContainedInPackageVersionId;
             res.Version = Version;
             res.ReleaseDate = ReleaseDate;
             res.Downloaded = Downloaded;
             res.NrTexts = NrTexts;
            return res;
        }
        public PackageVersion Clone()
        {
            PackageVersion res = new PackageVersion();
            res.PackageVersionId = PackageVersionId;
            res.PackageId = PackageId;
            res.ContainedInPackageVersionId = ContainedInPackageVersionId;
            res.Version = Version;
            res.ReleaseDate = ReleaseDate;
            res.Downloaded = Downloaded;
            res.NrTexts = NrTexts;
            res.PackageName = PackageName;
            res.FriendlyName = FriendlyName;
            res.PackageType = PackageType;
            res.PackageLinkName = PackageLinkName;
            res.LastChecked = LastChecked;
            res.ModuleId = ModuleId;
            res.PortalID = PortalID;
            return res;
        }
        #endregion

    }
}
