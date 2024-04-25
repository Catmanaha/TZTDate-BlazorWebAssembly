using Microsoft.AspNetCore.Components;

namespace TZTDateBlazorWebAssembly.Models;

public class ImageFile
{
    public ElementReference ElementReference { get; set; }
    public FileStream fileStream { get; set; }
}
