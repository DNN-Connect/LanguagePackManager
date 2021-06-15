using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Connect.LanguagePackManager.Core.Models.Packages
{

    [TableName("vw_Connect_LPM_Packages")]
    [PrimaryKey("PackageId", AutoIncrement = true)]
    [DataContract]
    public partial class Package  : PackageBase 
    {

        #region .ctor
        public Package()  : base() 
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime? LastChecked { get; set; }
        [DataMember]
        public int ModuleId { get; set; }
        #endregion

        #region Methods
        public PackageBase GetPackageBase()
        {
            PackageBase res = new PackageBase();
             res.PackageId = PackageId;
             res.LinkId = LinkId;
             res.PackageName = PackageName;
             res.FriendlyName = FriendlyName;
             res.PackageType = PackageType;
             res.InstallPath = InstallPath;
             res.LastVersion = LastVersion;
            return res;
        }
        public Package Clone()
        {
            Package res = new Package();
            res.PackageId = PackageId;
            res.LinkId = LinkId;
            res.PackageName = PackageName;
            res.FriendlyName = FriendlyName;
            res.PackageType = PackageType;
            res.InstallPath = InstallPath;
            res.LastVersion = LastVersion;
            res.Name = Name;
            res.LastChecked = LastChecked;
            res.ModuleId = ModuleId;
            return res;
        }
        #endregion

    }
}
