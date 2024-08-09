using Microsoft.JSInterop;

namespace RocketShop.SharedBlazor.SharedBlazorService.Scope
{
    public interface IDialogServices
    {
        Task Failed(string message, string? header = null);
        Task Success(string message, string? header = null);
        Task Custom(bool success, string message, string header);
        Task Custom(bool success, string message, string header, string footer);
        Task CustomWithHtml(bool success, string message, string header);
        Task CustomWithTimer(bool success, string message, string header, int timer);
        // Task CustomWithTimer(bool success, string message, string header, int timer, bool showerInterval);
        Task CustomWithNavigate(bool success, string message, string header, string link = "/", string? buttonColor = null, string buttonText = "OK");
    }
    public class DialogServices : IDialogServices
    {
        private readonly IJSRuntime _jSRuntime;
        public DialogServices(IJSRuntime jSRuntime) =>
            _jSRuntime = jSRuntime;

        public async Task Failed(string message, string? header = null) =>
            await _jSRuntime.InvokeAsync<object>("Failed", message, header);

        public async Task Success(string message, string? header = null) =>
            await _jSRuntime.InvokeAsync<object>("Success", message, header);

        public async Task Custom(bool success, string message, string header) =>
            await _jSRuntime.InvokeAsync<object>("Raw", success ? "success" : "error", header, message);

        public async Task Custom(bool success, string message, string header, string footer) =>
            await _jSRuntime.InvokeAsync<object>("Raw", success ? "success" : "error", header, message, footer);

        public async Task CustomWithHtml(bool success, string message, string header) =>
            await _jSRuntime.InvokeAsync<object>("Raw", success ? "success" : "error", header, message);

        public async Task CustomWithTimer(bool success, string message, string header, int timer) =>
            await _jSRuntime.InvokeAsync<object>("RawWithTimer", success ? "success" : "error", header, message, timer);

        /*  public async Task CustomWithTimer(bool success,string message,string header,int timer,bool showerInterval)
          {
              await _jSRuntime.InvokeAsync<object>("RawWithInterval", success ? "success":"error", header,message,timer,showerInterval);
          }*/
        public async Task CustomWithNavigate(bool success,
            string message,
            string header,
            string link = "/",
            string buttonColor = null,
            string buttonText = "OK") =>
            await _jSRuntime.InvokeAsync<object>("RawWithNavigate", success ? "success" : "error", header, message, link, buttonColor, buttonText);
    }
}
