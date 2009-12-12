using System;
using System.Collections.Generic;
using System.IO;

namespace Horn.Core.Utils.IO
{
    public class FileInfoCompare : IComparer<FileInfo>
    {
        public int Compare(FileInfo left, FileInfo right)
        {
            return DateTime.Compare(left.CreationTime, right.CreationTime);
        }
    }
}