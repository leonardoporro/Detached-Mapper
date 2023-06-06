using System;
using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.Bug18;

[Owned]
public class OwnedOne
{
    public DateTime DateTime { get; set; }
}