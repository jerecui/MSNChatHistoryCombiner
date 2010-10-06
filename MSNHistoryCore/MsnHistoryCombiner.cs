using System.Collections.Generic;
using System.IO;
using System;

namespace MsnHistoryCore
{
    public class MsnHistoryCombiner
    {
        public MsnHistoryCombiner()
        {

        }

        public List<string> XmlFilePaths
        {
            get
            {
                return _xmlFilePaths;
            }
        } private List<string> _xmlFilePaths = new List<string>();

        internal MsnLog MergedMsnLog { get; set; }

        internal List<MsnLog> SourceLogs
        {
            get
            {
                if (_sourceLogs == null)
                {
                    _sourceLogs = new List<MsnLog>();
                    XmlFilePaths.ForEach(path =>
                        {
                            if (File.Exists(path)) _sourceLogs.Add(HistoryXmlReader.Instance.Read(path));
                        });
                }

                return _sourceLogs;
            }
        } private List<MsnLog> _sourceLogs;

        public void Merge()
        {
            MergeSingle();

            MergeMultiple();
        }

        private void MergeMultiple()
        {
            if (this.SourceLogs.Count <= 1) return;

            this.MergedMsnLog = new MsnLog();
            this.MergedMsnLog.Declaration = this.SourceLogs[0].Declaration;
            this.MergedMsnLog.Xsl = this.SourceLogs[0].Xsl;

            this.SourceLogs.ForEach(item => this.MergedMsnLog.Messages.AddRange(item.Messages));

            var count = MergedMsnLog.Messages.FindAll(item => item == null).Count;
            if (count > 0)
                throw new Exception();

            this.MergedMsnLog.Messages.RemoveAll(item => item == null);

            this.MergedMsnLog.Messages.Sort((x, y) =>
                {
                    return x.UniversalTime.CompareTo(y.UniversalTime);
                });

            RemoveDuplicatedMessage();
            ResetSessionID();

            var fileName = GetMergedFileName();

            //log
            InternalLogger.Write("handling " + new FileInfo(fileName).Name.GetHistoryFileUniqueName());

            this.MergedMsnLog.Save(fileName);
        }

        private string GetMergedFileName()
        {
            if (this.SourceLogs.Count > 0)
            {
                var file = new FileInfo(this.SourceLogs[0].XmlFilePath);

                return Path.Combine(MsnContext.Instance.TargetDirectoryPath, file.Name.GetHistoryFileUniqueName());
            }
            return string.Empty;
        }

        private void MergeSingle()
        {
            if (this.SourceLogs.Count == 1)
            {
                var source = this.SourceLogs[0];
                var targetFileName = new FileInfo(source.XmlFilePath).Name.GetHistoryFileUniqueName();

                if (string.IsNullOrEmpty(targetFileName)) return;

                var targetFilePath = Path.Combine(MsnContext.Instance.TargetDirectoryPath, targetFileName);
                File.Copy(source.XmlFilePath, targetFilePath, true);
            }

        }

        internal void RemoveDuplicatedMessage()
        {
            for (int i = 0; i < this.MergedMsnLog.Messages.Count; i++)
            {
                var current = this.MergedMsnLog.Messages[i];

                for (int j = i + 1; j < this.MergedMsnLog.Messages.Count; j++)
                {
                    var comparing = this.MergedMsnLog.Messages[j];
                    if (comparing.IsDuplicated) continue;

                    if (current.GetSpanSecondsFromOther(comparing) > 10) break;

                    if (current.CompareTo(comparing) == 0)
                        comparing.IsDuplicated = true;

                }
            }

            this.MergedMsnLog.Messages.RemoveAll(item => item.IsDuplicated == true);
        }

        internal void ResetSessionID()
        {
            var lastSessionId = 0;
            if (this.MergedMsnLog.Messages.Count > 0) lastSessionId = this.MergedMsnLog.Messages[0].SessionID;

            var currentSessionId = 1;
            this.MergedMsnLog.Messages.ForEach(item =>
                {
                    if (lastSessionId != item.SessionID)
                    {
                        currentSessionId++;
                    }

                    lastSessionId = item.SessionID;
                    item.SessionID = currentSessionId;
                });
        }

    }
}
