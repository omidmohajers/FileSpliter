using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.FileSpliter
{
    public class SplitedFile
    {
        public string SaveTo { get; set; }
        public byte[] Content { get; set; }
        public int FileNumber { get; set; }
        public bool IsChecked { get; set; }
        public string FileName { get; internal set; }
    }
}
