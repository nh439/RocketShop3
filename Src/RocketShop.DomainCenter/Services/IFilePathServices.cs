using Org.BouncyCastle.Crypto.Agreement;

namespace RocketShop.DomainCenter.Services
{
    public interface IFilePathServices
    {
        bool CheckFileExists(string fileName);
        string GetContent(string fileName);
        void Create(string fileName, string content);
    }
    public class FilePathServices(IWebHostEnvironment hostingEnvironment,IConfiguration configuration) : IFilePathServices
    {
        readonly string? mount = configuration.GetSection("BindMount").Value;
        string GetBaseDir() => $"{(mount ?? hostingEnvironment.WebRootPath)}/packages";

        public bool CheckFileExists(string fileName)
        {
            
            var dirExists = Directory.Exists(GetBaseDir());
            if (!dirExists) { 
                Directory.CreateDirectory(GetBaseDir());
                return false;
            }
            return File.Exists($"{GetBaseDir()}/{fileName}");
        }
        public string GetContent(string fileName) =>
            File.ReadAllText($"{GetBaseDir()}/{fileName}");

        public void Create(string fileName, string content)
        {
            if (CheckFileExists(fileName))
                File.Delete($"{GetBaseDir()}/{fileName}");
            File.WriteAllText($"{GetBaseDir()}/{fileName}", content);
        }
            

        
    }
}
