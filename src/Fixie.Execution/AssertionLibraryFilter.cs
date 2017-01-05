﻿namespace Fixie.Execution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class AssertionLibraryFilter
    {
        readonly List<Type> exceptionTypes;
        readonly List<Type> stackTraceTypes;

        public AssertionLibraryFilter(Convention convention)
        {
            exceptionTypes = new List<Type>();
            stackTraceTypes = new List<Type>();

            foreach (var type in convention.Config.AssertionLibraryTypes)
            {
                bool isExceptionType = type.IsSubclassOf(typeof(Exception));

                if (isExceptionType)
                    exceptionTypes.Add(type);
                else
                    stackTraceTypes.Add(type);
            }
        }

        public string FilterStackTrace(Exception exception)
        {
            return exception.StackTrace == null
                ? null
                : String.Join(Environment.NewLine,
                    Lines(exception.StackTrace)
                        .SkipWhile(ContainsTypeToFilter));
        }

        [Obsolete]
        public string DisplayName(Exception exception)
        {
            return IsFailedAssertion(exception) ? "" : exception.GetType().FullName;
        }

        public bool IsFailedAssertion(Exception exception)
        {
            return exceptionTypes.Contains(exception.GetType());
        }

        bool ContainsTypeToFilter(string line)
        {
            return stackTraceTypes.Any(type => line.Contains(type.FullName));
        }

        static IEnumerable<string> Lines(string stackTrace)
        {
            using (var reader = new StringReader(stackTrace))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    yield return line;
            }
        }
    }
}