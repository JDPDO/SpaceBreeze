using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;

namespace FileManageAndBackupBot
{
    /// <summary>
    /// Represents a tree with a root directory and subordinated files and generic directories objects.
    /// </summary>
    public class FileTree<TDir> where TDir : IDirectory<TDir>
    {
        // The root directory.
        protected TDir startDirectory;
        // The current layer of main FileTree object.
        protected int layer;
        // List of child roots.
        protected List<FileTree<TDir>> subTrees;

        /// <summary>
        /// Retruns and sets the RootDirectory of the fileTree object,
        /// but to set the value may causes problems, because the object is not reinitialized.
        /// </summary>
        public TDir RootDirectory
        {
            get
            {
                return startDirectory;
            }
            set
            {
                startDirectory = value;
            }
        }

        /// <summary>
        /// Retruns the number of the current layer since the higest FileTree object.
        /// </summary>
        public int Layer
        {
            get
            {
                return layer;
            }
        }

        /// <summary>
        /// Creates a new file tree object using custom root driectory object.
        /// </summary>
        /// <param name="startDirectory">The managing directory object of root directory.</param>
        public FileTree(TDir startDirectory)
        {
            this.startDirectory = startDirectory;
            subTrees = new List<FileTree<TDir>>();
            layer = 0;

            Initialize();
        }

        /// <summary>
        /// Creates a new file tree object using custom root directory path string.
        /// </summary>
        /// <param name="path">The path of custom root directory in file system.</param>
        public FileTree(string path)
        {
            startDirectory = (TDir)Activator.CreateInstance(typeof(TDir), path);
            subTrees = new List<FileTree<TDir>>();
            layer = 0;

            Initialize();
        }

        /// <summary>
        /// Creats a new file tree object with given path string and layer since custom root directory.
        /// </summary>
        /// <param name="startDirectory">The path of directory in file system.</param>
        /// <param name="layer">The layer since custom root directory.</param>
        protected FileTree(TDir startDirectory, int layer)
        {
            this.startDirectory = startDirectory;
            subTrees = new List<FileTree<TDir>>();
            this.layer = layer;

            Initialize();
        }

        /// <summary>
        /// Initializes the current FileTree object recursively.
        /// </summary>
        protected void Initialize()
        {
            foreach(var directory in startDirectory.GetDirectories())
            {
                subTrees.Add(new FileTree<TDir>(directory, layer + 1));
            }
        }

        public FileTree<TDir>[] GetSubFileTrees() => subTrees.ToArray();

        /// <summary>
        /// Returns all child elements of the current layer.
        /// </summary>
        /// <returns>Returns FileSystemInfo object array.</returns>
        public IO.FileSystemInfo[] GetFileSystemInfos() => startDirectory.GetFileSystemInfos();

        /// <summary>
        /// Returns all directory elements of the current layer.
        /// </summary>
        /// <returns>Returns DirectoyInfo object array.</returns>
        public TDir[] GetDirectories() => startDirectory.GetDirectories();

        /// <summary>
        /// Returns all file elements of the current layer.
        /// </summary>
        /// <returns>Returns FileInfo object array.</returns>
        public IO.FileInfo[] GetFiles() => startDirectory.GetFiles();

        /// <summary>
        /// Searches recursively for all files and directories with the given name.
        /// </summary>
        /// <param name="fileName">Name of the element to be searched.</param>
        /// <returns>First found file object or if no object was found null.</returns>
        public IEnumerable<IO.FileSystemInfo> SearchChildByName(string fileName)
        {
            // List with elements for the search.
            List<IO.FileSystemInfo> children = new List<IO.FileSystemInfo>(GetFileSystemInfos());
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
        protected void PrintLine(int preTabs, string text)
        {
            string tabs = String.Concat(System.Linq.Enumerable.Repeat("  ", preTabs));
            Console.WriteLine(tabs + text);
        }
    }

    public class FileTree : FileTree<LocalDirectory>
    {
        /// <summary>
        /// Creates a new file tree object using custom root driectory object.
        /// </summary>
        /// <param name="startDirectory">The managing directory object of root directory.</param>
        public FileTree(LocalDirectory startDirectory) : base(startDirectory)
        {
            this.startDirectory = startDirectory;
            subTrees = new List<FileTree<LocalDirectory>>();
            layer = 0;

            Initialize();
        }

        /// <summary>
        /// Creates a new file tree object using custom root directory path string.
        /// </summary>
        /// <param name="path">The path of custom root directory in file system.</param>
        public FileTree(string path) : base(path)
        {
            startDirectory = (LocalDirectory)Activator.CreateInstance(typeof(LocalDirectory), path);
            subTrees = new List<FileTree<LocalDirectory>>();
            layer = 0;

            Initialize();
        }

        /// <summary>
        /// Creats a new file tree object with given path string and layer since custom root directory.
        /// </summary>
        /// <param name="startDirectory">The path of directory in file system.</param>
        /// <param name="layer">The layer since custom root directory.</param>
        private FileTree(LocalDirectory startDirectory, int layer) :base(startDirectory, layer)
        {
            this.startDirectory = startDirectory;
            subTrees = new List<FileTree<LocalDirectory>>();
            this.layer = layer;

            Initialize();
        }
    }
}
