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
            TextId = text.TextId;
            PackageVersionId = text.PackageVersionId;
            ResourceFileId = text.ResourceFileId;
            TextKey = text.TextKey;
            OriginalValue = text.OriginalValue;
            DeprecatedInVersionId = text.DeprecatedInVersionId;
        }
        #endregion

    }
}



