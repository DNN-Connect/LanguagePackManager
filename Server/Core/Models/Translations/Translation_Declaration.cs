using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Connect.LanguagePackManager.Core.Models.Translations
{

    [TableName("vw_Connect_LPM_Translations")]
    [DataContract]
    public partial class Translation  : TranslationBase 
    {

        #region .ctor
        public Translation()  : base() 
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public int PackageId { get; set; }
        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public string TextKey { get; set; }
        [DataMember]
        public string FirstInVersion { get; set; }
        [DataMember]
        public string DeprecatedInVersion { get; set; }
        [DataMember]
        public string CreatedByUser { get; set; }
        [DataMember]
        public string ModifiedByUser { get; set; }
        #endregion

        #region Methods
        public TranslationBase GetTranslationBase()
        {
            TranslationBase res = new TranslationBase();
             res.TextId = TextId;
             res.Locale = Locale;
             res.TextValue = TextValue;
            res.CreatedByUserID = CreatedByUserID;
            res.CreatedOnDate = CreatedOnDate;
            res.LastModifiedByUserID = LastModifiedByUserID;
            res.LastModifiedOnDate = LastModifiedOnDate;
            return res;
        }
        public Translation Clone()
        {
            Translation res = new Translation();
            res.TextId = TextId;
            res.Locale = Locale;
            res.TextValue = TextValue;
            res.PackageId = PackageId;
            res.FilePath = FilePath;
            res.TextKey = TextKey;
            res.FirstInVersion = FirstInVersion;
            res.DeprecatedInVersion = DeprecatedInVersion;
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
