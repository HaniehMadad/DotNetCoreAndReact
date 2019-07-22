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
  }
}
