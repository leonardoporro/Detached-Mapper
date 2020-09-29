using Detached.Mappers.EntityFramework.Queries;
using Detached.Mappers;
using System;
using System.Text.Json;

namespace Detached.Mappers.EntityFramework
{
    public class DetachedDbContextDependencies
    {
        public DetachedDbContextDependencies(Type dbContextType, Mapper mapper, JsonSerializerOptions serializerOptions, QueryProvider queryProvider)
        {
            Mapper = mapper;
            SerializerOptions = serializerOptions;
            QueryProvider = queryProvider;
            DbContextType = dbContextType;
        }

        public Type DbContextType { get; }

        public Mapper Mapper { get; }

        public JsonSerializerOptions SerializerOptions { get; }

        public QueryProvider QueryProvider { get; }
    }
}