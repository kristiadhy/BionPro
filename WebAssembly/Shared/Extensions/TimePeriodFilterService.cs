using static WebAssembly.Shared.Enum.DataFilterEnum;

namespace WebAssembly.Shared.Extensions;

public class TimePeriodFilterService
{
    public static List<object> GetTimePeriodData()
    {
        return
        [
             new TimePeriodList<DaySubPeriod>(TimePeriod.ByDay, [DaySubPeriod.Today, DaySubPeriod.Last3Days]),
             new TimePeriodList<WeekSubPeriod>(TimePeriod.ByWeek, [WeekSubPeriod.ThisWeek, WeekSubPeriod.LastWeek]),
             new TimePeriodList<MonthSubPeriod>(TimePeriod.ByMonth, [MonthSubPeriod.ThisMonth, MonthSubPeriod.PreviousMonth, MonthSubPeriod.Last3Months]),
             new CustomDatePeriodList(TimePeriod.ByCustomDate)
        ];
    }
}
