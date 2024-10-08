using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService
    {
        IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment= webHostEnvironment;
        }
        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if(!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            List<(string fileName, string path)> datas = new List<(string fileName, string path)>();
            List<bool> results = new List<bool>();
            foreach(IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(fullPath, file.FileName);

                bool result = await CopyFileAsync($"{fullPath}//{fileNewName}",file);

                datas.Add((fileNewName, $"{path}//{fileNewName}"));
                results.Add(result);
            }

            if(results.TrueForAll(result => result.Equals(true)))
            {
                return datas;
            }

            //todo: if hatalıysa kullanıcıya hatalı yükleme yapıldığına dair bilgi verilmesini sağlayan exception
            return null;
        }
        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(stream);
                await stream.FlushAsync();
                return true;
            }
            catch(Exception ex) {
                // todo: log!
                return false;
            }

        }

        private async Task<string> FileRenameAsync(string path,string fileName, int num =1)
        {
            return await Task.Run<string>(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string newFileName = $"{(num == 1 ? NameOperation.CharacterRegulatory(oldName) : $"{oldName.Split($"-{num-1}")[0]}-{num}")}{extension}";

                if (File.Exists($"{path}\\{newFileName}"))
                {
                    return await FileRenameAsync(path,newFileName,num+1);
                }

                return newFileName;
            });
        }

 
    }
}
