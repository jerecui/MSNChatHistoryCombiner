using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MsnHistoryCore
{
    public class MsnHistoryCombineWorker : SingletonBase<MsnHistoryCombineWorker>
    {
        public MsnHistoryCombineWorker()
        {

        }

        protected internal Dictionary<string, bool> HistoryRepository = new Dictionary<string, bool>();

        internal List<MsnHistoryCombiner> Group()
        {
            List<MsnHistoryCombiner> combiners = new List<MsnHistoryCombiner>();

            var itemArray = Instance.HistoryRepository.Keys.ToArray<string>();

            for (int outIndex = 0; outIndex < itemArray.Length; outIndex++)
            {
                var currentFileName = itemArray[outIndex];
                if (MsnHistoryCombineWorker.Instance.HistoryRepository[currentFileName]) continue;
                var currentFileInfo = new FileInfo(currentFileName);
                MsnHistoryCombineWorker.Instance.HistoryRepository[currentFileName] = true;

                var combiner = new MsnHistoryCombiner();
                combiner.XmlFilePaths.Add(currentFileName);

                for (int innerIndex = outIndex + 1; innerIndex < itemArray.Length; innerIndex++)
                {
                    var comparingFileName = itemArray[innerIndex];
                    if (MsnHistoryCombineWorker.Instance.HistoryRepository[comparingFileName]) continue;

                    var comparingFileInfo = new FileInfo(comparingFileName);
                    if (currentFileInfo.Name.IsSameFriendHistory(comparingFileInfo.Name))
                    {
                        MsnHistoryCombineWorker.Instance.HistoryRepository[comparingFileName] = true;

                        combiner.XmlFilePaths.Add(comparingFileName);
                    }
                }

                combiners.Add(combiner);
            }

            return combiners;
        }

        public void Clear()
        {
            MsnHistoryCombineWorker.Instance.HistoryRepository.Clear();
        }

        public void Scan()
        {
            this.Clear();

            MsnContext.Instance.SourceDirectories.ForEach(source =>
                {
                    if (Directory.Exists(source))
                    {
                        var xmlFiles = new DirectoryInfo(source).GetFiles("*.xml");
                        xmlFiles.ToList<FileInfo>().ForEach(xmlFile =>
                            {
                                if (xmlFile.Name.IsMatchedMsnHistoryFile())
                                    this.HistoryRepository.Add(xmlFile.FullName, false);
                                else
                                    MsnContext.Instance.ComplexedXmlFilePath.Add(xmlFile.FullName);
                            });
                    }
                });
        }

        public void Merge()
        {
            var combineList = this.Group();

            foreach (var item in combineList) item.Merge();

            CopyXslFile();

            NotifyDoneMessage();
        }

        internal void CopyXslFile()
        {
            var targetDirInfo = new DirectoryInfo(MsnContext.Instance.TargetDirectoryPath);
            var fileName = "MessageLog.xsl";
            var existXsl = targetDirInfo.GetFiles("MessageLog.xsl").Length > 0;

            if (!existXsl)
            {
                var xsl = new FileInfo(fileName);
                var targeFileName = Path.Combine(targetDirInfo.FullName, xsl.Name);
                if (xsl.Exists)
                {
                    xsl.CopyTo(targeFileName);
                }
            }
        }

        private void NotifyDoneMessage()
        {
            InternalLogger.Write("DONE!");

            if (MsnContext.Instance.ComplexedXmlFilePath.Count > 0)
                InternalLogger.Write("But you need to handle the following files manually:");
            MsnContext.Instance.ComplexedXmlFilePath.ForEach(item => InternalLogger.Write(item));
        }

    }
}
