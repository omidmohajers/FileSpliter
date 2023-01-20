using PA.FileUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.FileSplitter
{
    public class MasterFile : ICloneable
    {
        private int digitCount;

        public string SourceFilename { get; set; }
        public SplitType SplitBy { get; set; }
        public string Value { get; set; }
        public List<SplittedFile> Files { get; private set; }
        public string OutputPath { get; set; }
        public string OutputFilename { get; set; }
        public string FileExtention { get; set; }

        public object Clone()
        {
            MasterFile result = new MasterFile()
            {
                SourceFilename = this.SourceFilename,
                SplitBy = this.SplitBy,
                Value = this.Value,
                Files = this.Files,
                OutputPath = this.OutputPath,
                OutputFilename = this.OutputFilename,
                FileExtention = this.FileExtention
            };
            return result;
        }
        public MasterFile()
        {
            Files = new List<SplittedFile>();
        }
        public List<string> Validate()
        {
            List<string> result = new List<string>();
            if (string.IsNullOrWhiteSpace(SourceFilename))
            {
                result.Add("Source File Must be Selected!!!");
            }
            if (string.IsNullOrWhiteSpace(OutputPath))
            {
                result.Add("output Path Must be Selected!!!");
            }
            if (string.IsNullOrWhiteSpace(OutputFilename))
            {
                result.Add("output FileName Cant Empty!!!");
            }
            if (string.IsNullOrWhiteSpace(Value))
            {
                result.Add("Value is Invalid!!!");
            }
            return result;
        }

        public void Preview()
        {
            Files.Clear();
            switch (SplitBy)
            {
                case SplitType.ByLine:
                    PreByLine();
                    break;
                case SplitType.BySize:
                    PreBySize();
                    break;
                case SplitType.ByChar:
                    PreByChar();
                    break;
                case SplitType.ByPhrase:
                    PreByPhrase();
                    break;
            }
            GenerateFileName();
        }

        private void PreByLine()
        {
            string[] lines = File.ReadAllLines(SourceFilename);
            if (lines.Length == 0)
                return;
            int val = 0;
            int.TryParse(Value, out val);
            if (val < 1)
                val = 1;
            int counter = (lines.Length / val) + (lines.Length % val != 0 ? 1 : 0);
            for (int i = 0; i < counter; i++)
            {
                SplittedFile subFile = new SplittedFile();
                subFile.FileNumber = i + 1;
                Files.Add(subFile);
            }
            digitCount = (int)Math.Floor(Math.Log10(counter) + 1);
        }

        private void GenerateFileName()
        {
            foreach(SplittedFile sf in Files)
            {
                sf.FileName = string.Format("{0}{1}{2}", OutputFilename, sf.FileNumber.ToString().PadLeft(digitCount, '0'), FileExtention);
                sf.SaveTo = Path.Combine(OutputPath, sf.FileName);
            }
        }

        private void PreBySize()
        {
            byte[] data = FileProvider.ToByteArray(SourceFilename);
            if (data.Length == 0)
                return;
            int val = 0;
            int.TryParse(Value, out val);
            if (val < 1)
                val = 1;
            int len = val * 1024;
            int counter = (data.Length / len) + (data.Length % len != 0 ? 1 : 0);
            for (int i = 0; i < counter; i++)
            {
                SplittedFile subFile = new SplittedFile();
                subFile.FileNumber = i + 1;
                Files.Add(subFile);
            }
            digitCount = (int)Math.Floor(Math.Log10(counter) + 1);
        }
        private void PreByChar()
        {
            string data = File.ReadAllText(SourceFilename);
            if (data.Length == 0)
                return;
            int counter = 0;
            char val = Value.Length > 0 ? Value[0] : ',';
            string[] lines = data.Split(val);
            for (int i = 0; i < lines.Length; i++)
            {
                SplittedFile subFile = new SplittedFile();
                subFile.FileNumber = ++counter;
                Files.Add(subFile);
            }
            digitCount = (int)Math.Floor(Math.Log10(counter) + 1);
        }
        private void PreByPhrase()
        {
            string data = File.ReadAllText(SourceFilename);
            if (data.Length == 0)
                return;
            int counter = 0;
            string[] val = new string[] { Value };
            string[] lines = data.Split(val, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                SplittedFile subFile = new SplittedFile();
                subFile.FileNumber = ++counter;
                Files.Add(subFile);
            }
            digitCount = (int)Math.Floor(Math.Log10(counter) + 1);
        }

        private void SplitByLine()
        {
            string[] lines = File.ReadAllLines(SourceFilename);
            if (lines.Length == 0)
                return;
            int counter = 0;
            int val = 0;
            int.TryParse(Value, out val);
            if (val < 1)
                val = 1;
            for (int i = 0; i < lines.Length; )
            {
                int j = 0;
                StringBuilder sb = new StringBuilder();
                SplittedFile subFile = new SplittedFile();
                while (j < val && i < lines.Length)
                {
                    sb.AppendLine(lines[i]);
                    i++;
                    j++;
                }
                subFile.Content = Encoding.ASCII.GetBytes(sb.ToString());
                subFile.FileNumber = ++counter;
                Files.Add(subFile);
            }
            digitCount = (int)Math.Floor(Math.Log10(counter) + 1);
        }
        private void SplitBySize()
        {
            byte[] data = FileProvider.ToByteArray(SourceFilename);
            if (data.Length == 0)
                return;
            int counter = 0;
            int val = 0;
            int.TryParse(Value, out val);
            if (val < 1)
                val = 1;
            for (int i = 0; i < data.Length;)
            {
                int len = val * 1024;
                if (len > data.Length - i)
                    len = data.Length - i;
                byte[] subdata = new byte[len];
                Array.Copy(data, i, subdata, 0, len);
                SplittedFile subFile = new SplittedFile();
                subFile.Content = subdata;
                subFile.FileNumber = ++counter;
                Files.Add(subFile);
                i += len;
            }
            digitCount = (int)Math.Floor(Math.Log10(counter) + 1);
        }
        private void SplitByChar()
        {
            string data = File.ReadAllText(SourceFilename);
            if (data.Length == 0)
                return;
            int counter = 0;
            char val = Value.Length > 0 ? Value[0] : ',';
            string[] lines = data.Split(val);
            for (int i = 0; i < lines.Length;i++)
            {
                SplittedFile subFile = new SplittedFile();
                subFile.Content = Encoding.ASCII.GetBytes(lines[i]);
                subFile.FileNumber = ++counter;
                Files.Add(subFile);
            }
            digitCount = (int)Math.Floor(Math.Log10(counter) + 1);
        }
        private void SplitByPhrase()
        {
            string data = File.ReadAllText(SourceFilename);
            if (data.Length == 0)
                return;
            int counter = 0;
            string[] val = new string[] { Value };
            string[] lines = data.Split(val, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                SplittedFile subFile = new SplittedFile();
                subFile.Content = Encoding.ASCII.GetBytes(lines[i]);
                subFile.FileNumber = ++counter;
                Files.Add(subFile);
            }
            digitCount = (int)Math.Floor(Math.Log10(counter) + 1);
        }

        public void Start()
        {
            Files.Clear();
            switch (SplitBy)
            {
                case SplitType.ByLine:
                    SplitByLine();
                    break;
                case SplitType.BySize:
                    SplitBySize();
                    break;
                case SplitType.ByChar:
                    SplitByChar();
                    break;
                case SplitType.ByPhrase:
                    SplitByPhrase();
                    break;
            }
            GenerateFileName();
            SaveToFile();
        }

        private void SaveToFile()
        {
            foreach (SplittedFile sf in Files)
            {
                FileProvider.FromByteArray(sf.SaveTo, sf.Content, true);
            }
        }
    }
}
