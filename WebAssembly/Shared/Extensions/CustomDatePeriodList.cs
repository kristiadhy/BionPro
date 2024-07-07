using static WebAssembly.Shared.Enum.DataFilterEnum;

namespace WebAssembly.Shared.Extensions;

public class CustomDatePeriodList
{
    public TimePeriod TimePeriod { get; }

    public CustomDatePeriodList(TimePeriod timePeriod)
    {
        TimePeriod = timePeriod;
    }
}
