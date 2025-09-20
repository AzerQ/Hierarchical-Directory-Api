using System.Collections.Generic;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.WebApi;
using HierarchicalDirectory.Application;
using Microsoft.Extensions.DependencyInjection;

namespace HierarchicalDirectory.Api
{
    public class CategoriesController : WebApiController
    {
        private readonly ICategoryService _service;
        public CategoriesController(ServiceProvider provider)
        {
            _service = provider.GetService<ICategoryService>();
        }

        [Route(HttpVerbs.Get, "/categories")]
        public async Task<IEnumerable<CategoryDto>> GetAll(int? depth = null, string? search = null)
        {
            return await _service.GetAllAsync(depth, search);
        }

        // TODO: Add other endpoints as per README
    }
}
