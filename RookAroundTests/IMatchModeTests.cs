namespace RookAroundTests;
using RookAroundProject;

[TestClass]
public class IMatchModeTests{
    [TestMethod]
    public void SimpleMatchModeTitleTest(){
        IMatchMode chess = new ChessMode();
        IMatchMode duckMode = new DuckMode(chess);

        Assert.AreEqual(duckMode.Title, "Duck Chess");
    }

    [TestMethod]
    public void ComplexMatchModeTitleTest(){
        IMatchMode chess = new ChessMode();
        IMatchMode duckMode = new DuckMode(chess);
        IMatchMode blindFoldedDuckMode = new BlindFoldedMode(duckMode);
        IMatchMode drunkBlindFoldedDuckMode = new DrunkMode(blindFoldedDuckMode);


        Assert.AreEqual(drunkBlindFoldedDuckMode.Title, "Drunk Blindfolded Duck Chess");
    }

    
    [TestMethod]
    public void MakeResourceIsNotPerishableTest(){
        Resource res = new Resource(ResourceName.Board, 1);
        Assert.IsFalse(res.IsPerishable);
    }

    
}

