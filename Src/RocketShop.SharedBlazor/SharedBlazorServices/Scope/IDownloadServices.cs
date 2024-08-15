using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.SharedBlazor.SharedBlazorServices.Scope
{
    public interface IDownloadServices
    {
        Task DownloadAsync(string filename, byte[] content);
    }
    public class DownloadServices :IDownloadServices
    {
        readonly IJSRuntime _jSRuntime;
        public DownloadServices(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }
        public async Task DownloadAsync(string filename, byte[] content)
        {
            using var streamRef = new DotNetStreamReference(new MemoryStream(content));
            await _jSRuntime.InvokeAsync<object>("downloadFileFromStream", filename, streamRef);
        }
    }
}
