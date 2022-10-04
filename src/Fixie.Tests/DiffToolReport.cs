﻿namespace Fixie.Tests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Assertions;
    using Fixie.Reports;
    using DiffEngine;

    class DiffToolReport : IHandler<TestFailed>, IHandler<ExecutionCompleted>
    {
        int failures;
        Exception? singleFailure;

        public Task Handle(TestFailed message)
        {
            failures++;

            singleFailure = failures == 1 ? message.Reason : null;

            return Task.CompletedTask;
        }

        public async Task Handle(ExecutionCompleted message)
        {
            if (singleFailure is AssertException exception)
                if (!exception.HasCompactRepresentations)
                    await LaunchDiffTool(exception);
        }

        static async Task LaunchDiffTool(AssertException exception)
        {
            var tempPath = Path.GetTempPath();
            var expectedPath = Path.Combine(tempPath, "expected.txt");
            var actualPath = Path.Combine(tempPath, "actual.txt");

            File.WriteAllText(expectedPath, exception.Expected);
            File.WriteAllText(actualPath, exception.Actual);

            await DiffRunner.LaunchAsync(expectedPath, actualPath);
        }
    }
}