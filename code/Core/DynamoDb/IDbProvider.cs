using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace Core.DynamoDb
{
  public interface IDbProvider
  {
    Task CreateTable();
    Task AddNewEntry(string id, string formattedDate, decimal min, decimal max, decimal median);
    Task<GetItemResponse> GetItem(string id, string formattedDate);
  }
}
