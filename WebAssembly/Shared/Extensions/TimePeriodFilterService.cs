//using static WebAssembly.Shared.Enum.DataFilterEnum;

using static WebAssembly.Shared.Enum.DataFilterEnum;

public class TimePeriodFilterService
{
  //public static List<object> GetTimePeriodData()
  //{
  //    return
  //    [
  //         new TimePeriodList<DaySubPeriod>(TimePeriod.ByDay, [DaySubPeriod.Today, DaySubPeriod.Last3Days]),
  //         new TimePeriodList<WeekSubPeriod>(TimePeriod.ByWeek, [WeekSubPeriod.ThisWeek, WeekSubPeriod.LastWeek]),
  //         new TimePeriodList<MonthSubPeriod>(TimePeriod.ByMonth, [MonthSubPeriod.ThisMonth, MonthSubPeriod.PreviousMonth, MonthSubPeriod.Last3Months]),
  //         new CustomDatePeriodList(TimePeriod.ByCustomDate)
  //    ];
  //}

  public static IEnumerable<Enum> SetDetailTimePeriod(TimePeriod timePeriod)
  {
    return timePeriod switch
    {
      TimePeriod.ByDay => Enum.GetValues(typeof(DaySubPeriod)).Cast<Enum>(),
      TimePeriod.ByWeek => Enum.GetValues(typeof(WeekSubPeriod)).Cast<Enum>(),
      TimePeriod.ByMonth => Enum.GetValues(typeof(MonthSubPeriod)).Cast<Enum>(),
      TimePeriod.ByCustomDate => [],
      _ => throw new ArgumentOutOfRangeException(nameof(timePeriod), timePeriod, null),
    };
  }

  public static (DateTime StartDate, DateTime EndDate) SetDateRangeBasedOnDetailSelection(Enum? detailTimePeriod)
  {
    if (detailTimePeriod == null)
      throw new ArgumentNullException(nameof(detailTimePeriod), "Detail time period cannot be null.");

    DateTime today = DateTime.Today;
    DateTime firstDayOfThisMonth = new DateTime(today.Year, today.Month, 1);

    if (detailTimePeriod.GetType() == typeof(DaySubPeriod))
    {
      return SetDateRangeForDaySubPeriod((DaySubPeriod)detailTimePeriod, today);
    }
    else if (detailTimePeriod.GetType() == typeof(WeekSubPeriod))
    {
      return SetDateRangeForWeekSubPeriod((WeekSubPeriod)detailTimePeriod, today);
    }
    else if (detailTimePeriod.GetType() == typeof(MonthSubPeriod))
    {
      return SetDateRangeForMonthSubPeriod((MonthSubPeriod)detailTimePeriod, firstDayOfThisMonth);
    }
    else
    {
      throw new NotImplementedException($"Handling for {detailTimePeriod.GetType().Name} is not implemented.");
    }
  }


  private static (DateTime StartDate, DateTime EndDate) SetDateRangeForDaySubPeriod(DaySubPeriod period, DateTime today)
  {
    switch (period)
    {
      case DaySubPeriod.Today:
        return (today, today);
      case DaySubPeriod.Last3Days:
        return (today.AddDays(-2), today);
      default:
        throw new NotImplementedException($"Handling for {period} is not implemented.");
    }
  }

  private static (DateTime StartDate, DateTime EndDate) SetDateRangeForWeekSubPeriod(WeekSubPeriod period, DateTime today)
  {
    int currentDayOfWeek = (int)today.DayOfWeek;
    switch (period)
    {
      case WeekSubPeriod.ThisWeek:
        DateTime startOfWeek = today.AddDays(-currentDayOfWeek);
        return (startOfWeek, startOfWeek.AddDays(6));
      case WeekSubPeriod.LastWeek:
        DateTime startOfLastWeek = today.AddDays(-currentDayOfWeek - 7);
        return (startOfLastWeek, startOfLastWeek.AddDays(6));
      default:
        throw new NotImplementedException($"Handling for {period} is not implemented.");
    }
  }

  private static (DateTime StartDate, DateTime EndDate) SetDateRangeForMonthSubPeriod(MonthSubPeriod period, DateTime firstDayOfThisMonth)
  {
    switch (period)
    {
      case MonthSubPeriod.ThisMonth:
        DateTime endOfThisMonth = firstDayOfThisMonth.AddMonths(1).AddDays(-1);
        return (firstDayOfThisMonth, endOfThisMonth);
      case MonthSubPeriod.PreviousMonth:
        DateTime startOfPreviousMonth = firstDayOfThisMonth.AddMonths(-1);
        DateTime endOfPreviousMonth = startOfPreviousMonth.AddMonths(1).AddDays(-1);
        return (startOfPreviousMonth, endOfPreviousMonth);
      case MonthSubPeriod.Last3Months:
        DateTime startOfLast3Months = firstDayOfThisMonth.AddMonths(-2);
        DateTime endOfLast3Months = firstDayOfThisMonth.AddMonths(1).AddDays(-1);
        return (startOfLast3Months, endOfLast3Months);
      default:
        throw new NotImplementedException($"Handling for {period} is not implemented.");
    }
  }

}
