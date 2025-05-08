namespace RookAroundTests;
using RookAroundProject;

[TestClass]
public class ManagerTests
{

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Manager_EmptyUsername_ThrowsException()
    {
        var manager = new Manager("", "admin123", "Alice", "Walker");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Manager_EmptyPassword_ThrowsException()
    {
        var manager = new Manager("manager1", "", "Alice", "Walker");
    }
}
