using System.Runtime.Versioning;

namespace RookAroundProject;

public class Festival{

     private readonly IDataManager _dataManager;

        // Constructor to inject EFDataManager
        public Festival(IDataManager dataManager)
        {
            _dataManager = dataManager;

            Tournaments = _dataManager.LoadTournaments();
            Resources = _dataManager.LoadResources();
            Venues = _dataManager.LoadVenues();
            GMPlayers = _dataManager.LoadPlayers().OfType<GMPlayer>().ToList(); 
        }
    // tournaments represents all tournaments within the timespan of the festival
    public List<Tournament> Tournaments { get; set;}

    // Resources, Venues, GMs and Referees available for the festival
    public List<Resource> Resources { get; set;}
    public List<Venue> Venues {get;set;}
    public List<GMPlayer> GMPlayers {get;set;}

    
    //  Check 2 resource names are the same
    public bool CompareResourceName(Resource r1, Resource r2){
        return r1.Name == r2.Name;
    }

    //  Check if one resource(festival resource) has equal or 
    //  more quantity than another resource(tournament resource)
    public bool CheckResourceQuantity(Resource festivalResource, Resource tournamentResource){
        return festivalResource.Amount >= tournamentResource.Amount;
    }
    // Checks if a Festival has enough resources to create a tournament
    public bool ResourcesAreAvailable(List<Resource> tournamentResources){
        if (tournamentResources == null || tournamentResources.Count == 0){
            return false;
        }
        
        foreach(var resource in tournamentResources){
            bool  resourceFound = false;
            foreach (var availableResource in Resources)
            {
                if (CompareResourceName(availableResource, resource))
                {
                    resourceFound = CheckResourceQuantity(availableResource, resource);
                    if (!resourceFound){
                        return false; // Not enough quantity
                    }
                    break; // Resource matched, and quantity is fine
                }
            }
            if (!resourceFound){
                return false; // No matching resource found
            }
        }
        return true;
    }

    public List<Resource> GetCollidingResources(List<Tournament> collidingTournaments){
        List<Resource> TotalCollidingResources = new List<Resource>();
        foreach (var tournament in collidingTournaments){
            AddToResourceQuantity(tournament.TotalResources,TotalCollidingResources);
        }
        return TotalCollidingResources;
    }
    public void AddToResourceQuantity(List<Resource> resourcesToAdd, List<Resource> existingResources){
        foreach(var resource in resourcesToAdd){
            var existingResource = existingResources.FirstOrDefault(r => CompareResourceName(r,resource));
            if(existingResource != null){
                existingResource.Amount += resource.Amount;
            }
            else{
                existingResources.Add(new Resource(
                    resource.Name,
                    resource.Amount,
                    resource.IsPerishable));
            }
        }
    }
    

    public List<Tournament> GetCollidingTournaments(DateTime startDate, DateTime endDate)
    {
        List<Tournament> collidingTournaments = new List<Tournament>();

        foreach (var tournament in Tournaments)
        {
            // Check if the tournaments' times overlap
            if (startDate < tournament.EndDate && endDate > tournament.StartDate)
            {
                collidingTournaments.Add(tournament);
            }
        }
        return collidingTournaments;
    }
    // GM username has to be unique for this method to wwork
    public List<GMPlayer> GetGmsAvailable(DateTime startDate, DateTime endDate){
            
        List<GMPlayer> Avlbl = GetGmsThatFullyEnclose(startDate, endDate);

        List<Tournament> CollidingTmts = GetCollidingTournaments(startDate, endDate);
        
        foreach (var tournament in CollidingTmts)
        {
            Avlbl.RemoveAll(gm => gm.Username == tournament.GM.Username);
        }

        return Avlbl;
    }
    public List<GMPlayer> GetGmsThatFullyEnclose(DateTime startDate, DateTime endDate)
    {
        List<GMPlayer> enclosingGmPlayers = new List<GMPlayer>();

        foreach (var GmPlayer in GMPlayers)
        {
           if(GmPlayer.Availabilities.Contains(startDate.DayOfWeek)){
                if (CheckTimeBounds(startDate.TimeOfDay,endDate.TimeOfDay,GmPlayer.StartTime,GmPlayer.EndTime))
                {
                    enclosingGmPlayers.Add(GmPlayer);
                }
           }
        }
        return enclosingGmPlayers;
    }

