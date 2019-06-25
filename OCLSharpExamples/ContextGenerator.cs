using Cloo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCLSharpExamples
{
    public class ContextGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ComputeContext GetContext()
        {
            var platforms = ComputePlatform.Platforms;

            // Get platform to work with
            ComputePlatform platform = GetPlatform();

            // Get device to work with
            ComputeDevice device = GetDevice(platform);

            // Create a new OpenCL context
            ComputeContextPropertyList properties = new ComputeContextPropertyList(platform);
            return new ComputeContext(new ComputeDevice[] { device }, properties, null, IntPtr.Zero);
        }

        /// <summary>
        /// Allow user to select a platform
        /// </summary>
        private ComputePlatform GetPlatform()
        {
            Console.WriteLine("Choose a platform:");

            // List all available platforms
            for (int p = 0; p < ComputePlatform.Platforms.Count; p++)
            {
                var platform = ComputePlatform.Platforms[p];
                Console.WriteLine($"[{p}] {platform.Name}");
            }

            // Get platform index
            int platformIndex = int.Parse(Console.ReadLine());

            Console.WriteLine();

            // Return platform for specified index 
            return ComputePlatform.Platforms[platformIndex];
        }

        /// <summary>
        /// Allow user to select a device
        /// </summary>
        private ComputeDevice GetDevice(ComputePlatform platform)
        {
            Console.WriteLine("Choose a device:");

            // List all available devices
            for (int d = 0; d < platform.Devices.Count; d++)
            {
                var device = platform.Devices[d];
                Console.WriteLine($"[{d}] {device.Name}");
            }

            // Get device index
            int deviceIndex = int.Parse(Console.ReadLine());

            Console.WriteLine();

            // Return device for specified index 
            return platform.Devices[deviceIndex];
        }
    }
}
