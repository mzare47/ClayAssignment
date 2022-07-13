using Shared.Lib.Helpers;
using Shared.Lib.Models;

namespace UnitTests.Tests
{
    [TestFixture]
    public class SharedHelperTests
    {
        [Test]
        [TestCase(0, ExpectedResult = LockStatus.Lock)]
        [TestCase(1, ExpectedResult = LockStatus.Unlock)]
        [TestCase(-1, ExpectedResult = LockStatus.Undefined)]
        [TestCase(-47, ExpectedResult = LockStatus.Undefined)]
        [TestCase(20, ExpectedResult = LockStatus.Undefined)]
        public LockStatus getLockStatus_WhenCalled_WithLockStatusAsInt_ReturnProperLockStatus(int lockStatus)
        {

            //Act and Assert
            return Helper.getLockStatus(lockStatus);
        }
    }
}
