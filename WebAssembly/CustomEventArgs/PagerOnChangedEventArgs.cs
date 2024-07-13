namespace WebAssembly.CustomEventArgs;

public class PagerOnChangedEventArgs
{
    public int CurrentPage { get; set; }
    public bool IsFromFirstRender { get; set; }

    public PagerOnChangedEventArgs(int currentPage, bool isFromFirstRender)
    {
        CurrentPage = currentPage;
        IsFromFirstRender = isFromFirstRender;
    }
}
