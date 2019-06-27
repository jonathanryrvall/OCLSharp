# Documentation #

## Kernel ##


### C# ###

The kernel **must** be placed in a class inheriting from `OpenCLProgram`.

The kernel **must** be marked with the `[Kernel]` attribute.

The kernel **must**  `WorkItemArgs args` as the first argument.



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

### Translated to OpenCL ###
~~~~
__kernel public void DemoKernel(__global int* data)
{
    // Get global ID
    size_t globalID = get_global_id(0);

    // Set data in global memory
    data[globalID] = 42;
}
~~~~

## Barriers ##

Barrier in OpenCL:

~~~~
barrier(CLK_GLOBAL_MEM_FENCE);
~~~~


Barrier in C#:

~~~~
barrier(args, CLK_GLOBAL_MEM_FENCE);
~~~~


## WorkItemArgs args ##

### C# ###
~~~~
args.get_global_id(0);
args.get_local_id(1);
args.get_group_id(0);
~~~~


### Translated to OpenCL ###
~~~~
get_global_id(0);
get_local_id(1);
get_group_id(0);
~~~~


