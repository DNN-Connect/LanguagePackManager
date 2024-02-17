using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;
using Connect.LanguagePackManager.Core.Data;

namespace Connect.LanguagePackManager.Core.Models.Packages
{
    [TableName("Connect_LPM_Packages")]
    [PrimaryKey("PackageId", AutoIncrement = true)]
    [DataContract]
    public partial class PackageBase     {

        #region .ctor
        public PackageBase()
        {
            PackageId = -1;
        }
        #endregion

        #region Properties
        [DataMember]
        public int PackageId { get; set; }
        [DataMember]
        public int LinkId { get; set; }
        [DataMember]
        public string PackageName { get; set; }
        [DataMember]
        public string FriendlyName { get; set; }
        [DataMember]
        public string PackageType { get; set; }
        [DataMember]
        public string InstallPath { get; set; }
        [DataMember]
        public string LastVersion { get; set; }
        #endregion

        #region Methods
        public void ReadPackageBase(PackageBase package)
        {
            PackageId = package.PackageId;
            LinkId = package.LinkId;
            PackageName = package.PackageName;
            FriendlyName = package.FriendlyName;
            PackageType = package.PackageType;
            InstallPath = package.InstallPath;
            LastVersion = package.LastVersion;
        }
        #endregion

    }
}



