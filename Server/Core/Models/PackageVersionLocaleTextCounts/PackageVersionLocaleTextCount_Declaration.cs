using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Connect.LanguagePackManager.Core.Models.PackageVersionLocaleTextCounts
{

    [TableName("vw_Connect_LPM_PackageVersionLocaleTextCounts")]
    [DataContract]
    public partial class PackageVersionLocaleTextCount  : PackageVersionLocaleTextCountBase 
    {

        #region .ctor
        public PackageVersionLocaleTextCount()  : base() 
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public int? OriginalNr { get; set; }
        [DataMember]
        public int PackageId { get; set; }
        #endregion

        #region Methods
        public PackageVersionLocaleTextCountBase GetPackageVersionLocaleTextCountBase()
        {
            PackageVersionLocaleTextCountBase res = new PackageVersionLocaleTextCountBase();
             res.PackageVersionId = PackageVersionId;
             res.LocaleId = LocaleId;
             res.NrTexts = NrTexts;
             res.LastChange = LastChange;
            return res;
        }
        public PackageVersionLocaleTextCount Clone()
        {
            PackageVersionLocaleTextCount res = new PackageVersionLocaleTextCount();
            res.PackageVersionId = PackageVersionId;
            res.LocaleId = LocaleId;
            res.NrTexts = NrTexts;
            res.LastChange = LastChange;
            res.OriginalNr = OriginalNr;
            res.PackageId = PackageId;
            return res;
        }
        #endregion

    }
}
