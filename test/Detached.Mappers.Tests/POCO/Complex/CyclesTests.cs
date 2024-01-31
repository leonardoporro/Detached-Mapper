using System.Collections.Generic;
using Xunit;

namespace Detached.Mappers.Tests.POCO.Complex
{
    public class CyclesTests
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void map_direct_cycle()
        {
            DirectCycleDto dto = new DirectCycleDto();
            dto.Id = 1;
            dto.Name = "cycledto";
            dto.Parent = dto;

            var result = mapper.Map<DirectCycleDto, DirectCycle>(dto);

            Assert.Equal(1, dto.Id);
            Assert.Equal("cycledto", dto.Name);
            Assert.Equal(result, result.Parent);
        }

        [Fact]
        public void map_indirect_cycle()
        {
            RootDto root = new RootDto();
            root.Id = 1;
            root.Name = "root";
            root.Items = new List<ItemDto>();
            root.Items.Add(new ItemDto
            {
                Id = 1,
                Name = "item 1",
                Parent = root
            });
            root.Items.Add(new ItemDto
            {
                Id = 2,
                Name = "item 2",
                Parent = root
            });

            Root result = mapper.Map<RootDto, Root>(root);

            Assert.Equal(1, result.Id);
            Assert.Equal("root", result.Name);
            Assert.Equal(1, result.Items[0].Id);
            Assert.Equal("item 1", result.Items[0].Name);
            Assert.Equal(result, result.Items[0].Parent);
            Assert.Equal(2, result.Items[1].Id);
            Assert.Equal("item 2", result.Items[1].Name);
            Assert.Equal(result, result.Items[1].Parent);
        }

        public class DirectCycleDto
        {
            public DirectCycleDto Parent { get; set; }

            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class DirectCycle
        {
            public DirectCycle Parent { get; set; }

            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class RootDto
        { 
            public int Id { get; set; }

            public string Name { get; set; }

            public List<ItemDto> Items { get; set; }
        }

        public class Root
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public List<Item> Items { get; set; }
        }

        public class ItemDto
        {
            public RootDto Parent { get; set; }

            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class Item
        {
            public Root Parent { get; set; }

            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}