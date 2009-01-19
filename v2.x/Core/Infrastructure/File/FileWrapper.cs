namespace Kigg.Infrastructure
{
    using System.IO;

    public class FileWrapper : IFile
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public string[] ReadAllLine(string path)
        {
            return File.ReadAllLines(path);
        }

        public void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}