using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OCLSharp.OpenCL.Program
{
    /// <summary>
    /// Base class for an OpenCL program
    /// </summary>
    public partial class OpenCLProgram
    {
        protected const int CLK_LOCAL_MEM_FENCE = 0;
        protected const int CLK_GLOBAL_MEM_FENCE = 0;

        private int[] workGroupSize;
        private int[] workGroupCount;

        private Barrier[,,] barriers;
        private Dictionary<string, object>[,,] localMemory;

        //private int[] ndRange;
        //private int[] workGroupCount;

        /// <summary>
        /// Set program settings
        /// </summary>
        public void Initialize(int[] workGroupSize,
                               int[] workGroupCount,
                               int[] ndRange)
        {
            //if (workGroupSize.Length != 3)
            //{
            //    throw new ArgumentException("Work group size Array must be 3")
            //}
            this.workGroupSize = workGroupSize;
            this.workGroupCount = workGroupCount;
            // this.ndRange = ndRange;

            InitLocalMemory();

            InitBarriers();
        }

        /// <summary>
        /// Initialize barriers
        /// </summary>
        private void InitBarriers()
        {
            // Create array
            barriers = new Barrier[workGroupCount[0],
                                   workGroupCount[1],
                                   workGroupCount[2]];
            // Calculate participants
            int participants = workGroupSize[0] *
                                   workGroupSize[1] *
                                   workGroupSize[2];

            // Init array items
            for (int x = 0; x < workGroupCount[0]; x++)
            {
                for (int y = 0; y < workGroupCount[1]; y++)
                {
                    for (int z = 0; z < workGroupCount[2]; z++)
                    {
                        barriers[x, y, z] = new Barrier(participants);
                    }
                }
            }
        }

        /// <summary>
        /// Initialize local memory dictionary
        /// </summary>
        private void InitLocalMemory()
        {
            // Create array
            localMemory = new Dictionary<string, object>[workGroupCount[0],
                                                         workGroupCount[1],
                                                         workGroupCount[2]];
         
            // Init array items
            for (int x = 0; x < workGroupCount[0]; x++)
            {
                for (int y = 0; y < workGroupCount[1]; y++)
                {
                    for (int z = 0; z < workGroupCount[2]; z++)
                    {
                        localMemory[x, y, z] = new Dictionary<string, object>();
                    }
                }
            }
        }


        /// <summary>
        /// Barrier!
        /// </summary>
        protected void barrier(WorkItemArgs args, int fence)
        {
            barriers[args.workGroupID[0],
                     args.workGroupID[1],
                     args.workGroupID[2]].SignalAndWait();
        }

        /// <summary>
        /// Sync local memory between work items
        /// </summary>
        protected T GetLocalMem<T>(WorkItemArgs args, T def, string tag)
        {
            // Lock memory dictionary
            lock (localMemory)
            {
                var dictionary = localMemory[args.workGroupID[0],
                                             args.workGroupID[1],
                                             args.workGroupID[2]];

                // Add memory to dictionary if it does not exist
                if (!dictionary.ContainsKey(tag))
                {
                    dictionary.Add(tag, def);
                }

                // Memory already exist in dictionary
                return (T)dictionary[tag];
            }
        }


    }
}
