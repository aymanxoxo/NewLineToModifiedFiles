using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace BeforeFileSavePackage
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_string, PackageAutoLoadFlags.BackgroundLoad)]
    [Guid(PackageGuidString)]
    public sealed class BeforeFileSavePackage : AsyncPackage
    {
        public const string PackageGuidString = "15fcd3e1-3c5a-4cc2-95b6-f68bfcd26974";

        #region Package Members

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            IVsOutputWindow outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            new RunningDocTableEvents(this).BeforeSave += BeforeSave;
        }

        private void BeforeSave(object sender, Document document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (document.IsValidFile() && !document.EndsWithEmptyLine())
            {
                document.AddEmptyLineAtEndOfFile();
            }
        }
        #endregion
    }
}