    public List<Venue> GetVenuesThatFullyEnclose(DateTime startDate, DateTime endDate)
    {
        List<Venue> enclosingVenues = new List<Venue>();
        
        foreach (var venue in Venues)
        {
            if(venue.Availabilities.Contains(startDate.DayOfWeek)){
                // Check if the venue's availability fully encompasses the given time range
                if (CheckTimeBounds(startDate.TimeOfDay,endDate.TimeOfDay,venue.StartTime,venue.EndTime))
                {
                    enclosingVenues.Add(venue);
                }
            }
        }
        return enclosingVenues;
    }

    public List<Venue> GetVenuesAvailable(DateTime StartDate, DateTime EndDate, int Capacity)
    {
        List<Venue> Avlbl = GetVenuesThatFullyEnclose(StartDate, EndDate)
            .Where(v => v.Capacity >= Capacity)  // Only keep venues that meet capacity requirements
            .ToList();

        List<Tournament> CollidingTmts = GetCollidingTournaments(StartDate, EndDate);
        
        foreach (var tournament in CollidingTmts)
        {
            if (Avlbl.Contains(tournament.Venue))
            {
                Avlbl.Remove(tournament.Venue);
            }
        }

        return Avlbl;
    }


    
    public bool CheckTimeBounds(TimeSpan startDateTimeOfDay, TimeSpan endDateTimeOfDay, TimeSpan venueOrGmStartTime, TimeSpan venueOrGmEndTime){
        
        return startDateTimeOfDay >= venueOrGmStartTime &&  venueOrGmEndTime >= endDateTimeOfDay;
    }


    public void RemoveTournament(Tournament tournament)
    {
        // Perform domain-level cleanup
        tournament.RemovePlayers(); // removes this tournament from players' ParticipatingTournaments
        tournament.RemoveVenue();   // removes this tournament from the venue's HostingTournaments

        // Remove from local list if applicable
        Tournaments.Remove(tournament);

        // Delegate to EF manager to handle database removal
        _dataManager.RemoveTournament(tournament);
    }




    // Display the festival schedule
    public void DisplaySchedule(){
        foreach(var tournament in Tournaments){
            Console.WriteLine(tournament.DisplayTournamentDetails());
        }
    }
    
    // Check if an Tournament is valid before adding it -> are ressources available, does it fit into the festival's schedule, etc.
    public bool IsTournamentValid(Tournament tournament){
        
        List<Resource> colidingResources = GetCollidingResources(
            GetCollidingTournaments(tournament.StartDate,tournament.EndDate));
            
        AddToResourceQuantity(
            tournament.TotalResources,
            colidingResources
            );

        if(ResourcesAreAvailable(colidingResources)){
            List<Venue> availableVenues = GetVenuesAvailable(
                tournament.StartDate,
                tournament.EndDate,
                tournament.TotalResources.FirstOrDefault(
                    resource => resource.Name == ResourceName.Table).Amount);

            if(availableVenues.Count >= 1){
                if(tournament.GetGmPlayerCount() >= 1){
                    List<GMPlayer> availableGms = GetGmsAvailable(
                        tournament.StartDate,
                        tournament.EndDate);
                    if(availableGms.Count < 1){
                        return false;
                    }   
                }
                return true;
            }
        }
        return false;
    } 
    public void AddTournament(Tournament tournament){
        if(IsTournamentValid(tournament)){
            _dataManager.AddTournament(tournament);
            _dataManager.SaveChanges();
            Tournaments.Add(tournament);
        }
    }

    public void RemoveTournament(int tournamentId){
        foreach(var tournament in Tournaments){
            if(tournament.Id == tournamentId){
                tournament.RemovePlayers();
                tournament.RemoveVenue();
                Tournaments.Remove(tournament);
            }
        }
    }
}
