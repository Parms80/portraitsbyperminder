using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortraitsByPerminder.Services
{
    public class PhotoService
    {
        public List<Model.Photo> GetPhotos()
        {
            return new List<Model.Photo>()
            {
                new Model.Photo { Url = "img/gallery/photo1.jpg", Title = "Girl in black dress", Description = "Girl in black dress"}
            };
        }
    }
}