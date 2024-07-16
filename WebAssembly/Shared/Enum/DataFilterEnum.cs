using System.ComponentModel.DataAnnotations;

namespace WebAssembly.Shared.Enum;

public class DataFilterEnum
{
    public enum TimePeriod
    {
        [Display(Description = "By Day")]
        ByDay,
        [Display(Description = "By Week")]
        ByWeek,
        [Display(Description = "By Month")]
        ByMonth,
        [Display(Description = "By Custom Date")]
        ByCustomDate
    }

    public enum DaySubPeriod
    {
        Today,
        [Display(Description = "Last 3 Days")]
        Last3Days
    }

    public enum WeekSubPeriod
    {
        [Display(Description = "This Week")]
        ThisWeek,
        [Display(Description = "Last Week")]
        LastWeek
    }

    public enum MonthSubPeriod
    {
        [Display(Description = "This Month")]
        ThisMonth,
        [Display(Description = "Previous Month")]
        PreviousMonth,
        [Display(Description = "Last 3 Months")]
        Last3Months
    }

}
