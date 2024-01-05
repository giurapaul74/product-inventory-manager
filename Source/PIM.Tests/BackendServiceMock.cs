using Moq;
using ProductInventoryManager.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIM.Tests
{
    public class BackendServiceMock : Mock<IBackendService>
    {
        public BackendServiceMock() : base(MockBehavior.Strict) 
        {
        }
    }
}
