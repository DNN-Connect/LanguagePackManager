using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;
using Connect.LanguagePackManager.Core.Data;

namespace Connect.LanguagePackManager.Core.Models.Texts
{
    [TableName("Connect_LPM_Texts")]
    [PrimaryKey("TextId", AutoIncrement = true)]
    [DataContract]
    public partial class TextBase     {

        #region .ctor
        public TextBase()
        {
            TextId = -1;
        }
        #endregion

        #region Properties
        [DataMember]
        public int TextId { get; set; }
        [DataMember]
        public int PackageVersionId { get; set; }
        [DataMember]
        public int ResourceFileId { get; set; }
        [DataMember]
        public string TextKey { get; set; }
        [DataMember]
        public string OriginalValue { get; set; }
        [DataMember]
        public int? DeprecatedInVersionId { get; set; }
        #endregion

        #region Methods
        public void ReadTextBase(TextBase text)
        {
            if (text.TextId > -1)
                TextId = text.TextId;

            if (text.PackageVersionId > -1)
                PackageVersionId = text.PackageVersionId;

            if (text.ResourceFileId > -1)
                ResourceFileId = text.ResourceFileId;

            if (!String.IsNullOrEmpty(text.TextKey))
                TextKey = text.TextKey;

            if (!String.IsNullOrEmpty(text.OriginalValue))
                OriginalValue = text.OriginalValue;

            if (text.DeprecatedInVersionId > -1)
                DeprecatedInVersionId = text.DeprecatedInVersionId;

        }
        #endregion

    }
}



