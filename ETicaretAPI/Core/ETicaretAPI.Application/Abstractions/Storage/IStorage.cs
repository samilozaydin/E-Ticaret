using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Storage
{
    public interface IStorage
    {
        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files);
        public Task DeleteAsync(string pathOrContainerName, string fileName);
        public List<string> GetFiles(string pathOrContainerName);
        public bool HasFile(string pathOrContainerName, string FileName);

    }

}
