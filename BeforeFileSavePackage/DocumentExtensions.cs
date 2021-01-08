using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeFileSavePackage
{
    public static class DocumentExtensions
    {
        private static string[] validFileExtensions = new[] { ".cs", ".ts" };

        public static bool IsValidFile(this Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var file = new FileInfo(document.FullName);
            
            if (validFileExtensions.Contains(file.Extension))
            {
                return true;
            }

            return false;
        }

        public static bool EndsWithEmptyLine(this Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            var allLines = File.ReadAllLines(document.FullName);
            if (string.IsNullOrEmpty(allLines[allLines.Length - 1]))
            {
                return true;
            }

            return false;
        }

        public static void AddEmptyLineAtEndOfFile(this Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            ((TextSelection)document.ProjectItem.Document.Selection).Insert(Environment.NewLine);
        }
    }
}
