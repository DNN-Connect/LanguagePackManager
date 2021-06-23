using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;
using Connect.LanguagePackManager.Core.Data;

namespace Connect.LanguagePackManager.Core.Models.Locales
{
    [TableName("Connect_LPM_Locales")]
    [PrimaryKey("LocaleId", AutoIncrement = true)]
    [DataContract]
    public partial class Locale     {

        #region .ctor
        public Locale()
        {
            LocaleId = -1;
        }
        #endregion

        #region Properties
        [DataMember]
        public int LocaleId { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public int? GenericLocaleId { get; set; }
        #endregion

        #region Methods
        public void ReadLocale(Locale locale)
        {
            if (locale.LocaleId > -1)
                LocaleId = locale.LocaleId;

            if (!String.IsNullOrEmpty(locale.Code))
                Code = locale.Code;

            if (locale.GenericLocaleId > -1)
                GenericLocaleId = locale.GenericLocaleId;

        }
        #endregion

    }
}



