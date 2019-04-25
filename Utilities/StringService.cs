using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.IO;

namespace ViewModelLib.Utilities
{
    [Export(typeof(IStringService))]
    class StringService : IStringService
    {
        public string[] ReadAllLines(string path, FileAccess access, FileShare share)
        {
            using (var fs = new FileStream(path, FileMode.Open, access, share))
            using (var sr = new StreamReader(fs))
            {
                var file = new List<string>();
                while (!sr.EndOfStream)
                {
                    file.Add(sr.ReadLine());
                }

                return file.ToArray();
            }
        }
    }
}
