using KallpaBox.Core.Entities;

namespace KallpaBox.Core.Interfaces
{
    public interface IPhotoService
    {
        void CreatePhoto(Photo photo);
        Photo GetPhotoId(int? photoId);
        Photo GetPhotoClientId(int? clientId);
        void UpdatePhoto(Photo photo);
    }
}