﻿using FastExpressionCompiler;
using Mapster;
using TelephoneDirectory.ContactService.Records;
using TelephoneDirectory.Data.Entities;

namespace TelephoneDirectory.ContactService;

public static class MappingConfiguration
{
    public static TypeAdapterConfig Generate()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<CreateContact, Contact>();
        config.NewConfig<Contact, GetContact>();
        config.NewConfig<Contact, GetContactDetail>();

        config.NewConfig<ContactInformation, GetContactInformation>();

        config.Compiler = exp => exp.CompileFast();
        config.Compile();

        return config;
    }
}