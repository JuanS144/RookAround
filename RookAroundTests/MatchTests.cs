namespace RookAroundTests;
using RookAroundProject;

[TestClass]
public class MatchTests{

    [TestMethod]
    public void ToStringMatchTest()
    {
        IMatchMode chessMode = new ChessMode();
        IMatchMode gmVsPlayersMode = new GmVsPlayersMode(chessMode);

        Match match = new Match(gmVsPlayersMode);
        string result = match.ToString();

        Assert.AreEqual("__ vs __", result);
    }

    [TestMethod]
    public void AddPlayerToMatchSuccessfulTest(){
        IMatchMode chessMode = new ChessMode();
        Match match = new Match(chessMode);

        Player player1 = new BasicPlayer("Player1", "1234", "Leglorious", "James", "The king himself", 1500);
        Player player2 = new BasicPlayer("Player2", "1234", "the", "rizzler", "The challenger", 1100);

        match.AddPlayer(player1);
        match.AddPlayer(player2);

        Assert.AreEqual(match.ToString(), "Player1 vs Player2");
    }

    [TestMethod]
    public void AddPlayerToFullMatchTest(){
        IMatchMode chessMode = new ChessMode();
        Match match = new Match(chessMode);

        Player player1 = new BasicPlayer("Player1", "1234", "Leglorious", "James", "The king himself", 1500);
        Player player2 = new BasicPlayer("Player2", "1234", "the", "rizzler", "The challenger", 1100);
        Player player3 = new BasicPlayer("Player3", "1234", "Antony", "Goat", "Auraaaaaaaaaaaa", 999);

        match.AddPlayer(player1);
        match.AddPlayer(player2);
        bool worked = match.AddPlayer(player3);

        Assert.IsFalse(worked);
    }


    [TestMethod]
    public void GetMatchResourcesAmountTest(){
        IMatchMode chess = new ChessMode();
        IMatchMode duckMode = new DuckMode(chess);
        IMatchMode blindFoldedDuckMode = new BlindFoldedMode(duckMode);

        Match ComplexChess = new Match(blindFoldedDuckMode);
        List<Resource> resources = ComplexChess.GetResources();

        Assert.AreEqual(1, resources.First(r => r.Name == ResourceName.Board).Amount);
        Assert.AreEqual(2, resources.First(r => r.Name == ResourceName.Chair).Amount);
        Assert.AreEqual(1, resources.First(r => r.Name == ResourceName.Table).Amount);

        Assert.AreEqual(1, resources.First(r => r.Name == ResourceName.Duck).Amount);
        Assert.AreEqual(2, resources.First(r => r.Name == ResourceName.Blindfold).Amount);
    }

    [TestMethod]
    public void RemovePlayerFromMatchTest(){
        IMatchMode chess = new ChessMode();
        Match match = new Match(chess);
        Player player1 = new BasicPlayer("Player1", "1234", "Leglorious", "James", "The king himself", 1500);
        Player player2 = new BasicPlayer("Player2", "1234", "the", "rizzler", "The challenger", 1100);

        match.AddPlayer(player1);
        match.AddPlayer(player2);

        match.RemovePlayer(player1);

        Assert.AreEqual("__ vs Player2", match.ToString());
    }


    [TestMethod]
    public void AddEmptyPlayersListToMatchTest()
    {
        IMatchMode chess = new ChessMode();
        Match match = new Match(chess);
        List<Player> players = new List<Player>();

        match.AddPlayers(players);

        Assert.AreEqual("__ vs __", match.ToString());
    }

}

