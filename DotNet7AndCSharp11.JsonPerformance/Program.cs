using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FizzWare.NBuilder;
using System.Text.Json;
using System.Text.Json.Serialization;
using NewtonsoftSerializer = Newtonsoft.Json.JsonConvert;

BenchmarkRunner.Run<JsonBenchmarks>();


public partial class JsonBenchmarks
{
    private readonly DataPoint[] _data;
    private string _newtonsoftJson;
    private string _systemTextJson;

    public JsonBenchmarks()
    {
        _data = Builder<DataPoint>.CreateListOfSize(2)
            .All()
            .With(p => p.Id = Guid.NewGuid())
            .With(p => p.Title = Faker.Identification.UkNationalInsuranceNumber())
            .With(p => p.Description = Faker.Lorem.Sentence(100))
            .With(p => p.LeftData = GenerateDataPoint(0))
            .With(p => p.RightData = GenerateDataPoint(0))
            .Build().ToArray();

        _newtonsoftJson = NewtonsoftSerializer.SerializeObject(_data);
        _systemTextJson = JsonSerializer.Serialize(_data);
    }


    [Benchmark(Description = "NewtonsoftJsonSerialize")]
    public void NewtonsoftJsonSerialize()
    {
        _ = NewtonsoftSerializer.SerializeObject(_data);
    }

    [Benchmark(Description = "NewtonsoftJsonDeserialize")]
    public void NewtonsoftJsonDeserialize()
    {
        _ = NewtonsoftSerializer.DeserializeObject<DataPoint[]>(_newtonsoftJson);
    }


    [Benchmark(Description = "SystemTextJsonSerialize")]
    public void SystemTextJsonSerialize()
    {
        _ = JsonSerializer.Serialize(_data);
    }

    [Benchmark(Description = "SystemTextJsonDeserialize")]
    public void SystemTextJsonDeserialize()
    {
        _ = JsonSerializer.Deserialize<DataPoint[]>(_systemTextJson);
    }


    [Benchmark(Description = "SourceGeneratorJsonSerialize")]
    public void SourceGeneratorJsonSerialize()
    {
        _ = JsonSerializer.Serialize(_data, DataPointContext.Default.DataPointArray);
    }

    [Benchmark(Description = "SourceGeneratorJsonDeserialize")]
    public void SourceGeneratorJsonDeserialize()
    {
        _ = JsonSerializer.Deserialize(_systemTextJson, DataPointContext.Default.DataPointArray);
    }


    static DataPoint GenerateDataPoint(int level)
    {
        if (level < 10)
        {
            return new DataPoint
            {
                Id = Guid.NewGuid(),
                Title = Faker.Identification.UkNationalInsuranceNumber(),
                Description = Faker.Lorem.Sentence(10),
                LeftData = GenerateDataPoint(level + 1),
                RightData = GenerateDataPoint(level + 1)
            };
        }

        return null;
    }
}





public partial class DataPoint
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DataPoint LeftData { get; set; }
    public DataPoint RightData { get; set; }
}

[JsonSerializable(typeof(DataPoint))]
[JsonSerializable(typeof(DataPoint[]))]
internal partial class DataPointContext : JsonSerializerContext
{

}