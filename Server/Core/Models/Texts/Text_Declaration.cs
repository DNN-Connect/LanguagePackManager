using System;
using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Connect.LanguagePackManager.Core.Models.Texts
{

    [TableName("vw_Connect_LPM_Texts")]
    [PrimaryKey("TextId", AutoIncrement = true)]
    [DataContract]
    public partial class Text  : TextBase 
    {

        #region .ctor
        public Text()  : base() 
        {
        }
        #endregion

        #region Properties
        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public int PackageId { get; set; }
        [DataMember]
        public string FirstInVersion { get; set; }
        [DataMember]
        public string DeprecatedInVersion { get; set; }
        #endregion

        #region Methods
        public TextBase GetTextBase()
        {
            TextBase res = new TextBase();
             res.TextId = TextId;
             res.PackageVersionId = PackageVersionId;
             res.ResourceFileId = ResourceFileId;
             res.TextKey = TextKey;
             res.OriginalValue = OriginalValue;
             res.DeprecatedInVersionId = DeprecatedInVersionId;
            return res;
        }
        public Text Clone()
        {
            Text res = new Text();
            res.TextId = TextId;
            res.PackageVersionId = PackageVersionId;
            res.ResourceFileId = ResourceFileId;
            res.TextKey = TextKey;
            res.OriginalValue = OriginalValue;
            res.DeprecatedInVersionId = DeprecatedInVersionId;
            res.FilePath = FilePath;
            res.PackageId = PackageId;
            res.FirstInVersion = FirstInVersion;
            res.DeprecatedInVersion = DeprecatedInVersion;
            return res;
        }
        #endregion

    }
}
