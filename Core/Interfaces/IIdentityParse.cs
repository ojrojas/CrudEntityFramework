using System.Security.Principal;

namespace KallpaBox.Core.Interfaces
{
    public interface IIdentityParse<T>
    {
        T Parse(IPrincipal principal);
    }
    
}