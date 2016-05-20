﻿namespace Fixie.Tests.ConsoleRunner.Reports
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using Fixie.ConsoleRunner.Reports;
    using Fixie.Internal;
    using Should;

    public class NUnitXmlReportTests
    {
        public void ShouldProduceValidXmlDocument()
        {
            var listener = new ReportListener();

            var convention = SampleTestClassConvention.Build();

            using (var console = new RedirectedConsole())
            {
                typeof(SampleTestClass).Run(listener, convention);

                console.Lines()
                    .ShouldEqual(
                        "Console.Out: Fail",
                        "Console.Error: Fail",
                        "Console.Out: FailByAssertion",
                        "Console.Error: FailByAssertion",
                        "Console.Out: Pass",
                        "Console.Error: Pass");
            }

            var report = new NUnitXmlReport();
            var actual = report.Transform(listener.Report);

            XsdValidate(actual);
            CleanBrittleValues(actual.ToString(SaveOptions.DisableFormatting)).ShouldEqual(ExpectedReport);
        }

        static void XsdValidate(XDocument doc)
        {
            var schemaSet = new XmlSchemaSet();
            using (var xmlReader = XmlReader.Create(Path.Combine("ConsoleRunner", Path.Combine("Reports", "NUnitXmlReport.xsd"))))
            {
                schemaSet.Add(null, xmlReader);
            }

            doc.Validate(schemaSet, null);
        }

        static string CleanBrittleValues(string actualRawContent)
        {
            //Avoid brittle assertion introduced by system date.
            var cleaned = Regex.Replace(actualRawContent, @"date=""\d\d\d\d-\d\d-\d\d""", @"date=""YYYY-MM-DD""");

            //Avoid brittle assertion introduced by system time.
            cleaned = Regex.Replace(cleaned, @"time=""\d\d:\d\d:\d\d""", @"time=""HH:MM:SS""");

            //Avoid brittle assertion introduced by test duration.
            cleaned = Regex.Replace(cleaned, @"time=""[\d\.]+""", @"time=""1.234""");

            //Avoid brittle assertion introduced by stack trace line numbers.
            cleaned = Regex.Replace(cleaned, @":line \d+", ":line #");

            //Avoid brittle assertion introduced by environment attributes.
            cleaned = Regex.Replace(cleaned, @"clr-version=""[^""]*""", @"clr-version=""[clr-version]""");
            cleaned = Regex.Replace(cleaned, @"os-version=""[^""]*""", @"os-version=""[os-version]""");
            cleaned = Regex.Replace(cleaned, @"platform=""[^""]*""", @"platform=""[platform]""");
            cleaned = Regex.Replace(cleaned, @"cwd=""[^""]*""", @"cwd=""[cwd]""");
            cleaned = Regex.Replace(cleaned, @"machine-name=""[^""]*""", @"machine-name=""[machine-name]""");
            cleaned = Regex.Replace(cleaned, @"user=""[^""]*""", @"user=""[user]""");
            cleaned = Regex.Replace(cleaned, @"user-domain=""[^""]*""", @"user-domain=""[user-domain]""");

            //Avoid brittle assertion introduced by culture attributes.
            cleaned = Regex.Replace(cleaned, @"current-culture=""[^""]*""", @"current-culture=""[current-culture]""");
            cleaned = Regex.Replace(cleaned, @"current-uiculture=""[^""]*""", @"current-uiculture=""[current-uiculture]""");

            return cleaned;
        }

        string ExpectedReport
        {
            get
            {
                var assemblyLocation = GetType().Assembly.Location;
                var fileLocation = SampleTestClass.FilePath();
                return XDocument.Parse(File.ReadAllText(Path.Combine("ConsoleRunner", Path.Combine("Reports", "NUnitXmlReport.xml"))))
                                .ToString(SaveOptions.DisableFormatting)
                                .Replace("[assemblyLocation]", assemblyLocation)
                                .Replace("[fileLocation]", fileLocation);
            }
        }
    }
}