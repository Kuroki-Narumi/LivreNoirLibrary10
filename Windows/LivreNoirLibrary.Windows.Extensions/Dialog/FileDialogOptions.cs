using System.IO;
using Microsoft.Win32;

namespace LivreNoirLibrary.Windows
{
    public class FileDialogOptions
    {
        internal static FileDialogOptions Default { get; } = new();

        /// <summary>
        /// Gets or sets the file dialog box title.
        /// </summary>
        /// <returns>
        /// The file dialog box title. The default value is an empty <see cref="string"/> ("").
        /// </returns>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the initial directory displayed by the file dialog box.
        /// </summary>
        /// <returns>
        /// The initial directory displayed by the file dialog box. The default is an empty <see cref="string"/> ("").
        /// </returns>
        public string? InitialDirectory { get; set; }

        /// <summary>
        /// Gets or sets a string containing the file name selected in the file dialog box.
        /// </summary>
        /// <returns>
        /// The file name selected in the file dialog box. The default value is an empty <see cref="string"/> ("").
        /// </returns>
        public string? FileName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a path that does not exist.
        /// </summary>
        /// <returns>
        /// <see cref="bool">true</see> if the dialog box displays a warning when the user specifies a path that does not exist; otherwise, <see cref="bool">false</see>. The default value is <see cref="bool">true</see>.
        /// </returns>
        public bool CheckPathExesits { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box allows multiple files to be selected.
        /// </summary>
        /// <returns>
        /// <see cref="bool">true</see> if the dialog box allows multiple files to be selected together or concurrently; otherwise, <see cref="bool">false</see>. The default value is <see cref="bool">false</see>.
        /// </returns>
        public bool Multiselect { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a file name that does not exist.
        /// </summary>
        /// <returns>
        /// <see cref="bool">true</see> if the dialog box displays a warning when the user specifies a file name that does not exist; otherwise, <see cref="bool">false</see>. The default value is <see cref="bool">true</see>.
        /// </returns>
        public bool CheckFileExists { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the Save As dialog box displays a warning if the user specifies a file name that already exists.
        /// </summary>
        /// <returns>
        /// <see cref="bool">true</see> if the dialog box prompts the user before overwriting an existing file if the user specifies a file name that already exists; <see cref="bool">false</see> if the dialog box automatically overwrites the existing file without prompting the user for permission. The default value is <see cref="bool">true</see>.
        /// </returns>
        public bool OverwritePrompt { get; set; } = true;

        public static FileDialogOptions WithInitialPath(string? path, SetInitialOptions options = SetInitialOptions.None)
        {
            FileDialogOptions op = new();
            op.SetInitialPath(path, options);
            return op;
        }

        public void SetInitialPath(string? path, SetInitialOptions options = SetInitialOptions.None)
        {
            var dir = path;
            while (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                dir = Path.GetDirectoryName(path);
            }
            InitialDirectory = dir;
            if (options is not SetInitialOptions.IsDirectory)
            {
                FileName = options is SetInitialOptions.IncludesExtension ? Path.GetFileName(path) : Path.GetFileNameWithoutExtension(path);
            }
        }

        public void Apply(OpenFileDialog dialog)
        {
            ApplyBase(dialog);
            dialog.Multiselect = Multiselect;
            dialog.CheckFileExists = CheckFileExists;
        }

        public void Apply(SaveFileDialog dialog)
        {
            ApplyBase(dialog);
            dialog.OverwritePrompt = OverwritePrompt;
        }

        private void ApplyBase(FileDialog dialog)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                dialog.Title = Title;
            }
            if (string.IsNullOrEmpty(InitialDirectory))
            {
                dialog.RestoreDirectory = true;
            }
            else
            {
                dialog.RestoreDirectory = false;
                dialog.InitialDirectory = InitialDirectory;
            }
            if (!string.IsNullOrEmpty(FileName))
            {
                dialog.FileName = FileName;
            }
            dialog.CheckPathExists = CheckPathExesits;
        }

        public void Apply(OpenFolderDialog dialog)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                dialog.Title = Title;
            }
            if (!string.IsNullOrEmpty(InitialDirectory))
            {
                dialog.InitialDirectory = dialog.FolderName = InitialDirectory;
            }
        }
    }
}
