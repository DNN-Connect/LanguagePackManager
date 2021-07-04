using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Instrumentation;
using DotNetNuke.Web.Api;

namespace Connect.LanguagePackManager.Presentation.Common
{
    public enum SecurityAccessLevel
    {
        Anonymous = 0,
        Authenticated = 1,
        View = 2,
        Edit = 3,
        Admin = 4,
        Host = 5,
        Translator = 6
    }

    public class LpmAuthorizeAttribute : AuthorizeAttributeBase, IOverrideDefaultAuthLevel
    {
        private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(LpmAuthorizeAttribute));
        public SecurityAccessLevel SecurityLevel { get; set; }
        public UserInfo User { get; set; }
        public bool AllowApiKeyAccess { get; set; } = false;

        public LpmAuthorizeAttribute()
        {
            SecurityLevel = SecurityAccessLevel.Admin;
        }

        public LpmAuthorizeAttribute(SecurityAccessLevel accessLevel)
        {
            SecurityLevel = accessLevel;
        }

        public override bool IsAuthorized(AuthFilterContext context)
        {
            Logger.Trace("IsAuthorized");
            if (SecurityLevel == SecurityAccessLevel.Anonymous)
            {
                Logger.Trace("Anonymous");
                return true;
            }
            User = HttpContextSource.Current.Request.IsAuthenticated ? UserController.Instance.GetCurrentUserInfo() : new UserInfo();
            Logger.Trace("UserId " + User.UserID.ToString());
            ContextSecurity security = new ContextSecurity(context.ActionContext.Request.FindModuleInfo(), User);
            Logger.Trace(security.ToString());
            switch (SecurityLevel)
            {
                case SecurityAccessLevel.Authenticated:
                    return User.UserID != -1;
                case SecurityAccessLevel.Host:
                    return User.IsSuperUser;
                case SecurityAccessLevel.Admin:
                    return security.IsAdmin;
                case SecurityAccessLevel.Edit:
                    return security.CanEdit;
                case SecurityAccessLevel.View:
                    return security.CanView;
                case SecurityAccessLevel.Translator:
                    return security.IsTranslator;
            }
            return false;
        }
    }
}