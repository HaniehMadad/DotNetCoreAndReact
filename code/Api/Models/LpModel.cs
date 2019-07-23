using CsvHelper.Configuration.Attributes;
using Api.Controllers;

namespace Api.Models
{
  public class LpModel
  {
    [Name("MeterPoint Code")]
    public int MeterPointCode { get; set; }
    [Name("Serial Number")]
    public int SerialNumber { get; set; }
    [Name("Plant Code")]
    public string PlantCode { get; set; }
    [Name("Date/Time")]
    public string DateTime { get; set; }
    public string FormattedDate => Helper.FormatDateTime(DateTime);

    [Name("Data Type")]
    public string DataType { get; set; }
    public string FormattedDataType => DataType.Trim().Replace(" ", string.Empty);

    [Name("Data Value")]
    public decimal DataValue { get; set; }
    [Name("Units")]
    public string Units { get; set; }
    public string Id => Controllers.Helper.GenerateId(MeterPointCode, FormattedDataType);
  }
}
