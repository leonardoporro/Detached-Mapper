﻿namespace Detached.EntityFramework.Tests.Extensions
{
    public static class StringExtensions
    {
        public static string Json(string json)
            => json.Replace("'", "\"");
    }
}
