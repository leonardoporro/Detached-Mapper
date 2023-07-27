using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Detached.Mappers.EntityFramework.Tests.Model
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class TagListConverter : ValueConverter<List<Tag>, string>
    {
        public TagListConverter() 
            : base(toProvider, fromProvider, null)
        {
        }

        static Expression<Func<List<Tag>, string>> toProvider = (tags) => string.Join(", ", tags.Select(t => t.Name));

        static Expression<Func<string, List<Tag>>> fromProvider = (tags) =>
            tags.Split(", ", StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries)
                .Select(n => new Tag { Name = n })
                .ToList();
    }
}
