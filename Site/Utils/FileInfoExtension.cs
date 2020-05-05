using System.IO;

namespace Site.Utils
{
    public static class FileInfoExtension
    {
        public static long ObtenerTamanoArchivo(string rutaarchivo)
        {
            var f = new FileInfo(rutaarchivo);
            if(f != null) return f.Length;
            return default(long);
        }
    }
}
