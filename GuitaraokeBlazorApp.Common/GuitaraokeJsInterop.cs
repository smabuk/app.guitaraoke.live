using Microsoft.JSInterop;

namespace GuitaraokeBlazorApp.Common;

public class GuitaraokeJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public GuitaraokeJsInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new (() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/GuitaraokeBlazorApp.Common/guitaraokeJsInterop.js").AsTask());
    }


    public async ValueTask<string> Prompt(string message)
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<string>("showPrompt", message);
    }

    public async ValueTask<bool> Confirm(string message)
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<bool>("showConfirm", message);
    }


	// Start of Drag and Drop


	public async ValueTask<object> InitDragAndDrop() {
		var module = await moduleTask.Value;
		return await module.InvokeAsync<object>("initDragAndDrop");
	}



	//   End of Drag and Drop



	public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
