using System.IO;

namespace DiggerClassic
{
    public static class Resources
    {
        public static Stream FindResource(string path)
        {
            var type = typeof(Resources);
            var dll = type.Assembly;
            var prefix = type.FullName;
            var fullName = prefix + path.Replace('/', '.');
            var resource = dll.GetManifestResourceStream(fullName);
            return resource;
        }
    }
}