using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.OpenCL.Program
{
    /// <summary>
    /// Base class for an OpenCL program
    /// </summary>
    public partial class OpenCLProgram
    {
        //private int[] workGroupSize;
        //private int[] ndRange;
        //private int[] workGroupCount;

        /// <summary>
        /// Set program settings
        /// </summary>
        public void Initialize(int[] workGroupSize,
                               int[] ndRange)
        {
            //if (workGroupSize.Length != 3)
            //{
            //    throw new ArgumentException("Work group size Array must be 3")
            //}
            //this.workGroupSize = workGroupSize;
            //this.ndRange = ndRange;

           // localMem = new Dictionary<string, object>[][][1];


        }

        private struct WorkGroupKey
        {
            public int X;
            public int Y;
            public int Z;
        }

        private Dictionary<string, object>[][][] localMem;

        /// <summary>
        /// Sync local memory between work items
        /// </summary>
        //protected T GetLocalMem<T>(WorkItemArgs args, T def, string tag) 
        //{
        //    WorkGroupKey workGroupKey = new WorkGroupKey();
        //    workGroupKey.X = args.workGroupID[0];
        //    workGroupKey.Y = args.workGroupID[1];
        //    workGroupKey.Z = args.workGroupID[2];

        //    // Lock memory dictionary
        //    lock (localMem)
        //    {
        //        // Add memory to dictionary if it does not exist
        //        if (!localMem.ContainsKey(workGroupKey))
        //        {
        //            Dictionary<string, object> tagDictionary
        //            localMem.Add(tag, def);
        //        }

        //        // Memory already exist in dictionary
        //        return (T)localMem[tag];
        //    }
        //}


    }
}
