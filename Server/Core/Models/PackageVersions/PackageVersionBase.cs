using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;
using Connect.LanguagePackManager.Core.Data;

namespace Connect.LanguagePackManager.Core.Models.PackageVersions
{
    [TableName("Connect_LPM_PackageVersions")]
    [PrimaryKey("PackageVersionId", AutoIncrement = true)]
    [DataContract]
    public partial class PackageVersionBase     {

        #region .ctor
        public PackageVersionBase()
        {
            PackageVersionId = -1;
        }
        #endregion

        #region Properties
        [DataMember]
        public int PackageVersionId { get; set; }
        [DataMember]
        public int PackageId { get; set; }
        [DataMember]
        public int? ContainedInPackageVersionId { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public DateTime ReleaseDate { get; set; }
        [DataMember]
        public DateTime Downloaded { get; set; }
        [DataMember]
        public int? NrTexts { get; set; }
        #endregion

        #region Methods
        public void ReadPackageVersionBase(PackageVersionBase packageVersion)
        {
            PackageVersionId = packageVersion.PackageVersionId;
            PackageId = packageVersion.PackageId;
            ContainedInPackageVersionId = packageVersion.ContainedInPackageVersionId;
            Version = packageVersion.Version;
            ReleaseDate = packageVersion.ReleaseDate;
            Downloaded = packageVersion.Downloaded;
            NrTexts = packageVersion.NrTexts;
        }
        #endregion

    }
}



