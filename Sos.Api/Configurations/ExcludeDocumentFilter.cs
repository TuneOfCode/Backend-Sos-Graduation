using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Sos.Api.Configurations
{
    /// <summary>
    /// Represents exclude document filter.
    /// </summary>
    public class ExcludeDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc.Paths.ContainsKey("/"))
            {
                var rootPath = swaggerDoc.Paths["/"];
                foreach (var method in rootPath.Operations.Keys.ToArray())
                {
                    rootPath.Operations.Remove(method);
                }
            }
        }
    }
}
