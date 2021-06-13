using DotNetNuke.Instrumentation;
using DotNetNuke.Web.Api;
using System.Net;
using System.Net.Http;

namespace Connect.LanguagePackManager.Presentation.Common
{
    public class LanguagePackManagerApiController : DnnApiController
    {
        public static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(DnnApiController));
        
        private ContextHelper _LanguagePackManagerModuleContext;
        public ContextHelper LanguagePackManagerModuleContext
        {
            get { return _LanguagePackManagerModuleContext ?? (_LanguagePackManagerModuleContext = new ContextHelper(this)); }
        }

        public HttpResponseMessage ServiceError(string message) {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, message);
        }

        public HttpResponseMessage AccessViolation(string message)
        {
            return Request.CreateResponse(HttpStatusCode.Unauthorized, message);
        }

    }
}