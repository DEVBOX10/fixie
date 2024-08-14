﻿using System.Runtime.CompilerServices;
using System.Text;
using static System.Environment;

namespace Fixie.Tests.Assertions;

public class AssertionTests
{
    static readonly string Line = NewLine + NewLine;

    public void ShouldAssertBools()
    {
        true.ShouldBe(true);
        false.ShouldBe(false);

        Contradiction(true, x => x.ShouldBe(false), "x should be false but was true");
        Contradiction(false, x => x.ShouldBe(true), "x should be true but was false");
    }

    public void ShouldAssertIntegralNumbers()
    {
        sbyte.MinValue.ShouldBe(sbyte.MinValue);
        sbyte.MaxValue.ShouldBe(sbyte.MaxValue);
        Contradiction((sbyte)1, x => x.ShouldBe((sbyte)2), "x should be 2 but was 1");
        
        byte.MinValue.ShouldBe(byte.MinValue);
        byte.MaxValue.ShouldBe(byte.MaxValue);
        Contradiction((byte)2, x => x.ShouldBe((byte)3), "x should be 3 but was 2");
        
        short.MinValue.ShouldBe(short.MinValue);
        short.MaxValue.ShouldBe(short.MaxValue);
        Contradiction((short)3, x => x.ShouldBe((short)4), "x should be 4 but was 3");
        
        ushort.MinValue.ShouldBe(ushort.MinValue);
        ushort.MaxValue.ShouldBe(ushort.MaxValue);
        Contradiction((ushort)4, x => x.ShouldBe((ushort)5), "x should be 5 but was 4");

        int.MinValue.ShouldBe(int.MinValue);
        int.MaxValue.ShouldBe(int.MaxValue);
        Contradiction((int)5, x => x.ShouldBe((int)6), "x should be 6 but was 5");

        uint.MinValue.ShouldBe(uint.MinValue);
        uint.MaxValue.ShouldBe(uint.MaxValue);
        Contradiction((uint)6, x => x.ShouldBe((uint)7), "x should be 7 but was 6");
        
        long.MinValue.ShouldBe(long.MinValue);
        long.MaxValue.ShouldBe(long.MaxValue);
        Contradiction((long)7, x => x.ShouldBe((long)8), "x should be 8 but was 7");

        ulong.MinValue.ShouldBe(ulong.MinValue);
        ulong.MaxValue.ShouldBe(ulong.MaxValue);
        Contradiction((ulong)8, x => x.ShouldBe((ulong)9), "x should be 9 but was 8");

        nint.MinValue.ShouldBe(nint.MinValue);
        nint.MaxValue.ShouldBe(nint.MaxValue);
        Contradiction((nint)9, x => x.ShouldBe((nint)10), "x should be 10 but was 9");

        nuint.MinValue.ShouldBe(nuint.MinValue);
        nuint.MaxValue.ShouldBe(nuint.MaxValue);
        Contradiction((nuint)10, x => x.ShouldBe((nuint)11), "x should be 11 but was 10");
    }

    public void ShouldAssertFractionalNumbers()
    {
        decimal.MinValue.ShouldBe(decimal.MinValue);
        decimal.MaxValue.ShouldBe(decimal.MaxValue);
        Contradiction((decimal)1, x => x.ShouldBe((decimal)2), "x should be 2 but was 1");

        double.MinValue.ShouldBe(double.MinValue);
        double.MaxValue.ShouldBe(double.MaxValue);
        Contradiction((double)2, x => x.ShouldBe((double)3), "x should be 3 but was 2");

        float.MinValue.ShouldBe(float.MinValue);
        float.MaxValue.ShouldBe(float.MaxValue);
        Contradiction((float)3, x => x.ShouldBe((float)4), "x should be 4 but was 3");
    }

    public void ShouldAssertEquatables()
    {
        HttpMethod.Post.ShouldBe(HttpMethod.Post);
        Contradiction(HttpMethod.Post, x => x.ShouldBe(HttpMethod.Get), "x should be GET but was POST");
    }

