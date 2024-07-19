using PortraitsByPerminder.Model;
using PortraitsByPerminder.Data;
using Microsoft.EntityFrameworkCore;

namespace PortraitsByPerminder.Services
{
    public class PhotoService
    {
        private readonly PhotoContext _photoContext;

        public PhotoService(PhotoContext photoContext)
        {
            _photoContext = photoContext;
        }
    
        public async Task<List<Photo>> GetPhotosAsync()
        {
            return await _photoContext.Photos.ToListAsync();
        }

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            return await _photoContext.Photos.FindAsync(id);
        }
    }
}