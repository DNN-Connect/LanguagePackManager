using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Connect.LanguagePackManager.Core.Models.ResourceFiles;

namespace Connect.LanguagePackManager.Core.Repositories
{
	public partial class ResourceFileRepository : ServiceLocator<IResourceFileRepository, ResourceFileRepository>, IResourceFileRepository
    {
        public IEnumerable<ResourceFile> GetResourceFilesByPackage(int packageId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ResourceFile>();
                return rep.Find($"WHERE PackageId=@0", packageId);
            }
        }
    }
    public partial interface IResourceFileRepository
    {
        IEnumerable<ResourceFile> GetResourceFilesByPackage(int packageId);
    }
}

