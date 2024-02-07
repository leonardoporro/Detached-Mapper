using System.Collections.ObjectModel;

namespace Detached.Mappers.Tests.Contrib.ScottSoftware.Model;

public class Creator
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public DateTime Born { get; set; }

    public DateTime Died { get; set; }

    public string? PrimaryLanguage { get; set; }

    public Collection<Work> Works { get; set; }
}