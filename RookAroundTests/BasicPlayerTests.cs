namespace RookAroundTests;
using RookAroundProject;

[TestClass]
public class BasicPlayerTests
{

    [TestMethod]
    public void BasicPlayer_Equals_SameName_ReturnsTrue()
    {
        var player1 = new BasicPlayer("u1", "pw", "Alex", "Knight", "", 1000);
        var player2 = new BasicPlayer("u2", "pw", "Alex", "Knight", "", 1000);

        Assert.IsTrue(player1.Equals(player2));
    }

    [TestMethod]
    public void BasicPlayer_Equals_DifferentName_ReturnsFalse()
    {
        var player1 = new BasicPlayer("u1", "pw", "Alex", "Knight", "", 1000);
        var player2 = new BasicPlayer("u2", "pw", "Sam", "Hill", "", 1000);

        Assert.IsFalse(player1.Equals(player2));
    }
}
