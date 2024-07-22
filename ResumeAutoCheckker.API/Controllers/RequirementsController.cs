using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ResumeAutoCheckker.BuissnessLogic.ViewModels;

namespace ResumeAutoCheckker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequirementsController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public RequirementsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpPost]
        public ResponseModel Create(string requirements)
        {
            _memoryCache.Set("requirements", requirements, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddDays(30),
            });

            return new ResponseModel {
                StatusCode = 200,
                Message = "Requirements Successfully Saved To Memory Cache!",
                isSuccess = true
            };
        }

        [HttpGet]
        public ResponseModel Get()
        {
            var requirements = _memoryCache.Get("requirements") as string;

            return new ResponseModel
            {
                StatusCode = 200,
                Message = requirements!,
                isSuccess = true
            };
        }

        [HttpDelete]
        public ResponseModel Delete()
        {
            _memoryCache.Remove("requirements");

            return new ResponseModel
            {
                StatusCode = 200,
                Message = "Requirements Successfully Deleted From Memory Cache!",
                isSuccess = true
            };
        }
    }
}
