using OCLSharp.OpenCL.DataTypes.ScalarDataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Emulation
{
    /// <summary>
    /// Arguments for work items, used only for emulation
    /// </summary>
    public class WorkItemArgs
    {
        public int[] ndRange;
        public int[] workGroupCount;
        public int[] workGroupSize;
        public int[] workGroupID;
        public int[] localID;
        public int[] globalID;

        public WorkItemArgs(int[] ndRange,
                            int[] workGroupCount,
                            int[] workGroupSize,
                            int[] workGroupID,
                            int[] localID,
                            int[] globalID)
        {
            this.ndRange = ndRange;
            this.workGroupCount = workGroupCount;
            this.workGroupSize = workGroupSize;
            this.workGroupID = workGroupID;
            this.localID = localID;
            this.globalID = globalID;

        }

        /// <summary>
        /// Id of work group
        /// </summary>
        public int get_group_id(int dim)
        {
            // TODO: fix this
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the offset values specified in global_work_offset argument to clEnqueueNDRangeKernel. Valid values of dimindx are 0 to get_work_dim() - 1. For other values, get_global_offset() returns 0.
        /// For clEnqueueTask, this returns 0.
        /// </summary>
        public size_t get_global_offset(uint dim)
        {
            // TODO: fix this
            throw new NotImplementedException();
        }

        /// <summary>
        /// Number of work groups
        /// </summary>
        public int get_num_groups(int dim)
        {
            return workGroupCount[dim];
        }


        /// <summary>
        /// Get how many dimensions are in use
        /// </summary>
        public uint get_work_dim(int dim)
        {
            if (ndRange[2] > 1)
            {
                return 3;
            }
            if (ndRange[1] > 1)
            {
                return 2;
            }
            if (ndRange[0] > 1)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// Get size of entire work area
        /// </summary>
        public size_t get_global_size(uint dim)
        {
            return (uint)ndRange[dim];
        }

        /// <summary>
        /// Get size of workgroup
        /// </summary>
        public size_t get_local_size(uint dim)
        {
            return (uint)workGroupSize[dim];
        }

        /// <summary>
        /// Get workitem id withing entire work area
        /// </summary>
        public size_t get_global_id(uint dim)
        {
            return (uint)globalID[dim];
        }

        /// <summary>
        /// Get workitem id within workgroup
        /// </summary>
        public size_t get_local_id(uint dim)
        {
            return (uint)localID[dim];
        }


    }
}
