﻿namespace Fixie.Execution.Listeners
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using Execution;

    public class ReportListener<TXmlFormat> :
        Handler<AssemblyStarted>,
        Handler<ClassStarted>,
        Handler<CaseCompleted>,
        Handler<ClassCompleted>,
        Handler<AssemblyCompleted>
        where TXmlFormat : XmlFormat, new()
    {
        Report report;
        ClassReport currentClass;
        readonly Action<Report> save;

        public ReportListener(Action<Report> save)
        {
            this.save = save;
        }

        public ReportListener()
            : this(Save)
        {
        }

        public void Handle(AssemblyStarted message)
        {
            report = new Report(message.Assembly);
        }

        public void Handle(ClassStarted message)
        {
            currentClass = new ClassReport(message.Class);
            report.Add(currentClass);
        }

        public void Handle(CaseCompleted message)
        {
            currentClass.Add(message);
        }

        public void Handle(ClassCompleted message)
        {
            currentClass = null;
        }

        public void Handle(AssemblyCompleted message)
        {
            save(report);
            report = null;
        }

        static void Save(Report report)
        {
            var format = new TXmlFormat();
            var xDocument = format.Transform(report);
            var filePath = Path.GetFullPath(report.Assembly.Location) + ".xml";

            using (var stream = new FileStream(filePath, FileMode.Create))
            using (var writer = new StreamWriter(stream))
                xDocument.Save(writer, SaveOptions.None);
        }
    }
}