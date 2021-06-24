using System;
using System.Collections.Generic;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.PackageVersionLocaleTextCounts;

namespace Connect.LanguagePackManager.Core.Repositories
{

	public partial class PackageVersionLocaleTextCountRepository : ServiceLocator<IPackageVersionLocaleTextCountRepository, PackageVersionLocaleTextCountRepository>, IPackageVersionLocaleTextCountRepository
 {
        protected override Func<IPackageVersionLocaleTextCountRepository> GetFactory()
        {
            return () => new PackageVersionLocaleTextCountRepository();
        }
        public PackageVersionLocaleTextCount GetPackageVersionLocaleTextCount(int packageVersionId, int localeId)
        {
            using (var context = DataContext.Instance())
            {
                return context.ExecuteSingleOrDefault<PackageVersionLocaleTextCount>(System.Data.CommandType.Text,
                    "SELECT * FROM {databaseOwner}{objectQualifier}vw_Connect_LPM_PackageVersionLocaleTextCounts WHERE PackageVersionId=@0 AND LocaleId=@1",
                    packageVersionId,localeId);
            }
        }
        public void AddPackageVersionLocaleTextCount(PackageVersionLocaleTextCountBase packageVersionLocaleTextCount)
        {
            Requires.NotNull(packageVersionLocaleTextCount);
            using (var context = DataContext.Instance())
            {
                context.Execute(System.Data.CommandType.Text,
                    "IF NOT EXISTS (SELECT * FROM {databaseOwner}{objectQualifier}Connect_LPM_PackageVersionLocaleTextCounts " +
                    "WHERE PackageVersionId=@0 AND LocaleId=@1) " +
                    "INSERT INTO {databaseOwner}{objectQualifier}Connect_LPM_PackageVersionLocaleTextCounts (PackageVersionId, LocaleId, NrTexts) " +
                    "SELECT @0, @1, @2", packageVersionLocaleTextCount.PackageVersionId, packageVersionLocaleTextCount.LocaleId, packageVersionLocaleTextCount.NrTexts);
            }
        }
        public void DeletePackageVersionLocaleTextCount(PackageVersionLocaleTextCountBase packageVersionLocaleTextCount)
        {
            DeletePackageVersionLocaleTextCount(packageVersionLocaleTextCount.PackageVersionId, packageVersionLocaleTextCount.LocaleId);
        }
        public void DeletePackageVersionLocaleTextCount(int packageVersionId, int localeId)
        {
            using (var context = DataContext.Instance())
            {
                context.Execute(System.Data.CommandType.Text,
                    "DELETE FROM {databaseOwner}{objectQualifier}Connect_LPM_PackageVersionLocaleTextCounts WHERE PackageVersionId=@0 AND LocaleId=@1",
                    packageVersionId,localeId);
            }
        }
        public void UpdatePackageVersionLocaleTextCount(PackageVersionLocaleTextCountBase packageVersionLocaleTextCount)
        {
            Requires.NotNull(packageVersionLocaleTextCount);
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<PackageVersionLocaleTextCountBase>();
                rep.Update("SET NrTexts=@0 WHERE PackageVersionId=@1 AND LocaleId=@2",
                          packageVersionLocaleTextCount.NrTexts, packageVersionLocaleTextCount.PackageVersionId,packageVersionLocaleTextCount.LocaleId);
            }
        } 
 }

    public partial interface IPackageVersionLocaleTextCountRepository
    {
        PackageVersionLocaleTextCount GetPackageVersionLocaleTextCount(int packageVersionId, int localeId);
        void AddPackageVersionLocaleTextCount(PackageVersionLocaleTextCountBase packageVersionLocaleTextCount);
        void DeletePackageVersionLocaleTextCount(PackageVersionLocaleTextCountBase packageVersionLocaleTextCount);
        void DeletePackageVersionLocaleTextCount(int packageVersionId, int localeId);
        void UpdatePackageVersionLocaleTextCount(PackageVersionLocaleTextCountBase packageVersionLocaleTextCount);
    }
}

