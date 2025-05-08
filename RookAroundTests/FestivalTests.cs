using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Moq;
using RookAroundProject;

namespace RookAroundTests;

[TestClass]
public class FestivalTests {
private Festival festival;
private Venue venueRequiredByTournament;
private Mock<IDataManager> mockDataManager;  // Mock the EFDataManager

    [TestInitialize]
        public void Setup()
        {
            // Create a mock instance of EFDataManager
            mockDataManager = new Mock<IDataManager>();

            // Set up mock data for the resources, venues, and players
            mockDataManager.Setup(m => m.LoadVenues()).Returns(CreateVenues());
            mockDataManager.Setup(m => m.LoadResources()).Returns(CreateResources());
            mockDataManager.Setup(m => m.LoadPlayers()).Returns(CreatePlayers());
            mockDataManager.Setup(m => m.LoadTournaments()).Returns(CreateTmts());

            festival = new Festival(mockDataManager.Object);

        
            venueRequiredByTournament = new Venue(1,
                new TimeSpan(9, 0, 0),
                new TimeSpan(17, 0, 0),
                50,
                new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday });
        }

        public List<Resource> CreateResources(){
                Resource r1 = new Resource(ResourceName.Table, 5);
                Resource r2 = new Resource(ResourceName.Board, 5);
                Resource r3 = new Resource(ResourceName.Chair, 10);
            return [r1,r2,r3];
        }

        public List<Tournament> CreateTmts(){
            List<Venue> venues =CreateVenues();
            List<Player> GMs = CreatePlayers();
            Tournament tournament1 = new Tournament(1,
                "Crazy Tournament",
                new DateTime(2026, 4, 17, 9, 0, 0),
                new DateTime(2026, 4, 17, 17, 0, 0),
                venues[0],
                CreateMatch(),
                5);
            tournament1.GM = (GMPlayer)GMs[0];
            Tournament tournament2 = new Tournament(2,
                "Crazy Tournament",
                new DateTime(2026, 4, 17, 9, 0, 0),
                new DateTime(2026, 4, 17, 17, 0, 0),
                venues[1],
                CreateMatch(),
                5);
            tournament2.GM = (GMPlayer)GMs[1];
            Tournament tournament3 = new Tournament(3,
                "Crazy Tournament",
                new DateTime(2026, 4, 17, 8, 0, 0),
                new DateTime(2026, 4, 17, 20, 0, 0),
                venues[2],
                CreateMatch(),
                5);
            tournament3.GM = (GMPlayer)GMs[2];
            return [tournament1,tournament2,tournament3];
        }

        public List<Venue> CreateVenues(){
            Venue venue1 = new Venue(1,
                new TimeSpan(8, 0, 0),
                new TimeSpan(20, 0, 0),
                50,
                new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });

            Venue venue2 = new Venue(2,
                new TimeSpan(9, 0, 0),
                new TimeSpan(17, 0, 0),
                100,
                new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Friday });

            Venue venue3 = new Venue(3,
                new TimeSpan(7, 0, 0),
                new TimeSpan(21, 0, 0),
                100,
                new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday,DayOfWeek.Thursday, DayOfWeek.Friday });
            
            return [venue1, venue2, venue3];
        }
        public List<Player> CreatePlayers(){
            GMPlayer gm1  = new GMPlayer(
                "mgenius",
                "123",
                "magnus",
                "carlsen", 
                "The best chess player",
                2800,
                new TimeSpan(8, 0, 0),
                new TimeSpan(17, 0, 0),
                new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday });

            GMPlayer gm2 = new GMPlayer(
                "mnotgenius",
                "123",
                "notmagnus",
                "carlsen't", 
                "The 2nd best chess player",
                2600,
                new TimeSpan(8, 0, 0),
                new TimeSpan(20, 0, 0),
                new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday });

            GMPlayer gm3 = new GMPlayer(
                "mnotgenius33",
                "123",
                "notmagnusss",
                "carlsen't", 
                "The 2nd best chess player",
                2600,
                new TimeSpan(8, 0, 0),
                new TimeSpan(20, 0, 0),
                new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday });

            return[gm1,gm2,gm3];
        }

        public RookAroundProject.Match CreateMatch(){
            IMatchMode chessMode = new ChessMode();
            IMatchMode gmVsPlayersMode = new GmVsPlayersMode(chessMode);
            return new RookAroundProject.Match(gmVsPlayersMode);
        }

    [TestMethod]
    public void ResourcesAreAvailableTest() {
        // Compares against festival resources
        List<Resource> resourcesToCheck = new List<Resource>(
            [new Resource(ResourceName.Table,5),
            new Resource(ResourceName.Board,5),
            new Resource(ResourceName.Chair,10)]);
        Assert.IsTrue(festival.ResourcesAreAvailable(resourcesToCheck));
    }
    [TestMethod]
    public void ResourcesAreNotAvailableTest(){
        // Compares against festival resources
            List<Resource> resourcesToCheck = new List<Resource>(
            [new Resource(ResourceName.Table,5),
            new Resource(ResourceName.Board,5),
            new Resource(ResourceName.Chair,10),
            new Resource(ResourceName.Duck,1)]);
        Assert.IsFalse(festival.ResourcesAreAvailable(resourcesToCheck));
    }
    [TestMethod]
    public void ResourcesLackQuantityTest(){
        // Compares against festival resources
            List<Resource> resourcesToCheck = new List<Resource>(
            [new Resource(ResourceName.Table,10),
            new Resource(ResourceName.Board,10),
            new Resource(ResourceName.Chair,20)]);
        Assert.IsFalse(festival.ResourcesAreAvailable(resourcesToCheck));
    }
    [TestMethod]
    public void GetVenuesThatFullyEnclose_UnavailableDayTest()
    {
        // Comparing against festival venues
        DateTime saturdayStart = new DateTime(2026, 4, 18, 9, 0, 0); // Saturday no venues
        DateTime saturdayEnd = new DateTime(2026, 4, 18, 17, 0, 0);
        
        List<Venue> result = festival.GetVenuesThatFullyEnclose(saturdayStart, saturdayEnd);
        Assert.AreEqual(0, result.Count);
    }
    [TestMethod]
    public void GetVenuesThatFullyEncloseTest(){

        
        DateTime venueStart = new DateTime(2026, 4, 17, 9, 0, 0); // Friday so venue is valid
        DateTime venueEnd = new DateTime(2026, 4, 17, 17, 0, 0); //all venues enclos the timespan
        
        
        List<Venue> enclosingVenues = festival.GetVenuesThatFullyEnclose(venueStart,venueEnd);


        Assert.AreEqual(3,enclosingVenues.Count);
    }
    [TestMethod]
    public void GetVenuesAvailable_NoAvailableVenuesTest(){
        //All venues being used by tournaments so none available
        List<Venue> avlbl = festival.GetVenuesAvailable(
            new DateTime(2026, 4, 17, 9, 0, 0), 
            new DateTime(2026,04,17,17,00,00),5);
        
        Assert.IsTrue(avlbl.Count == 0);
    }
    [TestMethod]
    public void GetVenuesAvailableTest(){
        //Adding new venues because all others in use
        festival.Venues.Add(new Venue(4,
                new TimeSpan(9, 0, 0),
                new TimeSpan(17, 0, 0),
                100,
                new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Friday }));
        
        List<Venue> avlbl = festival.GetVenuesAvailable(
            new DateTime(2026, 4, 17, 9, 0, 0), 
            new DateTime(2026,04,17,17,00,00),50);
        
        Assert.IsTrue(avlbl.Count == 1);
    }
    [TestMethod]
    public void GetVenuesAvailable_NotEnoughCapacityTest(){
        //Adding new venues because all others in use
        festival.Venues.Add(new Venue(4,
                new TimeSpan(9, 0, 0),
                new TimeSpan(17, 0, 0),
                100,
                new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Friday }));
        
        // Capacity exceeds the new added venue so no venue available
        List<Venue> avlbl = festival.GetVenuesAvailable(
            new DateTime(2026, 4, 17, 9, 0, 0), 
            new DateTime(2026,04,17,17,00,00),110);
        
        Assert.IsTrue(avlbl.Count == 0);
    }
    [TestMethod]
    public void GetCollidingTournaments_MatchingTimeTest()
    {
        
        DateTime start = new DateTime(2026, 4, 17, 9, 0, 0);
        DateTime end = new DateTime(2026,04,17,17,00,00);
        
        List<Tournament> colidingTournaments = festival.GetCollidingTournaments(start,end);
        Assert.IsTrue(colidingTournaments.Count == 3);
    }
    [TestMethod]
    public void GetCollidingTournaments_OverlapingTimeTest()
    {
        
        DateTime start = new DateTime(2026, 4, 17, 9, 0, 0);
        DateTime end = new DateTime(2026,04,17,17,00,00);
        IMatchMode chessMode = new ChessMode();
        RookAroundProject.Match DuckChessMatch = new RookAroundProject.Match(new DuckMode(chessMode));
        RookAroundProject.Match ChessMatch = new RookAroundProject.Match(chessMode);

        Tournament NormalTournament = new Tournament(1,
            "Crazy Tournament", 
            new DateTime(2026, 4, 17, 12, 0, 0),
            new DateTime(2026,04,17,17,00,00),
            venueRequiredByTournament, 
            ChessMatch, 
            5);

        Tournament DuckTournament = new Tournament(2,
            "Duck Tournament", 
            new DateTime(2026, 4, 17, 9, 0, 0),
            new DateTime(2026,04,17,14,00,00),
            venueRequiredByTournament, 
            DuckChessMatch, 
            5);
        festival.Tournaments.Add(NormalTournament);
        festival.Tournaments.Add(DuckTournament);
        
        //Added 2 new tournaments with overlapping times
        List<Tournament> colidingTournaments = festival.GetCollidingTournaments(start,end);
        Assert.IsTrue(colidingTournaments.Count == 5);
    }
    [TestMethod]
    public void GetCollidingTournaments_NoMatchTest()
    {
        // No tournament on this day (different month)
        DateTime start = new DateTime(2026, 3, 17, 9, 0, 0);
        DateTime end = new DateTime(2026,03,17,17,00,00);
        
        List<Tournament> colidingTournaments = festival.GetCollidingTournaments(start,end);
        Assert.IsTrue(colidingTournaments.Count == 0);
    }

    [TestMethod]
    public void GetGmsAvailableTest(){
        // No Tournament is using the Gm so he is available
        festival.GMPlayers.Add(new GMPlayer(
            "abcc",
            "123",
            "magnus",
            "carlsen", 
            "The best chess player",
            2800,
            new TimeSpan(8, 0, 0),
            new TimeSpan(17, 0, 0),
            new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday }));
        
        List<GMPlayer> avlbl = festival.GetGmsAvailable(
            new DateTime(2026, 4, 17, 8, 0, 0), 
            new DateTime(2026,04,17,17,00,00));
        
        Assert.AreEqual(1,avlbl.Count);
    }
    [TestMethod]
    public void GetGmsAvailable_NoneAvailableTest(){
        // No new gm added, and all gms used in tournament 
        // so none available
        List<GMPlayer> avlbl = festival.GetGmsAvailable(
            new DateTime(2026, 4, 17, 9, 0, 0), 
            new DateTime(2026,04,17,17,00,00));
        
        
        Assert.IsTrue(avlbl.Count == 0);
    }
    [TestMethod]
    public void GetEnclosingGM_MatchingScheduleTest(){
        
        DateTime StartTime = new DateTime(2026, 4, 17, 9, 0, 0); // Friday & correct time so 2 gms available
        DateTime EndTime = new DateTime(2026, 4, 17, 17, 0, 0);
        List<GMPlayer> GMsAvailable = festival.GetGmsThatFullyEnclose(StartTime,EndTime);

        Assert.AreEqual(3,GMsAvailable.Count);
    }
    [TestMethod]
    public void GetEnclosingGM_NonMatchingDayTest(){

        DateTime StartTime = new DateTime(2026, 4, 19, 10, 0, 0); // Correct time but wrong day(sunday) so no GMs
        DateTime EndTime = new DateTime(2026, 4, 19, 17, 0, 0);
        List<GMPlayer> GMsAvailable = festival.GetGmsThatFullyEnclose(StartTime,EndTime);

        Assert.IsTrue(GMsAvailable.Count == 0);
    }
    [TestMethod]
    public void GetEnclosingGM_NonMatchingTimeTest(){
        
        DateTime StartTime = new DateTime(2026, 4, 18, 7, 0, 0); // Correct day(friday) but wrong time so no GM
        DateTime EndTime = new DateTime(2026, 4, 18, 17, 0, 0);
        List<GMPlayer> GMsAvailable = festival.GetGmsThatFullyEnclose(StartTime,EndTime);

        Assert.IsTrue(GMsAvailable.Count == 0);
    }

    
    [TestMethod]
    public void GetCollidingResourcesTest(){
        List<Tournament> colidingTournaments = new List<Tournament>();
        
        IMatchMode chessMode = new ChessMode();
        RookAroundProject.Match DuckChessMatch = new RookAroundProject.Match(new DuckMode(chessMode));
        RookAroundProject.Match ChessMatch = new RookAroundProject.Match(chessMode);

        Tournament NormalTournament = new Tournament(1,
            "Crazy Tournament", 
            new DateTime(2026, 4, 17, 9, 0, 0),
            new DateTime(2026,04,17,17,00,00),
            venueRequiredByTournament, 
            ChessMatch, 
            5);

        Tournament DuckTournament = new Tournament(2,
            "Duck Tournament", 
            new DateTime(2026, 4, 17, 9, 0, 0),
            new DateTime(2026,04,17,17,00,00),
            venueRequiredByTournament, 
            DuckChessMatch, 
            5);

            colidingTournaments.Add(NormalTournament);
            colidingTournaments.Add(DuckTournament);
            // 5 RookAroundProject.Matches  so 5 of each resource, ducks are unique to 1 tournament only
            // so they remain as 5, both tournamentes use tables so they addup
            List<Resource> ColidedResources = festival.GetCollidingResources(colidingTournaments);
            Resource colidedtables = ColidedResources.Find(r => r.Name == ResourceName.Table);
            Resource colidedduck = ColidedResources.Find(r => r.Name == ResourceName.Duck);
            Assert.AreEqual(10,colidedtables.Amount);
            Assert.AreEqual(5,colidedduck.Amount);
    }
    

    [TestMethod]
    public void CheckTimeBoundsWhenWithinBoundsTest(){ // Should Return True
        TimeSpan venueStart = new TimeSpan(9, 0, 0); // 9:00 AM
        TimeSpan venueEnd = new TimeSpan(17, 0, 0);  // 5:00 PM

        TimeSpan tournamentStart = new TimeSpan(10, 0, 0); // 10:00 AM
        TimeSpan tournamentEnd = new TimeSpan(16, 0, 0);   // 4:00 PM

        bool result = festival.CheckTimeBounds(tournamentStart, tournamentEnd,venueStart, venueEnd);
        Assert.IsTrue(result);
    }
    [TestMethod]
    public void CheckTimeBoundsWhenTooEarlyTest(){ // Should Return False
        TimeSpan venueStart = new TimeSpan(9, 0, 0); // 9:00 AM
        TimeSpan venueEnd = new TimeSpan(17, 0, 0);  // 5:00 PM

        TimeSpan tournamentStart = new TimeSpan(8, 0, 0); // 8:00 AM
        TimeSpan tournamentEnd = new TimeSpan(17, 0, 0);   // 5:00 PM

        bool result = festival.CheckTimeBounds(tournamentStart, tournamentEnd,venueStart, venueEnd);
        Assert.IsFalse(result);
    }
    [TestMethod]
        public void CheckTimeBoundsWhenTooLateTest(){ // Should Return False
        TimeSpan venueStart = new TimeSpan(9, 0, 0); // 9:00 AM
        TimeSpan venueEnd = new TimeSpan(17, 0, 0);  // 5:00 PM

        TimeSpan tournamentStart = new TimeSpan(9, 0, 0); // 8:00 AM
        TimeSpan tournamentEnd = new TimeSpan(20, 0, 0);   // 8:00 PM

        bool result = festival.CheckTimeBounds( tournamentStart, tournamentEnd,venueStart, venueEnd);
        Assert.IsFalse(result);
    }
    [TestMethod]
        public void CheckTimeBoundsWhenAtBoundsTest(){ // Should Return True
        TimeSpan venueStart = new TimeSpan(9, 0, 0); // 9:00 AM
        TimeSpan venueEnd = new TimeSpan(17, 0, 0);  // 5:00 PM

        TimeSpan tournamentStart = new TimeSpan(9, 0, 0); // 9:00 AM
        TimeSpan tournamentEnd = new TimeSpan(17, 0, 0);   // 5:00 PM

        bool result = festival.CheckTimeBounds(tournamentStart, tournamentEnd,venueStart, venueEnd );
        Assert.IsTrue(result);
    }
    [TestMethod]
    public void TournamentIsValidTest() {
        
        RookAroundProject.Match NormalChessMatch = new RookAroundProject.Match(new ChessMode());
        Tournament validTournament = new Tournament(6,
        "Crazy Tournament",
        new DateTime(2026, 4, 15, 9, 0, 0), //Wednesday and matching time so venue is valid
        new DateTime(2026,04,15,17,00,00),
        venueRequiredByTournament,
        NormalChessMatch,
        5);

       Assert.IsTrue(festival.IsTournamentValid(validTournament));     
    }
    [TestMethod]
    public void TournamentIsNotValid_LackResourceTest(){ // Tournament is invalid, due to lack of resources(duck)
        RookAroundProject.Match duckChessMatch = new RookAroundProject.Match(new DuckMode(new ChessMode()));
        Tournament invalidTournament = new Tournament(1,
        "Crazy Tournament",
        new DateTime(2026, 4, 17, 9, 0, 0), //Thursday so venue is valid
        new DateTime(2026,04,17,17,00,00),
        venueRequiredByTournament,
        duckChessMatch,
        5);
        Assert.IsFalse(festival.IsTournamentValid(invalidTournament));
    }
    [TestMethod]
    public void TournamentIsNotValidLackGmTest(){ // Tournament is invalid, due to lack of GMs
        RookAroundProject.Match GMChessMatch = new RookAroundProject.Match(new GmVsPlayersMode(new ChessMode()));
        Tournament tournamentWithGm = new Tournament(1,
        "Crazy Tournament",
        new DateTime(2026, 4, 17, 7, 0, 0), //Thursday so venue is valid, but no gms at that time
        new DateTime(2026,04,17,21,00,00),
        venueRequiredByTournament,
        GMChessMatch,
        5);
        Assert.IsFalse(festival.IsTournamentValid(tournamentWithGm));
    }
    
    [TestMethod]
    public void TournamentIsNotValid_LackVenueTest(){ // Tournament is invalid, due to lack of Venue

        Venue invalidDaysVenue = new Venue(3,         
            new TimeSpan(9, 0, 0),
            new TimeSpan(17, 0, 0),
            50,
            new List<DayOfWeek> { DayOfWeek.Saturday });
        RookAroundProject.Match NormalChessMatch = new RookAroundProject.Match(new ChessMode());
        Tournament tournament = new Tournament(1,
        "Crazy Tournament",
        new DateTime(2026, 4, 19, 9, 0, 0), // Saturday so venue is invalid
        new DateTime(2026, 4, 19, 17, 0, 0),
        invalidDaysVenue,
        NormalChessMatch,
        5);

       Assert.IsFalse(festival.IsTournamentValid(tournament));  
    }


    [TestMethod]
    public void ViewPlayerScheduleTest() {
        var player = new BasicPlayer (
            username: "chessFan99",
            pwd: "secret123",
            firstname: "Alex",
            lastname: "Knight",
            description: "Aggressive player",
            elo: 1800
        );

        player.ParticipatingTournaments.Add(festival.Tournaments.Where(t => t.Id == 1).First());
        player.ParticipatingTournaments.Add(festival.Tournaments.Where(t => t.Id == 3).First());

        Assert.AreEqual(2,player.ParticipatingTournaments.Count);
    }

    [TestMethod]
    public void RemoveTournament_ShouldCallRemovePlayersAndVenue()
    {
        // Arrange
        var tournament = new Mock<Tournament>(MockBehavior.Strict);
        tournament.Setup(t => t.RemovePlayers());
        tournament.Setup(t => t.RemoveVenue());

        festival.Tournaments = new List<Tournament> { tournament.Object };

        // Act
        festival.RemoveTournament(tournament.Object);

        // Assert
        tournament.Verify(t => t.RemovePlayers(), Times.Once);
        tournament.Verify(t => t.RemoveVenue(), Times.Once);
    }


}

