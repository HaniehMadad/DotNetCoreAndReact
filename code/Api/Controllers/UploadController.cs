using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using Api.ResponseModels;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using Core.DynamoDb;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class UploadController : ControllerBase
  {
    private IDbProvider _dbProvider { get; }
    public UploadController(IDbProvider dbProvider)
    {
      _dbProvider = dbProvider;
    }
    [HttpGet]
    [Route("createTable")]
    public async Task<ActionResult> CreateTable()
    {
      try
      {
        await _dbProvider.CreateTable();
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPost]
    [Route("uploadLp")]
    public async Task<ActionResult<string>> UploadLp()
    {
      try
      {
        using (StreamReader sr = new StreamReader(Request.Body, Encoding.UTF8))
        {
          CsvReader csvReader = new CsvReader(sr);
          var records = csvReader.GetRecords<LpModel>().ToList();
          var results = records.GroupBy(
            r => new { r.Id, r.FormattedDate },
            r => r.DataValue,
            (key, dataValues) => new
            {
              Key = key,
              Count = dataValues.Count(),
              Min = dataValues.Min(),
              Max = dataValues.Max()
            }).ToList();

          foreach (var result in results)
          {
            await _dbProvider.AddNewEntry(result.Key.Id, result.Key.FormattedDate, result.Min, result.Max, ((result.Min + result.Max) / 2));
          }

          return results.Count().ToString();
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPost]
    [Route("uploadTou")]
    public async Task<ActionResult<string>> UploadTou()
    {
      try
      {
        using (StreamReader sr = new StreamReader(Request.Body, Encoding.UTF8))
        {
          CsvReader csvReader = new CsvReader(sr);
          var records = csvReader.GetRecords<TouModel>().ToList();
          var results = records.GroupBy(
           r => new { r.Id, r.FormattedDate },
           r => r.Energy,
           (key, energies) => new
           {
             Key = key,
             Count = energies.Count(),
             Min = energies.Min(),
             Max = energies.Max()
           }).ToList();

          foreach (var result in results)
          {
            await _dbProvider.AddNewEntry(result.Key.Id, result.Key.FormattedDate, result.Min, result.Max, ((result.Min + result.Max) / 2));
          }

          return results.Count().ToString();
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

  }
}
