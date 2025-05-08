namespace RookAroundTests;
using RookAroundProject;
using System;
using System.Collections.Generic;

[TestClass]
public class GMPlayerTests
{

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GMPlayer_EloBelow2500_ThrowsException()
    {
        var gmPlayer = new GMPlayer(
            "gmplayer2", 
            "password", 
            "Jane", 
            "Smith", 
            "New GM", 
            2400, 
            new TimeSpan(10, 0, 0), 
            new TimeSpan(18, 0, 0), 
            new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday }
        );
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GMPlayer_EndTimeBeforeStartTime_ThrowsException()
    {
        var gmPlayer = new GMPlayer(
            "gmplayer3", 
            "password", 
            "Jack", 
            "Brown", 
            "Senior GM", 
            2800, 
            new TimeSpan(18, 0, 0), 
            new TimeSpan(9, 0, 0), // Invalid end time before start time
            new List<DayOfWeek> { DayOfWeek.Tuesday }
        );
    }

    [TestMethod]
    public void GMPlayer_ToString_ReturnsFormattedString()
    {
        var gmPlayer = new GMPlayer(
            "gmplayer4", 
            "gmPassword123", 
            "Sarah", 
            "Johnson", 
            "Top-tier GM", 
            3000, 
            new TimeSpan(9, 0, 0), 
            new TimeSpan(17, 0, 0), 
            new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday }
        );

        Assert.AreEqual("Sarah Johnson (3000) (GM)", gmPlayer.ToString());
    }
}
