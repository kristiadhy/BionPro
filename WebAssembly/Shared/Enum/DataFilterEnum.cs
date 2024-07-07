namespace WebAssembly.Shared.Enum;

public class DataFilterEnum
{
    public enum TimePeriod
    {
        ByDay,
        ByWeek,
        ByMonth,
        ByCustomDate
    }

    public enum DaySubPeriod
    {
        Today,
        Last3Days
    }

    public enum WeekSubPeriod
    {
        ThisWeek,
        LastWeek
    }

    public enum MonthSubPeriod
    {
        ThisMonth,
        PreviousMonth,
        Last3Months
    }

}
