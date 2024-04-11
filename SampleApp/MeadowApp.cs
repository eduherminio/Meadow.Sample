using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp
{
    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            onboardLed = new RgbPwmLed(
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);

            return base.Initialize();
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            ShowTelemetry();

            return CycleColors(TimeSpan.FromMilliseconds(1000));
        }

        async Task CycleColors(TimeSpan duration)
        {
            Resolver.Log.Info("Cycle colors...");

            while (true)
            {
                await ShowColorPulse(Color.Blue, duration);
                await ShowColorPulse(Color.Cyan, duration);
                await ShowColorPulse(Color.Green, duration);
                await ShowColorPulse(Color.GreenYellow, duration);
                await ShowColorPulse(Color.Yellow, duration);
                await ShowColorPulse(Color.Orange, duration);
                await ShowColorPulse(Color.OrangeRed, duration);
                await ShowColorPulse(Color.Red, duration);
                await ShowColorPulse(Color.MediumVioletRed, duration);
                await ShowColorPulse(Color.Purple, duration);
                await ShowColorPulse(Color.Magenta, duration);
                await ShowColorPulse(Color.Pink, duration);
            }
        }

        async Task ShowColorPulse(Color color, TimeSpan duration)
        {
            await onboardLed.StartPulse(color, duration / 2);
            await Task.Delay(duration);
        }

        public void ShowTelemetry()
        {
            Resolver.Log.Info("===== Meadow OS Telemetry =====");

            var memoryInfo = (Device.PlatformOS as F7PlatformOS)?.GetMemoryAllocationInfo();
            var gcAlloc = GC.GetTotalMemory(false);

            if (memoryInfo.HasValue)
            {
                Resolver.Log.Info(" Memory");
                Resolver.Log.Info($"   Total memory: {memoryInfo.Value.Arena:n0}");
                Resolver.Log.Info($"   Total allocated: {memoryInfo.Value.TotalAllocated:n0}");
                Resolver.Log.Info($"   Total free: {memoryInfo.Value.TotalFree:n0}");
                Resolver.Log.Info($"   GC Allocated: {gcAlloc:n0}");
            }

            var load = Device.PlatformOS.GetProcessorUtilization().Average();
            Resolver.Log.Info(" Processor");
            Resolver.Log.Info($"   Usage: {load}%");

            Resolver.Log.Info(" Storage");
            foreach (var drive in Device.PlatformOS.FileSystem.Drives)
            {
                Resolver.Log.Info($"  {drive.Name}");
                Resolver.Log.Info($"    {drive.SpaceAvailable.MegaBytes:0.00}/{drive.Size.MegaBytes:0.0}MB");
            }
            Resolver.Log.Info("====================");
        }
    }
}