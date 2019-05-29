using System;
using System.Collections.Generic;
using System.Text;
using IO = System.IO;

namespace JDPDO.Mittuntur
{
    /// <summary>
    /// Represents a tree with a root directoy and subordinated files and directories.
    /// </summary>
    public class FileTree<T> where T : Directory
    {
        // The root directory.
        private T startDirectory;
        // The current layer of main FileTree object.
        private readonly int layer;
        // List of child roots.
        private List<FileTree<T>> subTrees;

        /// <summary>
        /// Retruns and sets the RootDirectory of the fileTree object,
        /// but to set the value may causes problems, because the object is not reinitialized.
        /// </summary>
        public T RootDirectory
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
        public FileTree(T startDirectory)
        {
            this.startDirectory = startDirectory;
            subTrees = new List<FileTree<T>>();
            layer = 0;

            Initialize();
        }

        /// <summary>
        /// Creates a new file tree object using custom root directory path string.
        /// </summary>
        /// <param name="path">The path of custom root directory in file system.</param>
        public FileTree(string path)
        {
            startDirectory = (T)Activator.CreateInstance(typeof(T), path);
            subTrees = new List<FileTree<T>>();
            layer = 0;

            Initialize();
        }

        /// <summary>
        /// Creats a new file tree object with given path string and layer since custom root directory.
        /// </summary>
        /// <param name="startDirectory">The path of directory in file system.</param>
        /// <param name="layer">The layer since custom root directory.</param>
        private FileTree(T startDirectory, int layer)
        {
            this.startDirectory = startDirectory;
            subTrees = new List<FileTree<T>>();
            this.layer = layer;

            Initialize();
        }

        /// <summary>
        /// Initializes the current FileTree object recursively.
        /// </summary>
        private void Initialize()
        {
            try
            {
                foreach (T directory in startDirectory.GetDirectories())
                {
                    subTrees.Add(new FileTree<T>(directory, layer + 1));
                }
            }
            catch (Exception e)
            {
                ExeptionHandler.NewException(e);
            }
            subTrees.Sort();
        }

        /// <summary>
        /// Returns all direct following subtree items.
        /// </summary>
        /// <returns></returns>
        public FileTree<T>[] GetSubFileTrees() => subTrees.ToArray();


        /// <summary>
        /// Retruns a specified subtree item instance.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Retruns null, if no object was found.</returns>
        public FileTree<T> GetSubFileTree(string path)
        {
            if (String.IsNullOrEmpty(path)) return null;
            return subTrees.Find(x => x == new FileTree<T>(path));
        }

        /// <summary>
        /// Returns all child elements of the current layer.
        /// </summary>
        /// <returns>Returns FileSystemInfo object array.</returns>
        public IFileSystemItem[] GetChildren() => startDirectory.GetChildren();

        /// <summary>
        /// Returns all directory elements of the current layer.
        /// </summary>
        /// <returns>Returns DirectoyInfo object array.</returns>
        public Directory[] GetSubDirectories() => startDirectory.GetDirectories();

        /// <summary>
        /// Returns all file elements of the current layer.
        /// </summary>
        /// <returns>Returns FileInfo object array.</returns>
        public File[] GetSubFiles() => startDirectory.GetFiles();

        /// <summary>
        /// Searches recursively for all files and directories with the given name.
        /// </summary>
        /// <param name="fileName">Name of the element to be searched.</param>
        /// <returns>First found file object or if no object was found null.</returns>
        public IEnumerable<IFileSystemItem> SearchChildByName(string fileName)
        {
            // List with elements for the search.
            List<IFileSystemItem> children = new List<IFileSystemItem>(GetChildren());
            // List with found elements.
            List<IFileSystemItem> foundElements = new List<IFileSystemItem>();
            // The compare item.
            File searchObject = new File(fileName);
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
