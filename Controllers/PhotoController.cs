using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortraitsByPerminder.Data;
using PortraitsByPerminder.Model;

namespace PortraitsByPerminder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly PhotoContext _photoContext;
        public PhotoController(PhotoContext photoContext)
        {
            _photoContext = photoContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Photo>>> GetPhotos()
        {
            var photos = await _photoContext.Photos.ToListAsync();
            return photos.OrderByDescending(p => p.Id).ToList();
        }
    }
}