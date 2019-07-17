using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JDPDO.Mittuntur
{
    class ManagedDirectory : Directory
    {
        /// <summary>
        /// Stores the manager of this directory.
        /// </summary>
        private IProtocolManager manager;

        /// <summary>
        /// Stores the first initialized InstanceRegister object.
        /// </summary>
        private InstanceRegister firstInstanceRegister;

        public override DateTime Created { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ManagedDirectory(string path, IProtocolManager manager) : base(path)
        {
            /* 
             * Init instance variables.
             */
            this.manager = manager;

            /* 
             * Init type variables.
             */
            // Get first instance register object for instance storing.
            firstInstanceRegister = InstanceRegister.GetFirstInstanceRegister();
            // Store this instance into instance register.
            firstInstanceRegister.RegisterInstance(manager.InstanceType, path, this);
        }

        public override bool Delete()
        {
            if (EnumerateChildren() == null) return manager.Delete(this);
            return false;
        }

        public override bool Delete(bool recusive)
        {
            if (recusive) return manager.Delete(this);
            return Delete();
        }

        private bool DeleteChild(IFileSystemItem child)
        {
            if (child.IsDirectory())
            {
                if ((child as Directory).EnumerateChildren() == null) return manager.Delete(child);
                return false;
            }
            return manager.Delete(child);
        }

        public override bool DeleteChild(IFileSystemItem child, bool recusive)
        {
            if (recusive) manager.Delete(child);
            return DeleteChild(child);
        }

        public override IEnumerable<IFileSystemItem> EnumerateChildren() => manager.EnumerateChildren(this);

        public override IEnumerable<Directory> EnumerateDirectories()
        {
            IEnumerable<IFileSystemItem> items = EnumerateChildren();
            List<Directory> directories = new List<Directory>();
            foreach (IFileSystemItem item in items)
            {
                if (item.IsDirectory()) directories.Add(item as Directory);
            }
            return directories;
        }

        public override IEnumerable<File> EnumerateFiles()
        {
            IEnumerable<IFileSystemItem> items = EnumerateChildren();
            List<File> files = new List<File>();
            foreach (IFileSystemItem item in items)
            {
                if (!item.IsDirectory()) files.Add(item as File);
            }
            return files;
        }

        public override IFileSystemItem[] GetChildren()
        {
            return EnumerateChildren() as IFileSystemItem[];
        }

        public override Directory[] GetDirectories()
        {
            return EnumerateDirectories() as Directory[];
        }

        public override File[] GetFiles()
        {
            return EnumerateFiles() as File[];
        }

        public override MemoryStream GetFileStream(File child)
        {
            return manager.GetFileStream(child);
        }

        public override bool CreateChildDirectory(string name)
        {
            return manager.CreateDirectory(new ManagedDirectory(Path.Combine(GetUri().AbsolutePath, name), manager));
        }
    }
}
