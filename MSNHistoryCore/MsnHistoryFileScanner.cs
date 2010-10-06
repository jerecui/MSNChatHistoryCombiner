using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MsnHistoryCore
{
    public class MsnHistoryFileScanner : SingletonBase<MsnHistoryFileScanner>
    {
        public MsnHistoryFileScanner()
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
                if (MsnHistoryFileScanner.Instance.HistoryRepository[currentFileName]) continue;
                var currentFileInfo = new FileInfo(currentFileName);
                MsnHistoryFileScanner.Instance.HistoryRepository[currentFileName] = true;

                var combiner = new MsnHistoryCombiner();
                combiner.XmlFilePaths.Add(currentFileName);

                for (int innerIndex = outIndex + 1; innerIndex < itemArray.Length; innerIndex++)
                {
                    var comparingFileName = itemArray[innerIndex];
                    if (MsnHistoryFileScanner.Instance.HistoryRepository[comparingFileName]) continue;

                    var comparingFileInfo = new FileInfo(comparingFileName);
                    if (currentFileInfo.Name.IsSameFriendHistory(comparingFileInfo.Name))
                    {
                        MsnHistoryFileScanner.Instance.HistoryRepository[comparingFileName] = true;

                        combiner.XmlFilePaths.Add(comparingFileName);
                    }
                }

                combiners.Add(combiner);
            }

            return combiners;
        }

        public void Clear()
        {
            MsnHistoryFileScanner.Instance.HistoryRepository.Clear();
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
                            });
                    }
                });
        }

        public void Merge()
        {
            var combineList = this.Group();

            combineList.ForEach(item => item.Merge());
        }

    }
}
