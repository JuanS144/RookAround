using System.Data;

namespace RookAroundProject;

public class Venue{
    public int Id {get; set;}
    public TimeSpan StartTime {get;set;}
    public TimeSpan EndTime {get;set;}

    public int Capacity {get;set;}
    public List<DayOfWeek> Availabilities{get;} = new List<DayOfWeek>();
    
    public List<Tournament> HostingTournaments{get;} = new List<Tournament>();
    protected Venue() { }
    public Venue(int id, TimeSpan starttime, TimeSpan endtime,int capacity,List<DayOfWeek> availabilities){
        if (endtime <= starttime)
            throw new ArgumentException("End time must be after start time.");
        this.Id = id;
        this.StartTime = starttime;
        this.EndTime = endtime;
        this.Capacity = capacity;
        this.Availabilities = availabilities;
    }

    public Venue(Venue other){
        this.Id = other.Id;
        this.StartTime = other.StartTime;
        this.EndTime = other.EndTime;
        this.Capacity = other.Capacity;
        this.Availabilities = new List<DayOfWeek>(other.Availabilities); // Creates a new list
    }

    public void DisplayAvailabilities(){
        foreach (var availability in Availabilities)
        {
            Console.WriteLine($"{availability}:{StartTime}-{EndTime}");
        }
    }

    public void RemoveTournament(int tournamentId){
        foreach(var tournament in HostingTournaments){
            if(tournament.Id == tournamentId){
                HostingTournaments.Remove(tournament);
            }
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj is Venue other)
        {
            return Id == other.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}

