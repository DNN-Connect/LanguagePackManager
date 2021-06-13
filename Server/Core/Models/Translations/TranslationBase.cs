using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;
using Connect.LanguagePackManager.Core.Data;

namespace Connect.LanguagePackManager.Core.Models.Translations
{
    [TableName("Connect_LPM_Translations")]
    [DataContract]
    public partial class TranslationBase  : AuditableEntity 
    {

        #region .ctor
        public TranslationBase()
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public int TextId { get; set; }
        [DataMember]
        public string Locale { get; set; }
        [DataMember]
        public string TextValue { get; set; }
        #endregion

        #region Methods
        public void ReadTranslationBase(TranslationBase translation)
        {
            if (translation.TextId > -1)
                TextId = translation.TextId;

            if (!String.IsNullOrEmpty(translation.Locale))
                Locale = translation.Locale;

            if (!String.IsNullOrEmpty(translation.TextValue))
                TextValue = translation.TextValue;

        }
        #endregion

    }
}



