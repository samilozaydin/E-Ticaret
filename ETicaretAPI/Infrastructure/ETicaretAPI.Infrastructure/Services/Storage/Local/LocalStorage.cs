using ETicaretAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task DeleteAsync(string pathOrContainerName, string fileName)
            => File.Delete($"{pathOrContainerName}\\{fileName}");

        public List<string> GetFiles(string pathOrContainerName)
        {
            DirectoryInfo infos = new DirectoryInfo(pathOrContainerName);
            return infos.GetFiles().Select(file => file.Name).ToList();
        }

        public bool HasFile(string pathOrContainerName, string fileName)
            => File.Exists($"{pathOrContainerName}\\{fileName}");

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, pathOrContainerName);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            List<(string fileName, string path)> datas = new List<(string fileName, string path)>();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(pathOrContainerName, file.Name, HasFile);

                await CopyFileAsync($"{fullPath}\\{fileNewName}", file);

                datas.Add((fileNewName, $"{pathOrContainerName}\\{fileNewName}"));
            }


            //todo: if hatalıysa kullanıcıya hatalı yükleme yapıldığına dair bilgi verilmesini sağlayan exception
            return datas;
        }
        private async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(stream);
                await stream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                // todo: log!
                return false;
            }

        }
    }
}
