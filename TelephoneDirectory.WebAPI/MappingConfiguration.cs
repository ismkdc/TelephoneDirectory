using FastExpressionCompiler;
using Mapster;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.WebAPI.Records;

namespace TelephoneDirectory.WebAPI;

public static class MappingConfiguration
{
    public static TypeAdapterConfig Generate()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<CreateContact, Contact>();
        config.NewConfig<Contact, GetContact>();
        config.NewConfig<Contact, GetContactDetail>();

        config.NewConfig<ContactInformation, GetContactInformation>();

        config.NewConfig<Report, GetReport>();
        config.NewConfig<Report, GetReportDetail>();

        config.Compiler = exp => exp.CompileFast();
        config.Compile();

        return config;
    }
}