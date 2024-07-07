using static WebAssembly.Shared.Enum.DataFilterEnum;

namespace WebAssembly.Shared.Extensions;

public class TimePeriodList<T>(TimePeriod periodType, IEnumerable<T> subPeriods)
{
    public TimePeriod PeriodType { get; } = periodType;
    public IEnumerable<T> SubPeriods { get; } = subPeriods;
}


