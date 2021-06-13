﻿namespace Fixie.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Assertions;

    public class CaseNameTests
    {
        public async Task ShouldBeNamedAfterTheUnderlyingMethod()
        {
            var output = await RunScriptAsync<NoParametersTestClass>(async test =>
            {
                await RunAsync(test);
            });

            ShouldHaveNames(output, "NoParametersTestClass.NoParameters");
        }

        public async Task ShouldIncludeParameterValuesInNameWhenTheUnderlyingMethodHasParameters()
        {
            var output = await RunScriptAsync<ParameterizedTestClass>(async test =>
            {
                await RunAsync(test,
                    123, true, 'a', "with \"quotes\"", "long \"string\" gets truncated",
                    null, this, new ObjectWithNullStringRepresentation());
            });

            ShouldHaveNames(output,
                "ParameterizedTestClass.Parameterized(123, True, 'a', \"with \\\"quotes\\\"\", \"long \\\"string\\\" g...\", null, Fixie.Tests.CaseNameTests, Fixie.Tests.CaseNameTests+ObjectWithNullStringRepresentation)");
        }

        public async Task ShouldIncludeEscapeSequencesInNameWhenTheUnderlyingMethodHasCharParameters()
        {
            // Unicode characters 0085, 2028, and 2029 represent line endings Next Line, Line Separator, and Paragraph Separator, respectively.
            // Just like \r and \n, we escape these in order to present a readable string literal. All other unicode sequences pass through
            // with no additional special treatment.

            // \uxxxx - Unicode escape sequence for character with hex value xxxx.
            // \xn[n][n][n] - Unicode escape sequence for character with hex value nnnn (variable length version of \uxxxx).
            // \Uxxxxxxxx - Unicode escape sequence for character with hex value xxxxxxxx (for generating surrogates).

            var output = await RunScriptAsync<CharParametersTestClass>(async test =>
            {
                foreach (var c in new[] {'\"', '"', '\''})
                    await RunAsync(test, c);
                
                foreach (var c in new[] {'\\', '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v'})
                    await RunAsync(test, c);
                
                foreach (var c in new[] {'\u0000', '\u0085', '\u2028', '\u2029', '\u263A'})
                    await RunAsync(test, c);
                
                foreach (var c in new[] {'\x0000', '\x000', '\x00', '\x0'})
                    await RunAsync(test, c);
                
                foreach (var c in new[] {'\x0085', '\x085', '\x85', '\x2028', '\x2029', '\x263A'})
                    await RunAsync(test, c);
                
                foreach (var c in new[] {'\U00000000', '\U00000085', '\U00002028', '\U00002029', '\U0000263A'})
                    await RunAsync(test, c);
            });

            ShouldHaveNames(output,
                "CharParametersTestClass.Char('\"')",
                "CharParametersTestClass.Char('\"')",
                "CharParametersTestClass.Char('\\'')",

                "CharParametersTestClass.Char('\\\\')",
                "CharParametersTestClass.Char('\\0')",
                "CharParametersTestClass.Char('\\a')",
                "CharParametersTestClass.Char('\\b')",
                "CharParametersTestClass.Char('\\f')",
                "CharParametersTestClass.Char('\\n')",
                "CharParametersTestClass.Char('\\r')",
                "CharParametersTestClass.Char('\\t')",
                "CharParametersTestClass.Char('\\v')",
                
                "CharParametersTestClass.Char('\\0')",
                "CharParametersTestClass.Char('\\u0085')",
                "CharParametersTestClass.Char('\\u2028')",
                "CharParametersTestClass.Char('\\u2029')",
                "CharParametersTestClass.Char('☺')",

                "CharParametersTestClass.Char('\\0')",
                "CharParametersTestClass.Char('\\0')",
                "CharParametersTestClass.Char('\\0')",
                "CharParametersTestClass.Char('\\0')",

                "CharParametersTestClass.Char('\\u0085')",
                "CharParametersTestClass.Char('\\u0085')",
                "CharParametersTestClass.Char('\\u0085')",
                "CharParametersTestClass.Char('\\u2028')",
                "CharParametersTestClass.Char('\\u2029')",
                "CharParametersTestClass.Char('☺')",

                "CharParametersTestClass.Char('\\0')",
                "CharParametersTestClass.Char('\\u0085')",
                "CharParametersTestClass.Char('\\u2028')",
                "CharParametersTestClass.Char('\\u2029')",
                "CharParametersTestClass.Char('☺')"
            );
        }

        public async Task ShouldIncludeEscapeSequencesInNameWhenTheUnderlyingMethodHasStringParameters()
        {
            // Unicode characters 0085, 2028, and 2029 represent line endings Next Line, Line Separator, and Paragraph Separator, respectively.
            // Just like \r and \n, we escape these in order to present a readable string literal. All other unicode sequences pass through
            // with no additional special treatment.

            // \uxxxx - Unicode escape sequence for character with hex value xxxx.
            // \xn[n][n][n] - Unicode escape sequence for character with hex value nnnn (variable length version of \uxxxx).
            // \Uxxxxxxxx - Unicode escape sequence for character with hex value xxxxxxxx (for generating surrogates).

            var output = await RunScriptAsync<StringParametersTestClass>(async test =>
            {
                await RunAsync(test, " \' ' \" ");
                await RunAsync(test, " \\ \0 \a \b ");
                await RunAsync(test, " \f \n \r \t \v ");
                await RunAsync(test, " \u0000 \u0085 \u2028 \u2029 \u263A ");
                await RunAsync(test, " \x0000 \x000 \x00 \x0 ");
                await RunAsync(test, " \x0085 \x085 \x85 \x2028 \x2029 \x263A ");
                await RunAsync(test, " \U00000000 \U00000085 \U00002028 \U00002029 \U0000263A ");
            });

            ShouldHaveNames(output,
                "StringParametersTestClass.String(\" ' ' \\\" \")",
                "StringParametersTestClass.String(\" \\\\ \\0 \\a \\b \")",
                "StringParametersTestClass.String(\" \\f \\n \\r \\t \\v \")",
                "StringParametersTestClass.String(\" \\0 \\u0085 \\u2028 \\u2029 ☺ \")",
                "StringParametersTestClass.String(\" \\0 \\0 \\0 \\0 \")",
                "StringParametersTestClass.String(\" \\u0085 \\u0085 \\u0085 \\u2028 \\u2029 ☺ \")",
                "StringParametersTestClass.String(\" \\0 \\u0085 \\u2028 \\u2029 ☺ \")"
            );
        }

        public async Task ShouldIncludeResolvedGenericArgumentsInNameWhenTheUnderlyingMethodIsGeneric()
        {
            var output = await RunScriptAsync<GenericTestClass>(async test =>
            {
                await RunAsync(test, 123, true, "a", "b");
                await RunAsync(test, 123, true, 1, null);
                await RunAsync(test, 123, 1.23m, "a", null);
            });

            ShouldHaveNames(output,
                "GenericTestClass.Generic<System.Boolean, System.String>(123, True, \"a\", \"b\")",
                "GenericTestClass.Generic<System.Boolean, System.Int32>(123, True, 1, null)",
                "GenericTestClass.Generic<System.Decimal, System.String>(123, 1.23, \"a\", null)"
            );
        }

        public async Task ShouldUseGenericTypeParametersInNameWhenGenericTypeParametersCannotBeResolved()
        {
            var output = await RunScriptAsync<ConstrainedGenericTestClass>(async test =>
            {
                await RunAsync(test, 1);
                await RunAsync(test, true);
                await RunAsync(test, "Incompatible");
            });

            ShouldHaveNames(output,
                "ConstrainedGenericTestClass.ConstrainedGeneric<System.Int32>(1)",
                "ConstrainedGenericTestClass.ConstrainedGeneric<System.Boolean>(True)",
                "ConstrainedGenericTestClass.ConstrainedGeneric<T>(\"Incompatible\")"
            );
        }

        public async Task ShouldInferAppropriateClassUnderInheritance()
        {
            var parent = await RunScriptAsync<ParentTestClass>(async test =>
            {
                await RunAsync(test);
            });

            ShouldHaveNames(parent,
                "ParentTestClass.TestMethodDefinedWithinParentClass"
            );

            var child = await RunScriptAsync<ChildTestClass>(async test =>
            {
                await RunAsync(test);
            });

            ShouldHaveNames(child,
                "ChildTestClass.TestMethodDefinedWithinChildClass",
                "ChildTestClass.TestMethodDefinedWithinParentClass"
            );
        }

        class ScriptedExecution : Execution
        {
            readonly Func<Test, Task> scriptAsync;

            public ScriptedExecution(Func<Test, Task> scriptAsync)
                => this.scriptAsync = scriptAsync;

            public async Task RunAsync(TestAssembly testAssembly)
            {
                foreach (var test in testAssembly.Tests)
                    await scriptAsync(test);
            }
        }

        static Task<IEnumerable<string>> RunScriptAsync<TSampleTestClass>(Func<Test, Task> scriptAsync)
            => Utility.RunAsync(typeof(TSampleTestClass), new ScriptedExecution(scriptAsync));

        static async Task RunAsync(Test test, params object?[] parameters)
        {
            await test.RunAsync(parameters);
            await test.PassAsync(parameters);
            await test.FailAsync(parameters, new FailureException());
            await test.SkipAsync(parameters, reason: "Exercising Skipped Case Names");
        }

        void ShouldHaveNames(IEnumerable<string> actual, params string[] expected)
        {
            const int variants = 4;

            var actualArray = actual.ToArray();
            actualArray.Length.ShouldBe(expected.Length * variants);

            var expectedVariants = expected.Select(name => new[]
            {
                name.Contains("Incompatible")
                    ? $"{name} failed: Could not resolve type parameters for generic method."
                    : $"{name} passed",
                $"{name} passed",
                $"{name} failed: 'RunAsync' failed!",
                $"{name} skipped: Exercising Skipped Case Names"
            }).SelectMany(x => x);

            var fullyQualifiedExpectation = expectedVariants.Select(x => GetType().FullName + "+" + x).ToArray();

            foreach (var (first, second) in actualArray.Zip(fullyQualifiedExpectation))
                first.ShouldBe(second);
        }

        class ObjectWithNullStringRepresentation
        {
            public override string? ToString() => null;
        }

        class NoParametersTestClass
        {
            public void NoParameters() { }
        }

        class ParameterizedTestClass
        {
            public void Parameterized(int i, bool b, char ch, string s1, string s2, object obj, CaseNameTests complex, ObjectWithNullStringRepresentation nullStringRepresentation) { }
        }

        class CharParametersTestClass
        {
            public void Char(char c) { }
        }

        class StringParametersTestClass
        {
            public void String(string s) { }
        }

        class GenericTestClass
        {
            public void Generic<T1, T2>(int i, T1 t1, T2 t2a, T2 t2b) { }
        }

        class ConstrainedGenericTestClass
        {
            public void ConstrainedGeneric<T>(T t) where T : struct { }
        }

        class ParentTestClass
        {
            public void TestMethodDefinedWithinParentClass()
            {
            }
        }

        class ChildTestClass : ParentTestClass
        {
            public void TestMethodDefinedWithinChildClass()
            {
            }
        }
    }
}