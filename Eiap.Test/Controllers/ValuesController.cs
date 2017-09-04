
using System.Web.Http;

namespace Eiap.Test.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public string Get()
        {
            return DependencyManager.Instance.Resolver<IConfigurationManager>().CurrentEnvironment;
        }
    }
}
