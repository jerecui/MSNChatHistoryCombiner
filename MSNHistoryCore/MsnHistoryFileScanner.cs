using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

    }
}
