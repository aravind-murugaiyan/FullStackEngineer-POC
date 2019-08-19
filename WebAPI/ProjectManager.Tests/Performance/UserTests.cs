using NBench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectManager.Tests.Performance
{
    public class UserTests
    {
        [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Throughput,
        TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 5000)]
        public void PerformanceTests()
        {
            // Set up Prerequisites   
            var controller = new UserController();
            // Act on Test  
            var response = controller.GetUsers();
            // Assert the result  
            Assert.IsTrue(response != null);
        }
    }
}
