namespace RookAroundProject;

public interface IDataManager{
    void ClearDatabase();
    void AddPlayer(Player player);
    void AddTournament(Tournament tournament);
    void AddManager(Manager manager);
    void AddVenue(Venue venue);
    void UpdateAccountInfo(Player player);
    void UpdateManagerInfo(Manager manager);
    void RemovePlayer(Player player);
    void RemoveMatch(Match match);

    void RemoveManager(Manager manager);
    bool VerifyUser(String username, String pwd);
    List<Player> LoadPlayers();

    List<Tournament> LoadTournaments();

    List<Venue> LoadVenues();
    
    void RemoveTournament(Tournament tournament);
    List<Manager> LoadManagers();
    List<Match> LoadMatches();
    Manager? GetManagerByUsername(string username);    
    List<Resource> LoadResources();

    void SaveChanges();
}
