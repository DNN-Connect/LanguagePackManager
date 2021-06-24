using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;
using Connect.LanguagePackManager.Core.Data;

namespace Connect.LanguagePackManager.Core.Models.PackageVersionLocaleTextCounts
{
    [TableName("Connect_LPM_PackageVersionLocaleTextCounts")]
    [DataContract]
    public partial class PackageVersionLocaleTextCountBase     {

        #region .ctor
        public PackageVersionLocaleTextCountBase()
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public int PackageVersionId { get; set; }
        [DataMember]
        public int LocaleId { get; set; }
        [DataMember]
        public int NrTexts { get; set; }
        #endregion

        #region Methods
        public void ReadPackageVersionLocaleTextCountBase(PackageVersionLocaleTextCountBase packageVersionLocaleTextCount)
        {
            if (packageVersionLocaleTextCount.PackageVersionId > -1)
                PackageVersionId = packageVersionLocaleTextCount.PackageVersionId;

            if (packageVersionLocaleTextCount.LocaleId > -1)
                LocaleId = packageVersionLocaleTextCount.LocaleId;

            if (packageVersionLocaleTextCount.NrTexts > -1)
                NrTexts = packageVersionLocaleTextCount.NrTexts;

        }
        #endregion

    }
}



