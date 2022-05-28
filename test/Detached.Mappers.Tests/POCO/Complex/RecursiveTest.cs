using Xunit;

namespace Detached.Mappers.Tests.POCO.Complex
{
    public class RecursiveTest
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void map_recursive_types()
        {
            CycleSource sourceObj = new CycleSource
            {
                Text = "sample text",
                DirectCycle = new CycleSource
                {
                    Text = "sample cycle",
                    DirectCycle = new CycleSource
                    {
                        Text = "sample cycle 2",
                        DirectCycle = new CycleSource
                        {
                            Text = "sample cycle 3",
                            DirectCycle = new CycleSource
                            {
                                Text = "sample cycle 4"
                            }
                        }
                    }
                }
            };

            CycleTarget mappedObj = mapper.Map<CycleSource, CycleTarget>(sourceObj);
            Assert.Equal("sample text", mappedObj.Text);
            Assert.Equal("sample cycle", mappedObj.DirectCycle.Text);
            Assert.Equal("sample cycle 2", mappedObj.DirectCycle.DirectCycle.Text);
            Assert.Equal("sample cycle 3", mappedObj.DirectCycle.DirectCycle.DirectCycle.Text);
            Assert.Equal("sample cycle 4", mappedObj.DirectCycle.DirectCycle.DirectCycle.DirectCycle.Text);
        }

        [Fact]
        public void MapIndirectCycle()
        {
            var source = new CycleA
            {
                Name = "A1",
                B = new CycleB
                {
                    Name = "B1",
                    C = new CycleC
                    {
                        Name = "C1",
                        A = new CycleA
                        {
                            Name = "A2"
                        }
                    }
                }
            };

            CycleA result = mapper.Map(source, new CycleA());
            Assert.Equal("A1", result.Name);
            Assert.Equal("B1", result.B.Name);
            Assert.Equal("C1", result.B.C.Name);
            Assert.Equal("A2", result.B.C.A.Name);
        }


        [Fact]
        public void map_indirect_recursive_types()
        {
            var source = new
            {
                Name = "A1",
                B = new
                {
                    Name = "B1",
                    C = new
                    {
                        Name = "C1",
                        A = new
                        {
                            Name = "A2"
                        }
                    }
                }
            };

            CycleA result = mapper.Map(source, new CycleA());
            Assert.Equal("A1", result.Name);
            Assert.Equal("B1", result.B.Name);
            Assert.Equal("C1", result.B.C.Name);
            Assert.Equal("A2", result.B.C.A.Name);
        }

        public class CycleTarget
        {
            public string Text { get; set; }

            public CycleTarget DirectCycle { get; set; }
        }

        public class CycleSource
        {
            public string Text { get; set; }

            public CycleSource DirectCycle { get; set; }
        }

        public class CycleA
        {
            public string Name { get; set; }

            public CycleB B { get; set; }
        }

        public class CycleB
        {
            public string Name { get; set; }

            public CycleC C { get; set; }
        }

        public class CycleC
        {
            public string Name { get; set; }

            public CycleA A { get; set; }
        }
    }
}
