using System;
using Microsoft.Build.Framework;
using NAntGui.Framework;

namespace NAntGui.MSBuild
{
    class GuiLogger : ILogger
    {
        private readonly ILogsMessage _messageLogger;
        private readonly BuildFinishedEventHandler _buildFinished;
        private LoggerVerbosity _verbosity;

        public GuiLogger(ILogsMessage messageLogger, BuildFinishedEventHandler buildFinished)
        {
            _messageLogger = messageLogger;
            _buildFinished = buildFinished;
            _verbosity = LoggerVerbosity.Normal;
        }

        void ILogger.Initialize(IEventSource eventSource)
        {
            eventSource.MessageRaised += EventSourceMessageRaised;
            eventSource.WarningRaised += EventSourceWarningRaised;
            eventSource.ErrorRaised += EventSourceErrorRaised;
            eventSource.BuildFinished += _buildFinished;
        }

        public string Parameters
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Shutdown()
        {
            // do nothing
        }

        public LoggerVerbosity Verbosity
        {
            get { return _verbosity; }
            set { _verbosity = value; }
        }

        private void EventSourceErrorRaised(object sender, BuildErrorEventArgs e)
        {
            // BuildErrorEventArgs adds LineNumber, ColumnNumber, File, amongst other parameters
            string line = String.Format("ERROR {0}({1},{2}): ", e.File, e.LineNumber, e.ColumnNumber);
            WriteMessage(line, e);
        }

        private void EventSourceWarningRaised(object sender, BuildWarningEventArgs e)
        {
            // BuildWarningEventArgs adds LineNumber, ColumnNumber, File, amongst other parameters
            string line = String.Format("Warning {0}({1},{2}): ", e.File, e.LineNumber, e.ColumnNumber);
            WriteMessage(line, e);
        }

        private void EventSourceMessageRaised(object sender, BuildMessageEventArgs e)
        {
            // BuildMessageEventArgs adds Importance to BuildEventArgs
            // Let's take account of the verbosity setting we've been passed in deciding whether to log the message
            if ((e.Importance == MessageImportance.High && _verbosity == LoggerVerbosity.Minimal)
                || (e.Importance == MessageImportance.Normal && _verbosity == LoggerVerbosity.Normal)
                || (e.Importance == MessageImportance.Low && _verbosity == LoggerVerbosity.Detailed)
                )
            {
                WriteMessage(string.Empty, e);
            }
        }

        private void WriteMessage(string line, BuildEventArgs e)
        {
            _messageLogger.LogMessage(line + e.Message);
        }
     }
}
