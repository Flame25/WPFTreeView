using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace WPFTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Default Constructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region On Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var drive in Directory.GetLogicalDrives()) 
            {
                var item = new TreeViewItem()
                {
                    Header = drive,
                    Tag = drive
                };
                item.Items.Add(null);
                item.Expanded += Folder_Expanded; 
                FolderView.Items.Add(item); 
            }
        }
        #endregion

        #region Folder Expanded
        private void Folder_Expanded(Object sender, RoutedEventArgs e)
        {
            #region Inital Check
            var item = (TreeViewItem)sender;
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            item.Items.Clear();
            
            var fullPath = (string)item.Tag;
            #endregion

            #region Get Directories
            var dirs = new List<string>();
           
            try
            {
                var dir = Directory.GetDirectories(fullPath); 

                if(dir.Length > 0)
                {
                    dirs.AddRange(dir); 
                }
            }
            catch { }
            
            dirs.ForEach(dirPath =>
            {
                var subItem = new TreeViewItem()
                {
                    Header = GetFolderName(dirPath),
                    Tag = dirPath
                };


                subItem.Items.Add(null); 
                subItem.Expanded += Folder_Expanded;
                item.Items.Add(subItem); 
            });

            #endregion

            #region Get Files
            var fs = new List<string>();

            try
            {
                var files = Directory.GetFiles(fullPath);

                if (files.Length > 0)
                {
                    fs.AddRange(files);
                }
            }
            catch { }

            fs.ForEach(filePath =>
            {
                var subItem = new TreeViewItem()
                {
                    Header = GetFolderName(filePath),
                    Tag = filePath
                };
                subItem.Expanded += Folder_Expanded;
                item.Items.Add(subItem);
            });
            #endregion
        }
        #endregion

        #region Helpers
        public static string GetFolderName(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            var normalized = s.Replace('/', '\\');

            var lastIndex = normalized.LastIndexOf("\\");

            if (lastIndex < 0)
                return s;

            return normalized.Substring(lastIndex + 1);
        }

        #endregion
    }
}