    public void ShouldAssertChars()
    {
        'a'.ShouldBe('a');
        '☺'.ShouldBe('☺');
        Contradiction('a', x => x.ShouldBe('z'), "x should be 'z' but was 'a'");
        
        // Escape Sequence: Null
        '\u0000'.ShouldBe('\0');
        '\0'.ShouldBe('\0');
        Contradiction('\n', x => x.ShouldBe('\0'), "x should be '\\0' but was '\\n'");

        // Escape Sequence: Alert
        '\u0007'.ShouldBe('\a');
        '\a'.ShouldBe('\a');
        Contradiction('\n', x => x.ShouldBe('\a'), "x should be '\\a' but was '\\n'");

        // Escape Sequence: Backspace
        '\u0008'.ShouldBe('\b');
        '\b'.ShouldBe('\b');
        Contradiction('\n', x => x.ShouldBe('\b'), "x should be '\\b' but was '\\n'");

        // Escape Sequence: Horizontal tab
        '\u0009'.ShouldBe('\t');
        '\t'.ShouldBe('\t');
        Contradiction('\n', x => x.ShouldBe('\t'), "x should be '\\t' but was '\\n'");

        // Escape Sequence: New line
        '\u000A'.ShouldBe('\n');
        '\n'.ShouldBe('\n');
        Contradiction('\r', x => x.ShouldBe('\n'), "x should be '\\n' but was '\\r'");

        // Escape Sequence: Vertical tab
        '\u000B'.ShouldBe('\v');
        '\v'.ShouldBe('\v');
        Contradiction('\n', x => x.ShouldBe('\v'), "x should be '\\v' but was '\\n'");

        // Escape Sequence: Form feed
        '\u000C'.ShouldBe('\f');
        '\f'.ShouldBe('\f');
        Contradiction('\n', x => x.ShouldBe('\f'), "x should be '\\f' but was '\\n'");

        // Escape Sequence: Carriage return
        '\u000D'.ShouldBe('\r');
        '\r'.ShouldBe('\r');
        Contradiction('\n', x => x.ShouldBe('\r'), "x should be '\\r' but was '\\n'");

        // TODO: Applicable in C# 13
        // Escape Sequence: Escape
        // '\u001B'.ShouldBe('\e');
        // '\e'.ShouldBe('\e');
        // Contradiction('\n', x => x.ShouldBe('\e'), "x should be '\\e' but was '\\n'");

        // Literal Space
        ' '.ShouldBe(' ');
        '\u0020'.ShouldBe(' ');
        Contradiction('\n', x => x.ShouldBe(' '), "x should be ' ' but was '\\n'");

        // Escape Sequence: Double quote
        '\u0022'.ShouldBe('\"');
        '\"'.ShouldBe('\"');
        Contradiction('\n', x => x.ShouldBe('\"'), "x should be '\\\"' but was '\\n'");

        // Escape Sequence: Single quote
        '\u0027'.ShouldBe('\'');
        '\''.ShouldBe('\'');
        Contradiction('\n', x => x.ShouldBe('\''), "x should be '\\'' but was '\\n'");

        // Escape Sequence: Backslash
        '\u005C'.ShouldBe('\\');
        '\\'.ShouldBe('\\');
        Contradiction('\n', x => x.ShouldBe('\\'), "x should be '\\\\' but was '\\n'");

        foreach (var c in UnicodeEscapedCharacters())
        {
            c.ShouldBe(c);
            Contradiction('a', x => x.ShouldBe(c), $"x should be '\\u{(int)c:X4}' but was 'a'");
        }
    }

