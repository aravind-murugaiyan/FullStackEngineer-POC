using NBench;
using ProjectManager.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectManager.Tests.Performance
{
    class ProjectTests
    {
        [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Throughput,
        TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 5000)]
        public void PerformanceTests()
        {
            // Set up Prerequisites   
            var controller = new ProjectController();
            // Act on Test  
            var response = controller.GetProjects();
            // Assert the result  
            Assert.IsTrue(response != null);
        }
    }
}
