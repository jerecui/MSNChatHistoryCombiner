using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
            this.MergedMsnLog.Messages.Sort((x, y) =>
                {
                    return x.UniversalTime.CompareTo(y.UniversalTime);
                });

            RemoveDuplicatedMessage();
            ResetSessionID();

            var fileName = GetMergedFileName();

            this.MergedMsnLog.Save(fileName);
        }

        private string GetMergedFileName()
        {
            if (this.SourceLogs.Count > 0)
            {
                var file= new FileInfo(this.SourceLogs[0].XmlFilePath);

                return Path.Combine(MsnContext.Instance.TargetDirectoryPath , file.Name.GetHistoryFileUniqueName());
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
                File.Copy(source.XmlFilePath, targetFilePath);
            }

        }

        internal void RemoveDuplicatedMessage()
        {

        }

        internal void ResetSessionID()
        {

        }

    }
}
