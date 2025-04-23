using System.ComponentModel.DataAnnotations;

namespace WebAssembly.Shared.Enum;

public class GlobalEnum
{
  public enum FormStatus
  {
    New,
    Edit
  }

  public enum TableAction
  {
    Add,
    Edit,
    Delete,
    Detail
  }

  public enum FilterText
  {
    [Display(Description = "Add Filter")]
    AddFilter,
    [Display(Description = "Clear All Filters")]
    ClearFilters
  }

  public enum FilterIcon
  {
    [Display(Description = "search")]
    Search,
    [Display(Description = "highlight_off")]
    Cancel
  }
}