    public void ShouldAssertStrings()
    {
        "a☺".ShouldBe("a☺");
        Contradiction("a☺", x => x.ShouldBe("z☺"),
            """
            x should be "z☺" but was "a☺"
            """);

        ((string?)null).ShouldBe(null);
        Contradiction("abc", x => x.ShouldBe(null),
            """
            x should be null but was "abc"
            """);
        Contradiction(((string?)null), x => x.ShouldBe("abc"),
            """
            x should be "abc" but was null
            """);
        
        "\u0020 ".ShouldBe("  ");
        Contradiction("abc", x => x.ShouldBe("\u0020 "),
            """
            x should be "  " but was "abc"
            """);

        "\u0000\0 \u0007\a \u0008\b \u0009\t \u000A\n \u000D\r".ShouldBe("\0\0 \a\a \b\b \t\t \n\n \r\r");
        Contradiction("abc", x => x.ShouldBe("\0\a\b\t\n\r"),
            """
            x should be "\0\a\b\t\n\r" but was "abc"
            """);

        // TODO: In C# 13, include \u001B\e becoming \e\e
        "\u000C\f \u000B\v \u0022\" \u0027\' \u005C\\".ShouldBe("\f\f \v\v \"\" \'\' \\\\");
        // TODO: In C# 13, include \e being preserved.
        Contradiction("abc", x => x.ShouldBe("\f\v\"\'\\"),
            """
            x should be "\f\v\"\'\\" but was "abc"
            """);

        foreach (var c in UnicodeEscapedCharacters())
        {
            var s = c.ToString();

            s.ShouldBe(s);
            Contradiction("a", x => x.ShouldBe(s),
                $"""
                 x should be "\u{(int)c:X4}" but was "a"
                 """);
        }
    }

    public void ShouldAssertMultilineStrings()
    {
        var original = new StringBuilder()
            .AppendLine("Line 1")
            .AppendLine("Line 2")
            .AppendLine("Line 3")
            .Append("Line 4")
            .ToString();

        var altered = new StringBuilder()
            .AppendLine("Line 1")
            .AppendLine("Line 2 Altered")
            .AppendLine("Line 3")
            .Append("Line 4")
            .ToString();

        var mixedLineEndings = "\r \n \r\n \n \r";

        original.ShouldBe(original);
        altered.ShouldBe(altered);

        Contradiction(original, x => x.ShouldBe(altered),
            new StringBuilder()
                .AppendLine("x should be")
                .AppendLine("\t\"\"\"")
                .AppendLine("\tLine 1")
                .AppendLine("\tLine 2 Altered")
                .AppendLine("\tLine 3")
                .AppendLine("\tLine 4")
                .AppendLine("\t\"\"\"")
                .AppendLine()
                .AppendLine("but was")
                .AppendLine("\t\"\"\"")
                .AppendLine("\tLine 1")
                .AppendLine("\tLine 2")
                .AppendLine("\tLine 3")
                .AppendLine("\tLine 4")
                .Append("\t\"\"\"")
                .ToString());

        Contradiction(original, x => x.ShouldBe(mixedLineEndings),
            new StringBuilder()
                .AppendLine("x should be")
                .AppendLine("\t\"\\r \\n \\r\\n \\n \\r\"")
                .AppendLine()
                .AppendLine("but was")
                .AppendLine("\t\"\"\"")
                .AppendLine("\tLine 1")
                .AppendLine("\tLine 2")
                .AppendLine("\tLine 3")
                .AppendLine("\tLine 4")
                .Append("\t\"\"\"")
                .ToString());

        Contradiction(mixedLineEndings, x => x.ShouldBe(original),
            new StringBuilder()
                .AppendLine("x should be")
                .AppendLine("\t\"\"\"")
                .AppendLine("\tLine 1")
                .AppendLine("\tLine 2")
                .AppendLine("\tLine 3")
                .AppendLine("\tLine 4")
                .AppendLine("\t\"\"\"")
                .AppendLine()
                .AppendLine("but was")
                .Append("\t\"\\r \\n \\r\\n \\n \\r\"")
                .ToString());
    }

