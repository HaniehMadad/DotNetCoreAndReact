using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Core.DynamoDb
{
  public class DbProvider : IDbProvider
  {
    private readonly IAmazonDynamoDB _dynamoDbClient;
    private static readonly string tableName = "PowerTable";

    public DbProvider(IAmazonDynamoDB dynamoDbClient)
    {
      _dynamoDbClient = dynamoDbClient;
    }
    public async Task CreateTable()
    {
      try
      {
        await CreatePowerTable();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        throw;
      }
    }

    public async Task AddNewEntry(string id, string formattedDate, decimal min, decimal max, decimal median)
    {
      var queryRequest = AddEntryRequestBuilder(id, formattedDate, min, max, median);

      await PutItemAsync(queryRequest);
    }

    public async Task<GetItemResponse> GetItem(string id, string formattedDate)
    {
      var queryRequest = GetItemRequestBuilder(id, formattedDate);
      return await GetItemAsync(queryRequest);
    }
    private async Task CreatePowerTable()
    {
      if (TableExists()) return;

      var request = new CreateTableRequest
      {
        AttributeDefinitions = new List<AttributeDefinition>
        {
          new AttributeDefinition
            {
                AttributeName = "FormattedDate",
                AttributeType = "S"
            },
            new AttributeDefinition
            {
                AttributeName = "Id",
                AttributeType = "S"
            }
        },
        KeySchema = new List<KeySchemaElement>
          {
              new KeySchemaElement
              {
                  AttributeName = "FormattedDate",
                  KeyType = "HASH"
              },
              new KeySchemaElement
              {
                  AttributeName = "Id",
                  KeyType = "Range"
              }
          },
        ProvisionedThroughput = new ProvisionedThroughput
        {
          ReadCapacityUnits = 5,
          WriteCapacityUnits = 5
        },
        TableName = tableName
      };

      var response = await _dynamoDbClient.CreateTableAsync(request);

      //WaitUntilTableReady(tableName);
    }

    private bool TableExists() => _dynamoDbClient.ListTablesAsync().Result.TableNames.Any(tn => tn == tableName);
    public void WaitUntilTableReady(string tableName)
    {
      string status = null;

      do
      {
        Thread.Sleep(5000);
        try
        {
          var res = _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
          {
            TableName = tableName
          });

          status = res.Result.Table.TableStatus;
        }
        catch (ResourceNotFoundException)
        {

        }

      } while (status != "ACTIVE");
      {
        Console.WriteLine("Table Created Successfully");
      }
    }

    private PutItemRequest AddEntryRequestBuilder(string id, string formattedDate, decimal min, decimal max, decimal median)
    {
      var item = new Dictionary<string, AttributeValue>
        {
          {"FormattedDate", new AttributeValue {S = formattedDate}},
          {"Id", new AttributeValue {S = id}},
          {"Minimum", new AttributeValue {S = min.ToString("F")}},
          {"Maximum", new AttributeValue {S = max.ToString("F")}},
          {"Median", new AttributeValue {S = median.ToString("F")}}
        };

      return new PutItemRequest
      {
        TableName = tableName,
        Item = item
      };
    }

    private async Task PutItemAsync(PutItemRequest request)
    {
      await _dynamoDbClient.PutItemAsync(request);

    }

    private GetItemRequest GetItemRequestBuilder(string id, string formattedDate)
    {
      var key = new Dictionary<string, AttributeValue>
        {
          {"FormattedDate", new AttributeValue {S = formattedDate}},
          {"Id", new AttributeValue {S = id}}
        };

      return new GetItemRequest
      {
        TableName = tableName,
        Key = key
      };
    }
    private async Task<GetItemResponse> GetItemAsync(GetItemRequest request)
    {
      return await _dynamoDbClient.GetItemAsync(request);
    }

  }
}
