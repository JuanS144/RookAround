using Microsoft.EntityFrameworkCore;

namespace RookAroundProject;
public class EFDataManager : IDataManager {
    private  RookAroundContext _context;

    public EFDataManager(RookAroundContext context) {
        _context = context;
    }

    public void ClearDatabase() {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }
    public void AddPlayer(Player player) {
        _context.Players.Add(player);
        _context.SaveChanges();
    }

    public void AddMatch(Match match) {
        _context.Matches.Add(match);
        _context.SaveChanges();
    }

    public void AddTournament(Tournament tournament) {
        _context.Tournaments.Add(tournament);
        _context.SaveChanges();
    }

    public void AddManager(Manager manager) {
        _context.Managers.Add(manager);
        _context.SaveChanges();
    }

    public void AddManagers(List<Manager> managers) {
        _context.Managers.AddRange(managers);
        _context.SaveChanges();
    }

    public void AddVenue(Venue venue) {
        _context.Venues.Add(venue);
        _context.SaveChanges();
    }

    public void AddVenues(List<Venue> venues) {
        _context.Venues.AddRange(venues);
        _context.SaveChanges();
    }

    public void AddResources(List<Resource> resources) {
        _context.Resources.AddRange(resources);
        _context.SaveChanges();
    }

    public void UpdateAccountInfo(Player player) {
        _context.Players.Update(player);
        _context.SaveChanges();
    }

    public void UpdateManagerInfo(Manager manager) {
        _context.Managers.Update(manager);
        _context.SaveChanges();
    }

    public void RemovePlayer(Player player) {
        _context.Players.Remove(player);
        _context.SaveChanges();
    }

    public void RemoveMatch(Match match) {
        _context.Matches.Remove(match);
        _context.SaveChanges();
    }

    public void RemoveManager(Manager manager) {
        _context.Managers.Remove(manager);
        _context.SaveChanges();
    }

    public void RemoveTournament(Tournament tournament)
    {
    // Ensure we are working with a tracked entity
    var trackedTournament = _context.Tournaments
        .Include(t => t.Players)
        .Include(t => t.Matches)
        .Include(t => t.Venue)
        .FirstOrDefault(t => t.Id == tournament.Id);

    if (trackedTournament == null)
        return;

    // Remove the tournament from each player's list
    foreach (var player in trackedTournament.Players.ToList())
    {
        player.ParticipatingTournaments.Remove(trackedTournament);
        _context.Players.Update(player); // ensure EF knows it changed
    }

    // Clear the players list from tournament
    trackedTournament.Players.Clear();

    // Remove associated matches
    foreach (var match in trackedTournament.Matches.ToList())
    {
        _context.Matches.Remove(match);
    }

    // Remove tournament from the venue
    if (trackedTournament.Venue != null)
    {
        trackedTournament.Venue.HostingTournaments.Remove(trackedTournament);
        _context.Venues.Update(trackedTournament.Venue); // ensure EF knows it changed
    }

    // Remove the tournament
    _context.Tournaments.Remove(trackedTournament);

    // Persist changes
    _context.SaveChanges();
    }




    public bool VerifyUser(string username, string pwd) {
        return _context.Managers.Any(m => m.Username == username && m.Pwd == pwd);
    }

    public List<Manager> LoadManagers() {
        return _context.Managers.ToList();
    }

    public List<Player> LoadPlayers() {
        return _context.Players.ToList();
    }

    public List<Manager> LoadManager() {
        return _context.Managers.ToList();
    }


    public Manager? GetManagerByUsername(string username) {
        return _context.Managers.FirstOrDefault(m => m.Username == username);
    }

    public List<Venue> LoadVenues() {
        return _context.Venues.ToList();
    }

    public List<Match> LoadMatches() {
        return _context.Matches.ToList();
    }

    public List<Resource> LoadResources() {
        return _context.Resources.ToList();
    }

    public List<Tournament> LoadTournaments(){
        return _context.Tournaments.Include(t => t.Venue).Include(t => t.Players).ToList();
    }

    public void SaveChanges(){
        _context.SaveChanges();
    }
}