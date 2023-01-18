using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.FileSpliter
{
    public class MasterFile
    {
        public string SourceFilename { get; set; }
        public SplitType SplitBy { get; set; }
        public int Value { get; set; }
        public List<SplitedFile> Files { get; }
    }
}
