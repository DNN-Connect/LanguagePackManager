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
        #endregion

        #region Methods
        public void ReadPackageVersionBase(PackageVersionBase packageVersion)
        {
            if (packageVersion.PackageVersionId > -1)
                PackageVersionId = packageVersion.PackageVersionId;

            if (packageVersion.PackageId > -1)
                PackageId = packageVersion.PackageId;

            if (packageVersion.ContainedInPackageVersionId > -1)
                ContainedInPackageVersionId = packageVersion.ContainedInPackageVersionId;

            if (!String.IsNullOrEmpty(packageVersion.Version))
                Version = packageVersion.Version;

            ReleaseDate = packageVersion.ReleaseDate;

            Downloaded = packageVersion.Downloaded;

        }
        #endregion

    }
}



