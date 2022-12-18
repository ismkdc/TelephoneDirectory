using FastExpressionCompiler;
using Mapster;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.ReportAPI.Records;

namespace TelephoneDirectory.ReportAPI;

public static class MappingConfiguration
{
    public static TypeAdapterConfig Generate()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<Report, GetReport>();

        config.Compiler = exp => exp.CompileFast();
        config.Compile();

        return config;
    }
}