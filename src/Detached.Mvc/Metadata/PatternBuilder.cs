using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Detached.Mvc.Metadata
{
    public class PatternBuilder
    {
        const string DEFAULT_SEPARATOR = ".";
        StringBuilder pattern = new StringBuilder();

        public PatternBuilder Group(string separator = @"\.", [CallerMemberName]string name = null)
        {
            if (pattern.Length > 1)
                pattern.Append(separator);

            pattern.Append("(?<");
            pattern.Append(name.ToLower());
            pattern.Append(@">[\w]+)");
            return this;
        }

        public PatternBuilder OptionalGroup(string separator = @"\.", [CallerMemberName]string name = null)
        {
            pattern.Append("(?:");
            pattern.Append(separator);
            pattern.Append("(?<");
            pattern.Append(name.ToLower());
            pattern.Append(@">[\w]+))?");
            return this;
        }

        public PatternBuilder Literal(string literal)
        {
            pattern.Append(@"\b");
            pattern.Append(literal);
            pattern.Append(@"\b");
            return this;
        }

        public PatternBuilder Const(string literal)
        {
            pattern.Append(literal);
            return this;
        }

        public PatternBuilder IgnoreGroup(string separator = @"\.")
        {
            if (pattern.Length > 1)
                pattern.Append(separator);

            pattern.Append(@"(?:[\w]+)");
            return this;
        }

        public PatternBuilder EndOfString()
        {
            pattern.Append("$");
            return this;
        }

        public PatternBuilder StartOfString()
        {
            pattern.Append("^");
            return this;
        }

        public PatternBuilder Application() => Group();

        public PatternBuilder Module() => Group();

        public PatternBuilder Feature() => Group();

        public PatternBuilder Class() => Group();

        public PatternBuilder Property() => Group(@"\+");

        public PatternBuilder MetaProperty() => Group(@"\#");

        public PatternBuilder OptionalProperty() => OptionalGroup(@"\+", nameof(Property));

        public PatternBuilder OptionalMetaProperty() => OptionalGroup(@"\#", nameof(MetaProperty));

        public override string ToString()
        {
            return pattern.ToString();
        }

        public static implicit operator string(PatternBuilder pattern)
        {
            return pattern.ToString();
        }

        public static implicit operator Regex(PatternBuilder pattern)
        {
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }
}
