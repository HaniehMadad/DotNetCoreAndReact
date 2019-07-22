using Api.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Core.DynamoDb;
using System.Threading.Tasks;
using System;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  public class PowerController : ControllerBase
  {
    private readonly IDbProvider _dbProvider;
    public PowerController(IDbProvider dbProvider)
    {
      _dbProvider = dbProvider;
    }
    /// <summary>
    /// Power Consumption based on Date (no time), Meter and DataType
    /// </summary>
    /// <param name="date">date length is 8 and it’s format is (DDMMYYYY). e.g. 09012018
    /// </param>
    /// <param name="id">id is a string and it’s format is (MeterCodeDataType).e.g 214612534ExportWhTotal
    /// Meter Code is a string. e.g: 214612534
    /// DatatType is a string without white space. e.g. ExportWhTotal
    /// </param>
    /// <returns></returns>
    /// <response code="200">Success- Details obtained.</response>
    /// <response code="404">Not Found.</response>
    /// <response code="400">Bad Request. Make sure the date and id's format is correct.</response>
    /// <response code="500">Internal server error.</response>

    [HttpGet]
    [Route("{date}/{id}")]
    public async Task<ActionResult<PowerResponse>> Get(string date, string id)
    {
      if (!Helper.ValidateDateString(date) || String.IsNullOrEmpty(id)) return BadRequest("Make sure date format is ddMMyyyy and id is not empty");
      var result = await _dbProvider.GetItem(id.Trim(), date.Trim());
      if (result.Item.Count > 0)
      {
        return new PowerResponse
        {
          Maximum = result.Item["Maximum"].S,
          Minimum = result.Item["Minimum"].S,
          Median = result.Item["Median"].S
        };
      }
      return NotFound();
    }
  }
}
