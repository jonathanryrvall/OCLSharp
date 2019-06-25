using OCLSharp.OpenCL.Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OCLSharp
{
    /// <summary>
    /// Emulates C# code as if it was OpenCL
    /// Note that this is VERY inefficient and slow and only meant for debugging purposes
    /// </summary>
    public class Emulator<TProgram> where TProgram : OpenCLProgram, new()
    {
        private int[] workGroupSize;
        private int[] ndRange;
        private int[] workGroupCount;

        /// <summary>
        /// Create a new emulator
        /// </summary>
        public Emulator(int[] workGroupSize,
                        int[] ndRange)
        {
            // Validate input
            if (workGroupSize.Length > 3)
            {
                throw new ArgumentException("Cannot have workgroups with more than 3 dimensions");
            }


            // Set local fields
            this.workGroupSize = PadDimArray(workGroupSize);
            this.ndRange = PadDimArray(ndRange);

            // Calculate how many workgroups there are suppose to be
            CalculateWorkGroupCount();

        }

        /// <summary>
        /// Calculate work group count in all dimensions
        /// </summary>
        private void CalculateWorkGroupCount()
        {
            workGroupCount = new int[3];
            workGroupCount[0] = ndRange[0] / workGroupSize[0];
            workGroupCount[1] = ndRange[1] / workGroupSize[1];
            workGroupCount[2] = ndRange[2] / workGroupSize[2];
        }

        /// <summary>
        /// Add padding to dimension array and set empty elements to 1
        /// </summary>
        private int[] PadDimArray(int[] arr)
        {
            int[] result = Enumerable.Repeat(1, 3).ToArray();
            Array.Copy(arr, result, arr.Length);
            return result;
        }

        private void ValidateParams()
        {

        }

        /// <summary>
        /// Run kernel by name
        /// </summary>
        public void Run(string kernelName, object[] kernelArgs)
        {
            // Create new program
            TProgram program = new TProgram();
            program.Initialize(workGroupSize, 
                               workGroupCount, 
                               ndRange);

            // Iterate through all workgroups and start them one by one
            foreach (int[] workGroupID in GetWorkGroupIDs())
            {
                RunWorkGroup(workGroupID,
                             kernelName,
                             program,
                             kernelArgs);

            }
        }

        /// <summary>
        /// Run a specific work group in a kernel on a program
        /// </summary>
        private void RunWorkGroup(int[] workGroupID,
                                  string kernelName,
                                  TProgram program,
                                  object[] kernelArgs)
        {
            List<Task> tasks = new List<Task>();

            int maxThreads = workGroupSize[0] * workGroupSize[1] * workGroupSize[2];
            maxThreads += 2;
            ThreadPool.SetMaxThreads(maxThreads, maxThreads);

            // Start all work items in a work group
            foreach (WorkItemArgs workItemArgs in GetWorkItemArgs(workGroupID))
            {
                WorkItemArgs workItemArgsC = workItemArgs;

             

                // Create new task
                Task task = new Task(() => RunWorkItem(workItemArgs,
                                              kernelName,
                                              kernelArgs,
                                              program));

                // Add task to collection
                tasks.Add(task);

                // Start task
                task.Start();
            }

            // Wait for all threads to finish
            Task.WaitAll(tasks.ToArray());
   
        }

        /// <summary>
        /// Start a work item
        /// </summary>
        private void RunWorkItem(WorkItemArgs workItemArgs,
                                   string kernelName,
                                   object[] kernelArgs,
                                   TProgram program)
        {
            // Get program type
            Type type = typeof(TProgram);

            // Get method that represent that kernel
            MethodInfo theMethod = type.GetMethod(kernelName);

            // Get user arguments for kernel
            object[] args = CombineArgs(workItemArgs, kernelArgs);

            // Invoke method
            theMethod.Invoke(program, args);
        }

        /// <summary>
        /// Combine work items args with kernel args
        /// </summary>
        private object[] CombineArgs(WorkItemArgs workItemArgs, object[] kernelArgs)
        {
            // Create new args array
            object[] args = new object[kernelArgs.Length + 1];

            // Set first argument
            args[0] = workItemArgs;

            // Copy user arguments
            Array.Copy(kernelArgs, 0, args, 1, kernelArgs.Length);

            return args;
        }

        /// <summary>
        /// Get work items arguments
        /// </summary>
        private IEnumerable<WorkItemArgs> GetWorkItemArgs(int[] workGroupID)
        {
            for (int x = 0; x < workGroupSize[0]; x++)
            {
                for (int y = 0; y < workGroupSize[1]; y++)
                {
                    for (int z = 0; z < workGroupSize[2]; z++)
                    {
                        // Global ID
                        int[] globalID = new int[]
                        {
                            x + (workGroupID[0] * workGroupSize[0]),
                            y + (workGroupID[1] * workGroupSize[1]),
                            z + (workGroupID[2] * workGroupSize[2])
                        };

                        // Local ID
                        int[] localID = new int[] { x, y, z };

                        // Create new work item args
                        yield return new WorkItemArgs(ndRange,
                            workGroupSize,
                            workGroupID,
                            localID,
                            globalID);

                    }
                }
            }
        }

        /// <summary>
        /// Get work group IDs
        /// </summary>
        private IEnumerable<int[]> GetWorkGroupIDs()
        {
            for (int x = 0; x < workGroupCount[0]; x++)
            {
                for (int y = 0; y < workGroupCount[1]; y++)
                {
                    for (int z = 0; z < workGroupCount[2]; z++)
                    {
                        yield return new int[] { x, y, z };
                    }
                }
            }
        }

    }
}
