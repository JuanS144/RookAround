namespace RookAroundTests;
using RookAroundProject;

[TestClass]
public class UserTests
{

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_EmptyPassword_ShouldThrow()
    {
        var user = new Manager("user1", "", "John", "Doe");
    }

    [TestMethod]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        var user = new Manager("user1", "secure", "Jane", "Smith");

        Assert.IsTrue(user.VerifyPassword("secure"));
    }

    [TestMethod]
    public void VerifyPassword_WrongPassword_ReturnsFalse()
    {
        var user = new Manager("user1", "secure", "Jane", "Smith");

        Assert.IsFalse(user.VerifyPassword("wrong"));
    }
}
