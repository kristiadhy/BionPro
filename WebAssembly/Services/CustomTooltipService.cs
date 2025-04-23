using Microsoft.AspNetCore.Components;
using Radzen;

namespace WebAssembly.Services;

public class CustomTooltipService
{
  private readonly TooltipService _tooltipService;
  public CustomTooltipService(TooltipService tooltipService)
  {
    _tooltipService = tooltipService;
  }

  internal void ShowBottomTooltip(ElementReference elementReference, string title)
  {
    TooltipOptions options = new()
    {
      Position = TooltipPosition.Bottom,
      Style = "background: var(--rz-base-600); color: white; opacity: 0.5;"
    };

    _tooltipService.Open(elementReference, title, options);
  }
}
