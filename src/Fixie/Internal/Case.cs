﻿namespace Fixie.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// A test case being executed, representing a single call to a test method.
    /// </summary>
    class Case
    {
        readonly object?[] parameters;

        public Case(MethodInfo testMethod, object?[] parameters)
        {
            this.parameters = parameters;
            Test = new Test(testMethod);
            Method = testMethod.TryResolveTypeArguments(parameters);
            Name = CaseNameBuilder.GetName(Method, parameters);
        }

        /// <summary>
        /// Gets the test for which this case describes a single execution.
        /// </summary>
        public Test Test { get; }

        /// <summary>
        /// Gets the input parameters for this single execution of the test.
        /// </summary>
        public IReadOnlyList<object?> Parameters => parameters;

        /// <summary>
        /// Gets the name of the test case, including any input parameters.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the method that defines this test case.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Gets the exception describing this test case's failure.
        /// </summary>
        public Exception? Exception { get; private set; }

        /// <summary>
        /// Indicate the test case was skipped for the given reason.
        /// </summary>
        public void Skip()
        {
            State = CaseState.Skipped;
            Exception = null;
        }

        /// <summary>
        /// Indicate the test case passed.
        /// </summary>
        public void Pass()
        {
            State = CaseState.Passed;
            Exception = null;
        }

        /// <summary>
        /// Indicate the test case failed for the given reason.
        /// </summary>
        public void Fail(Exception reason)
        {
            if (reason is PreservedException preservedException)
                reason = preservedException.OriginalException;

            State = CaseState.Failed;

            Exception = reason;
        }

        public CaseState State { get; private set; }

        public async Task RunAsync(object? instance)
            => await Method.RunTestMethodAsync(instance, parameters);
    }

    enum CaseState
    {
        Skipped,
        Passed,
        Failed
    }
}