    public void ShouldAssertTypes()
    {
        typeof(int).ShouldBe(typeof(int));
        typeof(char).ShouldBe(typeof(char));
        Contradiction(typeof(Utility), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(Fixie.Tests.Utility)");
        Contradiction(typeof(bool), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(bool)");
        Contradiction(typeof(sbyte), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(sbyte)");
        Contradiction(typeof(byte), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(byte)");
        Contradiction(typeof(short), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(short)");
        Contradiction(typeof(ushort), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(ushort)");
        Contradiction(typeof(int), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(int)");
        Contradiction(typeof(uint), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(uint)");
        Contradiction(typeof(long), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(long)");
        Contradiction(typeof(ulong), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(ulong)");
        Contradiction(typeof(nint), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(nint)");
        Contradiction(typeof(nuint), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(nuint)");
        Contradiction(typeof(decimal), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(decimal)");
        Contradiction(typeof(double), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(double)");
        Contradiction(typeof(float), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(float)");
        Contradiction(typeof(char), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(char)");
        Contradiction(typeof(string), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(string)");
        Contradiction(typeof(object), x => x.ShouldBe(typeof(AssertionTests)), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(object)");

        1.ShouldBe<int>();
        'A'.ShouldBe<char>();
        Exception exception = new DivideByZeroException();
        DivideByZeroException typedException = exception.ShouldBe<DivideByZeroException>();
        Contradiction(new StubReport(), x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(Fixie.Tests.StubReport)");
        Contradiction(true, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(bool)");
        Contradiction((sbyte)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(sbyte)");
        Contradiction((byte)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(byte)");
        Contradiction((short)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(short)");
        Contradiction((ushort)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(ushort)");
        Contradiction((int)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(int)");
        Contradiction((uint)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(uint)");
        Contradiction((long)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(long)");
        Contradiction((ulong)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(ulong)");
        Contradiction((nint)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(nint)");
        Contradiction((nuint)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(nuint)");
        Contradiction((decimal)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(decimal)");
        Contradiction((double)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(double)");
        Contradiction((float)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(float)");
        Contradiction((char)1, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(char)");
        Contradiction("A", x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(string)");
        Contradiction(new object(), x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was typeof(object)");
        Contradiction((Exception?)null, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was null");
        Contradiction((AssertionTests?)null, x => x.ShouldBe<AssertionTests>(), "x should be typeof(Fixie.Tests.Assertions.AssertionTests) but was null");
    }

    public void ShouldAssertObjects()
    {
        var objectA = new SampleA();
        var objectB = new SampleB();

        ((object?)null).ShouldBe(((object?)null));
        objectA.ShouldBe(objectA);
        objectB.ShouldBe(objectB);

        Contradiction((object?)null, x => x.ShouldBe(objectA),
            "x should be Fixie.Tests.Assertions.AssertionTests+SampleA but was null");
        Contradiction(objectB, x => x.ShouldBe((object?)null),
            "x should be null but was Fixie.Tests.Assertions.AssertionTests+SampleB");
        Contradiction(objectB, x => x.ShouldBe(objectA),
            "x should be Fixie.Tests.Assertions.AssertionTests+SampleA but was Fixie.Tests.Assertions.AssertionTests+SampleB");
        Contradiction(objectA, x => x.ShouldBe(objectB),
            "x should be Fixie.Tests.Assertions.AssertionTests+SampleB but was Fixie.Tests.Assertions.AssertionTests+SampleA");
    }

    public void ShouldAssertLists()
    {
        new int[]{}.ShouldBe([]);

        Contradiction(new[] { 0 }, x => x.ShouldBe([]),
            """
            x should be
            	[
            	
            	]

            but was
            	[
            	  0
            	]
            """);

        Contradiction(new int[] { }, x => x.ShouldBe([0]),
            """
            x should be
            	[
            	  0
            	]

            but was
            	[
            	
            	]
            """);

        new[] { false, true, false }.ShouldBe([false, true, false]);

        Contradiction(new[] { false, true, false }, x => x.ShouldBe([false, true]),
            """
            x should be
            	[
            	  false,
            	  true
            	]

            but was
            	[
            	  false,
            	  true,
            	  false
            	]
            """);
        
        new[] { 'A', 'B', 'C' }.ShouldBe(['A', 'B', 'C']);
        
        Contradiction(new[] { 'A', 'B', 'C' }, x => x.ShouldBe(['A', 'C']),
            """
            x should be
            	[
            	  'A',
            	  'C'
            	]

            but was
            	[
            	  'A',
            	  'B',
            	  'C'
            	]
            """);

        new[] { "A", "B", "C" }.ShouldBe(["A", "B", "C"]);

        Contradiction(new[] { "A", "B", "C" }, x => x.ShouldBe(["A", "C"]),
            """
            x should be
            	[
            	  "A",
            	  "C"
            	]

            but was
            	[
            	  "A",
            	  "B",
            	  "C"
            	]
            """);

        new[] { typeof(int), typeof(bool) }.ShouldBe([typeof(int), typeof(bool)]);

        Contradiction(new[] { typeof(int), typeof(bool) }, x => x.ShouldBe([typeof(bool), typeof(int)]),
            """
            x should be
            	[
            	  typeof(bool),
            	  typeof(int)
            	]
            
            but was
            	[
            	  typeof(int),
            	  typeof(bool)
            	]
            """);

        var sampleA = new Sample("A");
        var sampleB = new Sample("B");

        new[] { sampleA, sampleB }.ShouldBe([sampleA, sampleB]);

        Contradiction(new[] { sampleA, sampleB }, x => x.ShouldBe([sampleB, sampleA]),
            """
            x should be
            	[
            	  Sample B,
            	  Sample A
            	]

            but was
            	[
            	  Sample A,
            	  Sample B
            	]
            """);
    }

    class SampleA;
    class SampleB;

    class Sample(string name)
    {
        public override string ToString() => $"Sample {name}";
    }

    static void Contradiction<T>(T actual, Action<T> shouldThrow, string expectedMessage, [CallerArgumentExpression(nameof(shouldThrow))] string? assertion = null)
    {
        try
        {
            shouldThrow(actual);
        }
        catch (Exception exception)
        {
            if (exception is AssertException)
            {
                if (exception.Message != expectedMessage)
                    throw new Exception(
                        $"An example assertion failed as expected, but with the wrong message.{Line}" +
                        $"Expected Message:{Line}{Indent(expectedMessage)}{Line}" +
                        $"Actual Message:{Line}{Indent(exception.Message)}");
                return;
            }

            throw new Exception(
                $"An example assertion failed as expected, but with the wrong type.{Line}" +
                $"\t{assertion}{Line}" +
                $"The actual value in question was:{Line}" +
                $"\t{actual}{Line}" +
                $"The assertion threw {exception.GetType().FullName} with message:{Line}" +
                $"\t{exception.Message}");
        }

        throw new Exception(
            $"An example assertion was expected to fail, but did not:{Line}" +
            $"\t{assertion}{Line}" +
            $"The actual value in question was:{Line}" +
            $"\t{actual}");
    }

    static string Indent(string multiline) =>
        string.Join(NewLine, multiline.Split(NewLine).Select(x => $"\t{x}"));

    static IEnumerable<char> UnicodeEscapedCharacters()
    {
        // Code points from \u0000 to \u001F, \u007F, and from \u0080 to \u009F are
        // "control characters". Some of these have single-character escape sequences
        // like '\u000A' being equivalent to '\n'. When we omit code points better
        // served by single-character escape sequences, we are left with those deserving
        // '\uHHHH' hex escape sequences.

        for (char c = '\u0001'; c <= '\u0006'; c++) yield return c;
        for (char c = '\u000E'; c <= '\u001F'; c++) yield return c;
        yield return '\u007F';
        for (char c = '\u0080'; c <= '\u009F'; c++) yield return c;

        // Several code points represent various kinds of whitespace. Some of these have
        // single-character escape sequences like '\u0009' being equivalent to '\t', and
        // the single space character ' ' is naturally represented with no need for any
        // escape sequence. All other whitespace code points deserve '\uHHHH' hex escape
        // sequences.

        foreach (char c in (char[])['\u0085', '\u00A0', '\u1680']) yield return c;
        for (char c = '\u2000'; c <= '\u2009'; c++) yield return c;
        foreach (char c in (char[])['\u200A', '\u2028', '\u2029', '\u202F', '\u205F', '\u3000']) yield return c;
    }
}