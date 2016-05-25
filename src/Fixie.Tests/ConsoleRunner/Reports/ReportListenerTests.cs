﻿namespace Fixie.Tests.ConsoleRunner.Reports
{
    using System;
    using System.Linq;
    using Fixie.ConsoleRunner.Reports;
    using Fixie.Execution;
    using Fixie.Internal;
    using Should;

    public class ReportListenerTests : MessagingTests
    {
        public void ShouldBuildReport()
        {
            var listener = new ReportListener();

            using (var console = new RedirectedConsole())
            {
                Run(listener);

                console.Lines().ShouldEqual(
                    "Console.Out: Fail",
                    "Console.Error: Fail",
                    "Console.Out: FailByAssertion",
                    "Console.Error: FailByAssertion",
                    "Console.Out: Pass",
                    "Console.Error: Pass");
            }

            var report = listener.Report;

            report.Passed.ShouldEqual(1);
            report.Failed.ShouldEqual(2);
            report.Skipped.ShouldEqual(2);
            report.Total.ShouldEqual(5);

            report.Assemblies.Count.ShouldEqual(1);

            var assemblyReport = report.Assemblies.Single();
            assemblyReport.Location.ShouldEqual(typeof(ReportListenerTests).Assembly.Location);
            assemblyReport.Duration.ShouldBeGreaterThanOrEqualTo(TimeSpan.Zero);
            assemblyReport.Passed.ShouldEqual(1);
            assemblyReport.Failed.ShouldEqual(2);
            assemblyReport.Skipped.ShouldEqual(2);
            assemblyReport.Total.ShouldEqual(5);

            var classReport = assemblyReport.Classes.Single();
            classReport.Name.ShouldEqual(TestClass);
            classReport.Duration.ShouldBeGreaterThanOrEqualTo(TimeSpan.Zero);
            classReport.Passed.ShouldEqual(1);
            classReport.Failed.ShouldEqual(2);
            classReport.Skipped.ShouldEqual(2);

            var cases = classReport.Cases;

            cases.Count.ShouldEqual(5);

            var skipWithReason = (CaseSkipped)cases[0];
            var skipWithoutReason = (CaseSkipped)cases[1];
            var fail = (CaseFailed)cases[2];
            var failByAssertion = (CaseFailed)cases[3];
            var pass = cases[4];

            skipWithReason.MethodGroup.FullName.ShouldEqual(TestClass + ".SkipWithReason");
            skipWithReason.Name.ShouldEqual(TestClass + ".SkipWithReason");
            skipWithReason.Status.ShouldEqual(CaseStatus.Skipped);
            skipWithReason.Duration.ShouldEqual(TimeSpan.Zero);
            skipWithReason.Output.ShouldEqual(null);
            skipWithReason.Reason.ShouldEqual("Skipped with reason.");

            skipWithoutReason.MethodGroup.FullName.ShouldEqual(TestClass + ".SkipWithoutReason");
            skipWithoutReason.Name.ShouldEqual(TestClass + ".SkipWithoutReason");
            skipWithoutReason.Status.ShouldEqual(CaseStatus.Skipped);
            skipWithoutReason.Duration.ShouldEqual(TimeSpan.Zero);
            skipWithoutReason.Output.ShouldEqual(null);
            skipWithoutReason.Reason.ShouldEqual(null);

            fail.MethodGroup.FullName.ShouldEqual(TestClass + ".Fail");
            fail.Name.ShouldEqual(TestClass + ".Fail");
            fail.Status.ShouldEqual(CaseStatus.Failed);
            fail.Duration.ShouldBeGreaterThanOrEqualTo(TimeSpan.Zero);
            fail.Output.Lines().ShouldEqual("Console.Out: Fail", "Console.Error: Fail");
            fail.Exception.Type.ShouldEqual("Fixie.Tests.FailureException");
            fail.Exception.Message.ShouldEqual("'Fail' failed!");
            fail.Exception.FailedAssertion.ShouldEqual(false);
            fail.Exception.StackTrace
                .CleanStackTraceLineNumbers()
                .ShouldEqual(At("Fail()"));

            failByAssertion.MethodGroup.FullName.ShouldEqual(TestClass + ".FailByAssertion");
            failByAssertion.Name.ShouldEqual(TestClass + ".FailByAssertion");
            failByAssertion.Status.ShouldEqual(CaseStatus.Failed);
            failByAssertion.Duration.ShouldBeGreaterThanOrEqualTo(TimeSpan.Zero);
            failByAssertion.Output.Lines().ShouldEqual("Console.Out: FailByAssertion", "Console.Error: FailByAssertion");
            failByAssertion.Exception.Type.ShouldEqual("Should.Core.Exceptions.EqualException");
            failByAssertion.Exception.Message.Lines().ShouldEqual(
                "Assert.Equal() Failure",
                "Expected: 2",
                "Actual:   1");
            failByAssertion.Exception.FailedAssertion.ShouldEqual(true);
            failByAssertion.Exception.StackTrace
                .CleanStackTraceLineNumbers()
                .ShouldEqual(At("FailByAssertion()"));

            pass.MethodGroup.FullName.ShouldEqual(TestClass + ".Pass");
            pass.Name.ShouldEqual(TestClass + ".Pass");
            pass.Status.ShouldEqual(CaseStatus.Passed);
            pass.Duration.ShouldBeGreaterThanOrEqualTo(TimeSpan.Zero);
            pass.Output.Lines().ShouldEqual("Console.Out: Pass", "Console.Error: Pass");
        }
    }
}