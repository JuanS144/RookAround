using RookAroundProject;
namespace RookAroundTests;
[TestClass]
public class TournamentTests
{
    [TestMethod]
    public void AddPlayersWithinLimitSuccessTest()
    {
        List<DayOfWeek> availabilities = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday };
        IMatchMode chessMode = new ChessMode();
        Match baseMatch = new Match(chessMode);
        Venue venue = new Venue(20, TimeSpan.Zero, TimeSpan.FromHours(8), 100, availabilities);

        Tournament tournament = new Tournament(1,
            "Test Tournament",
            DateTime.Now,
            DateTime.Now.AddDays(1),
            venue,
            baseMatch,
            2
        );
        List<Player> players = new List<Player> {
            new BasicPlayer("player1", "pwd1", "John", "Doe", "", 1500),
            new BasicPlayer("player2", "pwd2", "Jane", "Smith", "", 1600)
        };
        bool added = tournament.AddPlayers(players);
        Assert.IsTrue(added);
        Assert.AreEqual(2, tournament.Players.Count);
    }

    [TestMethod]
    public void AddPlayersExceedingLimitFailsTest()
    {
        IMatchMode chessMode = new ChessMode();
        IMatchMode gmMode = new GmVsPlayersMode(chessMode);
        Match baseMatch = new Match(gmMode);
        List<DayOfWeek> availabilities = new List<DayOfWeek> { DayOfWeek.Monday };
        Venue venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, availabilities);

        Tournament tournament = new Tournament(1,
            "Test Tournament",
            DateTime.Now,
            DateTime.Now.AddDays(1),
            venue,
            baseMatch,
            1
        );

        List<Player> players = new List<Player>{
            new BasicPlayer("player1", "pwd","John", "Doe", "Player 1", 1500),
            new BasicPlayer("player2", "pwd","Jane", "Smith", "Player", 1600)
        };

        bool added = tournament.AddPlayers(players);
        Assert.IsFalse(added);
    }

    [TestMethod]
    public void GMNotAddedToAGmTournamentTest()
    {
        List<DayOfWeek> availabilities = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday };

        Venue venue = new Venue(20, TimeSpan.Zero, TimeSpan.FromHours(8), 100, availabilities);
        
        IMatchMode chessMode = new ChessMode();
        IMatchMode gmChessMode = new GmVsPlayersMode(chessMode);
        Match gmMatch = new Match(gmChessMode);


        Tournament tournament = new Tournament(1,
            "Test Tournament", 
            DateTime.Now, 
            DateTime.Now.AddDays(1),
            venue,
            gmMatch,
            2);

        List<Player> players = new List<Player> {
            new BasicPlayer("player1", "pwd1", "John", "Doe", "Player 1", 1500),
            new BasicPlayer("player2", "pwd2", "Jane", "Smith", "Player 2", 1600),
        };

        tournament.AddPlayers(players);

        Console.WriteLine(tournament.DisplayTournamentDetails());

        foreach (Match match in tournament.Matches)
        {
            Assert.AreEqual("__ vs __", match.ToString());
        }
    }

    [TestMethod]
    public void PairPlayersTournamentTest()
    {
        List<DayOfWeek> availabilities = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday };
        IMatchMode chessMode = new ChessMode();
        Match baseMatch = new Match(chessMode);
        Venue venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, availabilities);

        Tournament tournament = new Tournament(1,
            "Test Tournament",
            DateTime.Now,
            DateTime.Now.AddDays(1),
            venue,
            baseMatch,
            2
            );

        List<Player> players = new List<Player> {
            new BasicPlayer("player1", "pwd1", "John", "Doe", "Player 1", 1500),
            new BasicPlayer("player2", "pwd2", "Jane", "Smith", "Player 2", 1600),
            new BasicPlayer("player3", "pwd3", "Alice", "Johnson", "Player 3", 1700),
            new BasicPlayer("player4", "pwd4", "Bob", "Brown", "Player 4", 1800)
        };

        bool added = tournament.AddPlayers(players);
        Assert.IsTrue(added);

        // Depending on your logic, the pairings may differ
        // If your Match class assigns players in order, adjust accordingly
        Assert.AreEqual("player1 vs player2", tournament.Matches[0].ToString());
        Assert.AreEqual("player3 vs player4", tournament.Matches[1].ToString());
    }
    [TestMethod]
    public void GetGmPlayerCountTest(){
        List<DayOfWeek> availabilities = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday };

        Venue venue = new Venue(20, TimeSpan.Zero, TimeSpan.FromHours(8), 100, availabilities);
        
        IMatchMode chessMode = new ChessMode();
        IMatchMode gmChessMode = new GmVsPlayersMode(chessMode);
        Match gmMatch = new Match(gmChessMode);


        Tournament tournament = new Tournament(1,
            "Test Tournament", 
            DateTime.Now, 
            DateTime.Now.AddDays(1),
            venue,
            gmMatch,
            2);

            Assert.AreEqual(2,tournament.GetGmPlayerCount());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_ThrowsWhenEndDateBeforeStartDate()
    {
        var mode = new ChessMode();
        var match = new Match(mode);
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });

        new Tournament(1, "Invalid Dates", DateTime.Today.AddDays(2), DateTime.Today.AddDays(1), venue, match, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_ThrowsWhenStartDateInPast()
    {
        var mode = new ChessMode();
        var match = new Match(mode);
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });

        new Tournament(1, "Past Start", DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1), venue, match, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ThrowsWhenTitleIsNull()
    {
        var mode = new ChessMode();
        var match = new Match(mode);
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });

        new Tournament(1, null, DateTime.Today, DateTime.Today.AddDays(1), venue, match, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_ThrowsWhenTitleIsEmpty()
    {
        var mode = new ChessMode();
        var match = new Match(mode);
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });

        new Tournament(1, "  ", DateTime.Today, DateTime.Today.AddDays(1), venue, match, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ThrowsWhenVenueIsNull()
    {
        var mode = new ChessMode();
        var match = new Match(mode);

        new Tournament(1, "No Venue", DateTime.Today, DateTime.Today.AddDays(1), null, match, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ThrowsWhenMatchIsNull()
    {
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });

        new Tournament(1, "No Match", DateTime.Today, DateTime.Today.AddDays(1), venue, null, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Constructor_ThrowsWhenVenueAlreadyBooked()
    {
        var chessMode = new ChessMode();
        var match = new Match(chessMode);
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });

        // Create existing tournament to occupy the date
        var existingTournament = new Tournament(2, "Existing", DateTime.Today, DateTime.Today.AddDays(2), venue, match, 1);
        venue.HostingTournaments.Add(existingTournament);

        new Tournament(3, "Conflict", DateTime.Today.AddDays(1), DateTime.Today.AddDays(3), venue, match, 1);
    }

    [TestMethod]
    public void AddPlayers_NullList_ReturnsFalse()
    {
        var venue = new Venue(10, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });
        var match = new Match(new ChessMode());
        var tournament = new Tournament(1, "Null Players", DateTime.Today, DateTime.Today.AddDays(1), venue, match, 1);

        bool result = tournament.AddPlayers(null);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void AddPlayers_DuplicatePlayers_ReturnsFalse()
    {
        var venue = new Venue(10, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });
        var match = new Match(new ChessMode());
        var tournament = new Tournament(1, "Dup Players", DateTime.Today, DateTime.Today.AddDays(1), venue, match, 1);

        var player = new BasicPlayer("user", "pwd", "Alice", "Smith", "", 1500);
        tournament.AddPlayers(new List<Player> { player });

        // Try to add the same player again
        bool result = tournament.AddPlayers(new List<Player> { player });

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void RemovePlayer_RemovesPlayerFromTournament()
    {
        var player = new BasicPlayer("user", "pwd", "Alice", "Smith", "", 1500);
        var venue = new Venue(10, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });
        var match = new Match(new ChessMode());
        var tournament = new Tournament(1, "Test", DateTime.Today, DateTime.Today.AddDays(1), venue, match, 1);
        tournament.AddPlayers(new List<Player> { player });

        bool removed = tournament.RemovePlayer(player);

        Assert.IsTrue(removed);
        Assert.IsFalse(tournament.Players.Contains(player));
    }

    [TestMethod]
    public void RemoveVenue_SetsVenueToNull()
    {
        var venue = new Venue(10, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });
        var match = new Match(new ChessMode());
        var tournament = new Tournament(1, "Test", DateTime.Today, DateTime.Today.AddDays(1), venue, match, 1);

        tournament.RemoveVenue();

        Assert.IsNull(tournament.Venue);
    }

    [TestMethod]
    public void AddingSecondGMToGmTournamentReturnsFalse()
    {
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });
        var gmMode = new GmVsPlayersMode(new ChessMode());
        var match = new Match(gmMode);
        var tournament = new Tournament(1, "Tournament", DateTime.Now, DateTime.Now.AddDays(1), venue, match, 1);

        var gm1 = new GMPlayer(
            "mgenius",
            "123",
            "magnus",
            "carlsen", 
            "The best chess player",
            2800,
            new TimeSpan(9, 0, 0),
            new TimeSpan(17, 0, 0),
            new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday });

        var gm2 = new GMPlayer(
            "hikaru",
            "123",
            "hikaru",
            "utada", 
            "great singer",
            2800,
            new TimeSpan(9, 0, 0),
            new TimeSpan(17, 0, 0),
            new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday });

        bool firstAdd = tournament.AddPlayers(new List<Player> { gm1 });
        bool secondAdd = tournament.AddPlayers(new List<Player> { gm2 });

        Assert.IsTrue(firstAdd);
        Assert.IsFalse(secondAdd);
        Assert.AreEqual(gm1, tournament.GM);
    }

    [TestMethod]
    public void AddingGMToRegularTournamentDoesNotAddToPlayers()
    {
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });
        var match = new Match(new ChessMode()); // Not GM mode
        var tournament = new Tournament(1, "Regular Tournament", DateTime.Now, DateTime.Now.AddDays(1), venue, match, 1);

        var gm = new GMPlayer(
            "mgenius",
            "123",
            "magnus",
            "carlsen", 
            "The best chess player",
            2800,
            new TimeSpan(9, 0, 0),
            new TimeSpan(17, 0, 0),
            new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday });

        bool added = tournament.AddPlayers(new List<Player> { gm });

        Assert.IsFalse(added);
        Assert.AreEqual(0, tournament.Players.Count);
        Assert.IsNull(tournament.GM);
    }

    [TestMethod]
    public void BasicPlayerTournamentIdsUpdatedOnAdd()
    {
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });
        var match = new Match(new ChessMode());
        var tournament = new Tournament(42, "Tournament", DateTime.Now, DateTime.Now.AddDays(1), venue, match, 1);

        var player = new BasicPlayer("player", "pass", "John", "Doe", "p@example.com", 1200);

        tournament.AddPlayers(new List<Player> { player });

        Assert.IsTrue(player.TournamentIds.Contains(42));
    }

    [TestMethod]
    public void PairPlayers_WithoutGMInGmTournament_ReturnsFalse()
    {
        var gmMode = new GmVsPlayersMode(new ChessMode());
        var match = new Match(gmMode);
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });
        var tournament = new Tournament(1, "GM Needed", DateTime.Now, DateTime.Now.AddDays(1), venue, match, 1);

        tournament.AddPlayers(new List<Player> {
            new BasicPlayer("p1", "pass", "A", "One", "", 1000)
        });

        Assert.IsFalse(tournament.PairPlayers());
    }

    [TestMethod]
    public void PairPlayers_EmptyPlayerList_ReturnsFalse()
    {
        var match = new Match(new ChessMode());
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });
        var tournament = new Tournament(1, "Empty", DateTime.Now, DateTime.Now.AddDays(1), venue, match, 1);

        Assert.IsFalse(tournament.PairPlayers());
    }

    [TestMethod]
    public void CalculateTotalResources_AggregatesResourcesAcrossMatches()
    {
        var mode = new ChessMode(); // Assume each match gives one "Board"
        var match = new Match(mode);
        var venue = new Venue(1, TimeSpan.Zero, TimeSpan.FromHours(8), 100, new List<DayOfWeek> { DayOfWeek.Monday });
        var tournament = new Tournament(1, "Resources", DateTime.Now, DateTime.Now.AddDays(1), venue, match, 3);

        var boardCount = tournament.TotalResources.Find(r => r.Name == ResourceName.Board).Amount;
        Assert.AreEqual(3, boardCount); // One per match
    }

}
