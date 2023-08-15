namespace Detached.Mappers.Tests.Contrib.ScottSoftware;

public class Work
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Language { get; set; }

    public int CreatorId { get; set; }

    public Creator Creator { get; set; }
}