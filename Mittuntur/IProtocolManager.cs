using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JDPDO.Mittuntur
{
    interface IProtocolManager
    {
        /// <summary>
        /// Provides the directory type of the manager.
        /// </summary>
        InstanceType InstanceType { get; }

        /// <summary>
        /// Copies an item only on the system of the managed server.
        /// </summary>
        /// <param name="item">The file or directory to copy.</param>
        /// <param name="directory">The destination directory.</param>
        /// <returns>True if item was copied successfully.</returns>
        bool Copy(IFileSystemItem item, Directory directory, bool overwrite);

        /// <summary>
        /// Deletes an item on the managed server.
        /// </summary>
        /// <param name="item">The file or directory to be deleted.</param>
        /// <returns>True if deletion was success.</returns>
        bool Delete(IFileSystemItem item);

        /// <summary>
        /// Inserts an item into the managed server. 
        /// </summary>
        /// <param name="item">The file or directory to insert.</param>
        /// <param name="directory">The destination directory.</param>
        /// <param name="overwrite">Determine if existing files and directories should be overwritten.</param>
        /// <returns>True if item was pasted successfully.</returns>
        bool AddItem(IFileSystemItem item, Directory directory, bool overwrite = false);

        /// <summary>
        /// Inserts an file into the managed server using a Stream contain
        /// </summary>
        /// <param name="stream">The file or directory to insert, represented by a stream.</param>
        /// <param name="name">The name of the file to be created.</param>
        /// <param name="directory">The destination directory.</param>
        /// <param name="overwrite">Determine if existing files and directories should be overwritten.</param>
        /// <returns>True if item was pasted successfully.</returns>
        bool AddFile(Stream stream, string name, Directory directory, bool overwrite = false);

        /// <summary>
        /// Inserts an file into the managed server. 
        /// </summary>
        /// <param name="item">The file or directory to insert.</param>
        /// <param name="directory">The destination directory.</param>
        /// <param name="overwrite">Determine if existing files and directories should be overwritten.</param>
        /// <returns>True if item was pasted successfully.</returns>

        bool AddFile(File item, Directory directory, bool overwrite);

        /// <summary>
        /// Renames an item on the managed server.
        /// </summary>
        /// <param name="item">The item to rename.</param>
        /// <param name="newName">The new name of the item.</param>
        /// <param name="overwrite">Determine if existing file or directory should be overwritten.</param>
        /// <returns>True if the item was renamed successfully.</returns>
        bool Rename(IFileSystemItem item, string newName, bool overwrite = false);

        /// <summary>
        /// Moves an item only on the system of the managed server.
        /// </summary>
        /// <param name="item">The file or directory to move.</param>
        /// <param name="directory">The destination directory.</param>
        /// <param name="overwrite">Determine if existing files and directories should be overwritten.</param>
        /// <returns>True if item was moved successfully.</returns>
        bool Move(IFileSystemItem item, Directory directory, bool overwrite = false);

        /// <summary>
        /// Creates an directory on the managed server. If the preceding directories do not exist, then they are created.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        bool CreateDirectory(Directory directory);

        /// <summary>
        /// Provides the 'FileStream' for a file.
        /// </summary>
        /// <param name="item">The file to stream.</param>
        /// <param name="callback">The method to call if download completed.</param>
        /// <returns>'FileStream' object of item.</returns>
        System.IO.MemoryStream GetFileStream(File item, AsyncCallback callback = null);

        /// <summary>
        /// Returns a (downloaded) temporary file reference.
        /// </summary>
        /// <param name="item">The remote file to download and store locally.</param>
        /// <param name="directory">The local directory to store the file.</param>
        /// <returns></returns>
        File SaveFile(File item, Directory directory);

        /// <summary>
        /// Enumerates all children of an directory.
        /// </summary>
        /// <param name="item">The directory which children will be enumerated.</param>
        /// <returns></returns>
        IEnumerable<IFileSystemItem> EnumerateChildren(Directory item);
    }
}
