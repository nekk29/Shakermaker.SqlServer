using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shakermaker.SqlServer.Test
{
    [TestClass]
    public class ContinuousIntegration
    {
        [TestMethod]
        public void ContinuousIntegrationTest()
        {
            Program.Main(new string[] {
                "--release", "Release-1.0.0.1",
                "--environment", "dev",
                "--connection-string", ""
            });
        }
    }
}
