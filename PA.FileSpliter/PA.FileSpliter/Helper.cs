using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PA.FileSplitter
{
    public static class Helper
    {
        public static void ShowErrors(List<String> errors)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string err in errors)
                sb.AppendLine(err);
            MessageBox.Show(sb.ToString());
            return;
        }
    }
}
