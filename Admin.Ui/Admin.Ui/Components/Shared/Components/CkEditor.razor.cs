using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Admin.Ui.Components.Shared.Components;

public partial class CkEditor
{
    private ElementReference EditorElement;
    private IJSObjectReference? _editorInstance;

    [Parameter]
    public string? HtmlContent { get; set; }

    [Parameter]
    public EventCallback<string> HtmlContentChanged { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                _editorInstance = await JSRuntime.InvokeAsync<IJSObjectReference>(
                    "initializeCKEditor", 
                    EditorElement, 
                    DotNetObjectReference.Create(this),
                    HtmlContent);
            }
            catch (JSException ex)
            {
                Console.WriteLine($"Error initializing CKEditor: {ex.Message}");
            }
        }
    }

    [JSInvokable]
    public async Task EditorDataChanged(string data)
    {
        HtmlContent = data;
        await HtmlContentChanged.InvokeAsync(data);
    }

    public async ValueTask DisposeAsync()
    {
        if (_editorInstance != null)
        {
            try
            {
                await _editorInstance.InvokeVoidAsync("destroy");
                await _editorInstance.DisposeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disposing editor: {ex.Message}");
            }
        }
    }
}