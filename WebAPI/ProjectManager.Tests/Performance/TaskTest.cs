using NBench;
using ProjectManager.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectManager.Tests.Performance
{
    class TaskTest
    {
        [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Throughput,
        TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 5000)]
        public void PerformanceTests()
        {
            // Set up Prerequisites   
            var controller = new TaskController();
            // Act on Test  
            var response = controller.GetTasks();
            // Assert the result  
            Assert.IsTrue(response != null);
        }
    }
}
