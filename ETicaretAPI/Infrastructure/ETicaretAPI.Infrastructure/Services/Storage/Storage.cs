using ETicaretAPI.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName,string fileName);
        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod, int num = 1)
        {
            return await Task.Run<string>(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string newFileName = $"{(num == 1 ? NameOperation.CharacterRegulatory(oldName) : $"{oldName.Split($"-{num - 1}")[0]}-{num}")}{extension}";

                if (hasFileMethod(pathOrContainerName, newFileName))
                {
                    return await FileRenameAsync(pathOrContainerName, newFileName,hasFileMethod, num + 1);
                }

                return newFileName;
            });
        }
    }
}
