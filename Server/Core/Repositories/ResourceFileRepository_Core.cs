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
        protected override Func<IResourceFileRepository> GetFactory()
        {
            return () => new ResourceFileRepository();
        }
        public IEnumerable<ResourceFile> GetResourceFiles()
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ResourceFile>();
                return rep.Get();
            }
        }
        public ResourceFile GetResourceFile(int resourceFileId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ResourceFile>();
                return rep.GetById(resourceFileId);
            }
        }
        public ResourceFile AddResourceFile(ResourceFile resourceFile)
        {
            Requires.NotNull(resourceFile);
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ResourceFile>();
                rep.Insert(resourceFile);
            }
            return resourceFile;
        }
        public void DeleteResourceFile(ResourceFile resourceFile)
        {
            Requires.NotNull(resourceFile);
            Requires.PropertyNotNegative(resourceFile, "ResourceFileId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ResourceFile>();
                rep.Delete(resourceFile);
            }
        }
        public void DeleteResourceFile(int resourceFileId)
        {
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ResourceFile>();
                rep.Delete("WHERE ResourceFileId = @0", resourceFileId);
            }
        }
        public void UpdateResourceFile(ResourceFile resourceFile)
        {
            Requires.NotNull(resourceFile);
            Requires.PropertyNotNegative(resourceFile, "ResourceFileId");
            using (var context = DataContext.Instance())
            {
                var rep = context.GetRepository<ResourceFile>();
                rep.Update(resourceFile);
            }
        } 
    }
    public partial interface IResourceFileRepository
    {
        IEnumerable<ResourceFile> GetResourceFiles();
        ResourceFile GetResourceFile(int resourceFileId);
        ResourceFile AddResourceFile(ResourceFile resourceFile);
        void DeleteResourceFile(ResourceFile resourceFile);
        void DeleteResourceFile(int resourceFileId);
        void UpdateResourceFile(ResourceFile resourceFile);
    }
}

