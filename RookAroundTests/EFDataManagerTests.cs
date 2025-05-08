    
    using RookAroundProject;
    using Moq;
    using Microsoft.EntityFrameworkCore;
    namespace RookAroundTests;


    [TestClass]
    public class EFDataManagerTests
    {
        [TestMethod]
        public void TestAddPlayer(){
            //Arrange

            Player player1 = new BasicPlayer("Player1", "1234", "Leglorious", "James", "The king himself", 1500);
            var mockSet = new Mock<DbSet<Player>>();
            
            var mockContext = new Mock<RookAroundContext>();
            mockContext.Setup(m => m.Players).Returns(mockSet.Object);
            var EFDataManager = new EFDataManager(mockContext.Object);

            
            //Act
            EFDataManager.AddPlayer(player1);

            //Assert
            mockSet.Verify(m => m.Add(It.IsAny<Player>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void LoadVenuesTest(){
            //Arrange
            var listVenues = new List<Venue>();
            listVenues.Add(new Venue(1,
            new TimeSpan(5, 0, 0),
            new TimeSpan(9, 0, 0),
            50,
            new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Friday }));

            listVenues.Add(new Venue(2,
            new TimeSpan(8, 0, 0),
            new TimeSpan(20, 0, 0),
            20,
            new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Friday }));

            listVenues.Add(new Venue(3,
            new TimeSpan(10, 0, 0),
            new TimeSpan(20, 0, 0),
            10,
            new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Friday }));
            
            var data = listVenues.AsQueryable();
            var mockSet = new Mock<DbSet<Venue>>();
            mockSet.As<IQueryable<Venue>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Venue>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Venue>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Venue>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            
            var mockContext = new Mock<RookAroundContext>();
            mockContext.Setup(m => m.Venues).Returns(mockSet.Object);
            var EFDataManager = new EFDataManager(mockContext.Object);

            
            //Act
            var venues = EFDataManager.LoadVenues();

            //Assert
            Assert.AreEqual(3,venues.Count);
            Assert.AreEqual(1,venues[0].Id);
            Assert.AreEqual(2,venues[1].Id);
            Assert.AreEqual(3,venues[2].Id);
        }
        
    }