using System.IO;

namespace ViewModelLib.Utilities
{
    public interface IStringService
    {
        string[] ReadAllLines(string path, FileAccess access, FileShare share);
    }
}
