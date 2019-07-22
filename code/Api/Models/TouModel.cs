using System;

namespace Api.Models
{
  public class TouModel
  {
    public int MeterCode { get; set; }
    public int Serial { get; set; }
    public string PlantCode { get; set; }
    public DateTime DateTime { get; set; }
    public string FormattedDate => DateTime.ToString("ddMMyyyy");
    public string Quality { get; set; }
    public string Stream { get; set; }
    public string DataType { get; set; }
    public string FormattedDataType => DataType.Trim().Replace(" ", string.Empty);
    public decimal Energy { get; set; }
    public string Units { get; set; }
    public string Id => Controllers.Helper.GenerateId(MeterCode, FormattedDataType);
  }
}
