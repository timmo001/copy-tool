using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Copy
{
    public class Copy
    {
        public static void CopyFile(string sourceFileName, string destFileName)
        {
            try
            {
                File.Copy(sourceFileName, destFileName, true);
            }
            catch (Exception ex)
            {
                try
                {
                    AutoClosingMessageBox.Show(Path.GetFileName(sourceFileName) + " failed to copy. Attempting the rename method...\n\nAdditional Details: \n" + ex.Message,
                        "Using alternate method", 3000);
                    File.Move(destFileName, destFileName + ".DELETE");
                    File.Copy(sourceFileName, destFileName, true);
                    Thread.Sleep(1000);
                    try
                    {
                        File.Delete(destFileName + ".DELETE");
                    }
                    catch (Exception ex2)
                    {
                        DialogResult dialogResult2 = MessageBox.Show(Path.GetFileName(destFileName) + " was copied but " + Path.GetFileName(destFileName) + ".DELETE failed to delete afterwards. Would you like to open the directory to try deleting the renamed file later?\n\nAdditional Details: \n" + ex2.Message, "Delete Failure", MessageBoxButtons.YesNo);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            Process.Start("explorer.exe", "-p " + string.Format("/Select, \"{0}\"", Path.GetDirectoryName(destFileName)));
                        }
                    }
                }
                catch (Exception ex3)
                {
                    DialogResult dialogResultFail = MessageBox.Show(Path.GetFileName(sourceFileName) + " failed to copy. Show Folders?\n\nAdditional Details: \n" + ex3.Message, "Copy Failure", MessageBoxButtons.YesNo);
                    if (dialogResultFail == DialogResult.Yes)
                    {
                        Process.Start("explorer.exe", "-p " + string.Format("/Select, \"{0}\"", Path.GetDirectoryName(sourceFileName)));
                        Process.Start("explorer.exe", "-p " + string.Format("/Select, \"{0}\"", Path.GetDirectoryName(destFileName)));
                    }
                }
            }
        }

        public static void CopyDirectory(string sourcePath, string targetPath)
        {
            // Check if directory exists
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            if (Directory.Exists(sourcePath))
            {
                string[] files = Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    string fileName = Path.GetFileName(s);
                    string destFile = Path.Combine(targetPath, fileName);
                    try
                    {
                        File.Copy(s, destFile, true);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(s + " failed to copy. Check that it isn't in use and try again.", "Copy Failure");
                    }
                }
            }
        }


        public static void MoveFile(string sourceFileName, string destFileName, bool debugMode)
        {
            try
            {
                File.Move(sourceFileName, destFileName);
            }
            catch (Exception e)
            {
                if (debugMode)
                {
                    MessageBox.Show(sourceFileName + " failed to move. Check that it isn't in use and try again." + "\n\n" + e, "Move Failure");
                }
            }
        }

        public static void MoveDirectory(string sourcePath, string targetPath, bool debugMode)
        {
            // Check if target directory does not exist
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            // Check if source directory exists
            if (Directory.Exists(sourcePath))
            {
                try
                {
                    Directory.Move(sourcePath, targetPath);
                }
                catch (Exception e)
                {
                    if (debugMode)
                    {
                        MessageBox.Show(sourcePath + " failed to move. Check that it isn't in use and try again." + "\n\n" + e, "Move Failure");
                    }
                }
            }
        }


        public static void DeleteFile(string path, bool debugMode)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                if (debugMode)
                {
                    MessageBox.Show(path + " failed to delete. Check that it isn't in use and try again." + "\n\n" + e, "Delete Failure");
                }
            }
        }

        public static void DeleteDirectory(string path, bool debugMode)
        {
            // Check if directory exists
            if (Directory.Exists(path))
            {
                try
                {
                    string[] files = Directory.GetFiles(path);
                    string[] dirs = Directory.GetDirectories(path);

                    foreach (string file in files)
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }
                    foreach (string dir in dirs)
                    {
                        DeleteDirectory(dir, debugMode);
                    }
                    Directory.Delete(path);
                }
                catch (Exception e)
                {
                    if (debugMode)
                    {
                        MessageBox.Show(path + " failed to delete. Check that it isn't in use and try again." + "\n\n" + e, "Delete Failure");
                    }
                }
            }
        }

        public static void CreateFile(string path, bool debugMode)
        {
            try
            {
                File.Create(path);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to create " + path + ".", "Create Failure");
                if (debugMode)
                {
                    MessageBox.Show("Create failed - " + path + "\n\n" + e);
                }
            }
        }

        public static void CreateDirectory(string path, bool debugMode)
        {
            // Check if directory exists
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to create " + path + ".", "Create Failure");
                    if (debugMode)
                    {
                        MessageBox.Show("Create failed - " + path + "\n\n" + e);
                    }
                }
            }
        }

        public static bool TestDirectory(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                di.GetFiles("*");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
