using System.Linq;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using KallpaBox.Site.Data;

namespace Site.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IRepository<Photo> _photoRepository;
        public PhotoService() { _photoRepository = new EfRepository<Photo>(new GymContext()); }

        public void CreatePhoto(Photo photo)
        {
             _photoRepository.Add(photo);
        }

        public Photo GetPhotoClientId(int? clientId)
        {
            var photo =  _photoRepository.ListAll();
            var photoResult = photo.Where(x => x.ClientId == clientId).FirstOrDefault();
            return photoResult;
        }

        public Photo GetPhotoId(int? photoId)
        {
            return _photoRepository.GetById(photoId);
        }

        public void UpdatePhoto(Photo photo)
        {
            _photoRepository.Update(photo);
        }
    }
}