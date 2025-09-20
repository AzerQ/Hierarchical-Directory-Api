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

        [Route(HttpVerbs.Get, "/categories/{id}")]
        public async Task<CategoryDto> GetById(string id, bool includeChildren = false)
        {
            var result = await _service.GetByIdAsync(id, includeChildren);
            if (result == null)
                throw HttpException.NotFound($"Category {id} not found");
            return result;
        }

        [Route(HttpVerbs.Post, "/categories/batch")]
        public async Task<IEnumerable<CategoryDto>> GetBatch([JsonData] BatchRequest request)
        {
            if (request?.Ids == null)
                throw HttpException.BadRequest("Invalid request format");
            return await _service.GetBatchAsync(request.Ids, request.IncludeChildren);
        }

        [Route(HttpVerbs.Post, "/categories")]
        public async Task<CategoryDto> Create([JsonData] CategoryDto dto)
        {
            try
            {
                return await _service.CreateAsync(dto);
            }
            catch (Exception ex)
            {
                throw HttpException.BadRequest(ex.Message);
            }
        }

        [Route(HttpVerbs.Put, "/categories/{id}")]
        public async Task<CategoryDto> Update(string id, [JsonData] CategoryDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto);
                if (result == null)
                    throw HttpException.NotFound($"Category {id} not found");
                return result;
            }
            catch (Exception ex)
            {
                throw HttpException.BadRequest(ex.Message);
            }
        }

        [Route(HttpVerbs.Delete, "/categories/{id}")]
        public async Task Delete(string id)
        {
            await _service.DeleteAsync(id);
        }

        [Route(HttpVerbs.Post, "/categories/{parentId}/leaves")]
        public async Task<CategoryDto> CreateLeafVersion(string parentId, [JsonData] CategoryDto dto)
        {
            try
            {
                return await _service.CreateLeafVersionAsync(parentId, dto);
            }
            catch (Exception ex)
            {
                throw HttpException.BadRequest(ex.Message);
            }
        }

        [Route(HttpVerbs.Post, "/categories/{id}/validate")]
        public async Task<object> Validate(string id, [JsonData] ValidateRequest request)
        {
            var result = await _service.ValidateAsync(id, request.Data);
            return new { valid = result.valid, errors = result.errors };
        }

        public class BatchRequest
        {
            public List<string> Ids { get; set; }
            public bool IncludeChildren { get; set; }
        }

        public class ValidateRequest
        {
            public object Data { get; set; }
        }
    }
}
