using System;
using System.Globalization;

namespace Api.Controllers
{
  public static class Helper
  {
    public static string GenerateId(int meterCode, string formattedDataType)
    {
      return meterCode.ToString().Trim() + formattedDataType;
    }
    public static bool ValidateDateString(string date)
    {
      return date.Length == 8;
    }

    public static string FormatDateTime(string dateTime) => Convert.ToDateTime(dateTime, new CultureInfo("es-ES")).ToString("ddMMyyyy");


  }
}
