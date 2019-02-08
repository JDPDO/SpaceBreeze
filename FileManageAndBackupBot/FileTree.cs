using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;

namespace FileManageAndBackupBot
{
    /// <summary>
    /// Represents a tree with a root directoy and subordinated files and directories.
    /// </summary>
    public class FileTree
    {
        // The root directory.
        private IO.DirectoryInfo startDirectory;
        // The current layer of main FileTree object.
        private int layer;
        // List of child roots.
        private List<FileTree> subTrees;

        /// <summary>
        /// Creates a new file tree object using custom root driectory object.
        /// </summary>
        /// <param name="startDirectory">The managing directory object of root directory.</param>
        public FileTree(IO.DirectoryInfo startDirectory)
        {
            this.startDirectory = startDirectory;
            subTrees = new List<FileTree>();
            layer = 0;

            Initialize();
        }

        /// <summary>
        /// Creates a new file tree object using custom root directory path string.
        /// </summary>
        /// <param name="path">The path of custom root directory in file system.</param>
        public FileTree(string path)
        {
            startDirectory = (IO.DirectoryInfo)Activator.CreateInstance(typeof(IO.DirectoryInfo), path);
            subTrees = new List<FileTree>();
            layer = 0;

            Initialize();
        }

        /// <summary>
        /// Creats a new file tree object with given path string and layer since custom root directory.
        /// </summary>
        /// <param name="startDirectory">The path of directory in file system.</param>
        /// <param name="layer">The layer since custom root directory.</param>
        private FileTree(IO.DirectoryInfo startDirectory, int layer)
        {
            this.startDirectory = startDirectory;
            subTrees = new List<FileTree>();
            this.layer = layer;

            Initialize();
        }

        /// <summary>
        /// Initializes the current FileTree object recursively.
        /// </summary>
        private void Initialize()
        {
            foreach(var directory in startDirectory.GetDirectories())
            {
                subTrees.Add(new FileTree(directory, layer + 1));
            }
        }

        public FileTree[] GetSubFileTrees() => subTrees.ToArray();

        /// <summary>
        /// Returns all child elements of the current layer.
        /// </summary>
        /// <returns>Returns FileSystemInfo object array.</returns>
        public IO.FileSystemInfo[] GetChildren() => startDirectory.GetFileSystemInfos();

        /// <summary>
        /// Returns all directory elements of the current layer.
        /// </summary>
        /// <returns>Returns DirectoyInfo object array.</returns>
        public IO.DirectoryInfo[] GetSubDirectories() => startDirectory.GetDirectories();

        /// <summary>
        /// Returns all file elements of the current layer.
        /// </summary>
        /// <returns>Returns FileInfo object array.</returns>
        public IO.FileInfo[] GetSubFiles() => startDirectory.GetFiles();

        /// <summary>
        /// Searches recursively for all files and directories with the given name.
        /// </summary>
        /// <param name="fileName">Name of the element to be searched.</param>
        /// <returns>First found file object or if no object was found null.</returns>
        public IEnumerable<IO.FileSystemInfo> SearchChildByName(string fileName)
        {
            // List with elements for the search.
            List<IO.FileSystemInfo> children = new List<IO.FileSystemInfo>(GetChildren());
            // List with found elements.
            List<IO.FileSystemInfo> foundElements = new List<IO.FileSystemInfo>();
            // The compare item.
            IO.FileSystemInfo searchObject = new IO.FileInfo(fileName);
            // If object in {children} found its index.
            int index = children.BinarySearch(searchObject);
            // Check if index is object and adds it to {foundElements}.
            if (index >= 0) foundElements.Add(children[index]);
            // Get matching objects of all following {subTrees}.
            foreach (var subTree in subTrees)
            {
                foundElements.AddRange(subTree.SearchChildByName(fileName));
            }
            return foundElements;
        }



        /// <summary>
        /// Prints all subfiles and subdirectories of current one hierarchically in output console.
        /// </summary>
        public void PrintFileTree()
        {
            foreach (var child in subTrees)
            {
                PrintLine(layer, child.startDirectory.Name);
                child.PrintFileTree();
            }
        }

        /// <summary>
        /// Prints new line in output console with prewritten tabs.
        /// </summary>
        /// <param name="preTabs">Number of prewritten tabs.</param>
        /// <param name="text">Text to write in the output console.</param>
        private void PrintLine(int preTabs, string text)
        {
            string tabs = String.Concat(System.Linq.Enumerable.Repeat("  ", preTabs));
            Console.WriteLine(tabs + text);
        }
    }
}
