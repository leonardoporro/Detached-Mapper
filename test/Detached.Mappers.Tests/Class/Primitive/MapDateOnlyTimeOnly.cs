using Detached.Mappers.Types;
using System;
using Xunit;

namespace Detached.Mappers.Tests.Class.Primitive
{
    // https://github.com/leonardoporro/Detached-Mapper/issues/120
    public class MapDateOnlyTimeOnly
    {
        Mapper mapper = new Mapper();

        [Fact]
        public void dateonly_and_timeonly_are_primitives()
        {
            Assert.True(mapper.Options.IsPrimitive(typeof(DateOnly)));
            Assert.True(mapper.Options.IsPrimitive(typeof(TimeOnly)));
            Assert.True(mapper.Options.GetType(typeof(DateOnly)).IsPrimitive());
            Assert.True(mapper.Options.GetType(typeof(TimeOnly)).IsPrimitive());
        }

        [Fact]
        public void map_dateonly_and_timeonly_members()
        {
            var dto = new EventDto
            {
                Day = new DateOnly(2025, 11, 12),
                OptionalDay = new DateOnly(2026, 1, 2),
                Time = new TimeOnly(10, 30),
                OptionalTime = null
            };

            Event entity = mapper.Map<EventDto, Event>(dto);

            Assert.Equal(dto.Day, entity.Day);
            Assert.Equal(dto.OptionalDay, entity.OptionalDay);
            Assert.Equal(dto.Time, entity.Time);
            Assert.Null(entity.OptionalTime);
        }

        [Fact]
        public void map_dateonly_directly()
        {
            DateOnly value = new DateOnly(2025, 11, 12);

            Assert.Equal(value, mapper.Map<DateOnly, DateOnly>(value));
            Assert.Equal(value, mapper.Map<DateOnly?, DateOnly>(value));
            Assert.Equal(value, mapper.Map<DateOnly, DateOnly?>(value));
        }

        public class EventDto
        {
            public DateOnly Day { get; set; }

            public DateOnly? OptionalDay { get; set; }

            public TimeOnly Time { get; set; }

            public TimeOnly? OptionalTime { get; set; }
        }

        public class Event
        {
            public DateOnly Day { get; set; }

            public DateOnly? OptionalDay { get; set; }

            public TimeOnly Time { get; set; }

            public TimeOnly? OptionalTime { get; set; } = new TimeOnly(1, 1);
        }
    }
}
