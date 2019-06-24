using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp
{
    /// <summary>
    /// Arguments for work items, used only for emulation
    /// </summary>
    public class WorkItemArgs
    {
        public int[] ndRange;
        public int[] workGroupID;
        public int[] localID;
        public int[] globalID;

        public WorkItemArgs(int[] ndRange,
                            int[] workGroupID,
                            int[] localID,
                            int[] globalID)
        {
            this.ndRange = ndRange;
            this.workGroupID = workGroupID;
            this.localID = localID;
            this.globalID = globalID;

        }

        public int get_global_size(int dim)
        {
            return ndRange[dim];
        }

        public int get_global_id(int dim)
        {
            return globalID[dim];
        }

        public int GetLocalID(int dim)
        {
            return localID[dim];
        }


    }
}
