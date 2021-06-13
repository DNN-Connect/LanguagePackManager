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
        public int? ContainedIn { get; set; }
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
            if (package.PackageId > -1)
                PackageId = package.PackageId;

            if (package.LinkId > -1)
                LinkId = package.LinkId;

            if (package.ContainedIn > -1)
                ContainedIn = package.ContainedIn;

            if (!String.IsNullOrEmpty(package.PackageName))
                PackageName = package.PackageName;

            if (!String.IsNullOrEmpty(package.FriendlyName))
                FriendlyName = package.FriendlyName;

            if (!String.IsNullOrEmpty(package.PackageType))
                PackageType = package.PackageType;

            if (!String.IsNullOrEmpty(package.InstallPath))
                InstallPath = package.InstallPath;

            if (!String.IsNullOrEmpty(package.LastVersion))
                LastVersion = package.LastVersion;

        }
        #endregion

    }
}



