# Kernel #


## C# ##

The kernel **must** be placed in a class inheriting from `OpenCLProgram`.

The kernel **must** be marked with the `[Kernel]` attribute.

The kernel **must** have `WorkItemArgs args` as the first argument.



~~~~
public class DemoKernels : OpenCLProgram
{

    [Kernel] public void DemoKernel(WorkItemArgs args,
                                             [Global] int[] data)
    {
        // Get global ID
        size_t globalID = args.get_global_id(0);

        // Set data in global memory
        data[globalID] = 42;
    }

}
~~~~

## Translated to OpenCL ##
~~~~
__kernel public void DemoKernel(__global int* data)
{
    // Get global ID
    size_t globalID = get_global_id(0);

    // Set data in global memory
    data[globalID] = 42;
}
~~~~

# Barriers #


## C# ##
~~~~
barrier(args, CLK_GLOBAL_MEM_FENCE);
~~~~


## Translated to OpenCL ##
~~~~
barrier(CLK_GLOBAL_MEM_FENCE);
~~~~



# WorkItemArgs args #
Every thread comes with a unique instance of WorkItemArgs, containing information of
workitem id, workgroup id workgroup size etc.

## C# ##
~~~~
args.get_group_id(0); 
args.get_global_offset(0);
args.get_num_groups(0);
args.get_work_dim(0);
args.get_global_size(0);
args.get_local_size(1);
args.get_global_id(2);
args.get_local_id(1);
~~~~


## Translated to OpenCL ##
~~~~
get_group_id(0); 
get_global_offset(0);
get_num_groups(0);
get_work_dim(0);
get_global_size(0);
get_local_size(1);
get_global_id(2);
get_local_id(1);
~~~~


