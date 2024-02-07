namespace Detached.Mappers.Tests.Contrib.ScottSoftware.Model;

public class Work
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Language { get; set; }

    public Creator Creator { get; set; }

    public int? CreatorId { get; set; }
}