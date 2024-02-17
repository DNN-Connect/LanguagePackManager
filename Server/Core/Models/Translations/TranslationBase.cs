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
        public int Locale { get; set; }
        [DataMember]
        public string TextValue { get; set; }
        #endregion

        #region Methods
        public void ReadTranslationBase(TranslationBase translation)
        {
            TextId = translation.TextId;
            Locale = translation.Locale;
            TextValue = translation.TextValue;
        }
        #endregion

    }
}



