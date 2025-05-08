namespace RookAroundProject;

public class Match {
    public int Id { get; set; }

    // Navigation property to the tournament
    public int? TournamentId { get; set; }
    public Tournament Tournament { get; set; }

    public string Title { get; set; }
    public bool IsCancelled { get; set; }
    private Player? player1 { get; set; }
    private Player? player2 { get; set; }
    
    // Allow dependency injection to create Match types and define players/resources
    public IMatchMode MatchMode { get; set; }

    protected Match(){}

    // Constructor
    public Match(IMatchMode mode) {
        MatchMode = mode;
        Title = MatchMode.Title ?? "Untitled";
        IsCancelled = false;
    }

    // Adds player to the match
    public bool AddPlayer(Player player) {
        if (player1 == null || player2 == null) {
            if (player1 == null)
                player1 = player;
            else
                player2 = player;
            return true;
        }
        return false;
    }

    // Adds multiple players to the match
    public bool AddPlayers(List<Player> players) {
        if (players == null || players.Count == 0 || players.Count > 2) {
            return false;
        }
        
        int currentPlayerCount = (player1 != null ? 1 : 0) + (player2 != null ? 1 : 0);
        if (currentPlayerCount + players.Count > 2)
            return false;   
        
        foreach (Player player in players) {
            AddPlayer(player);
        }
        return true;
    }

    // Removes players from the match
    public bool RemovePlayer(Player player) {
        if (player1 != null && player1.Equals(player)) {
            player1 = null;
            return true;
        } else if (player2 != null && player2.Equals(player)) {
            player2 = null;
            return true;
        } else {
            return false;
        }
    }

    //Removes all players from the match
    public void ClearPlayers(){
        player1 = null;
        player2 = null;
    }


    // Gets the resources required for the match
    public List<Resource> GetResources() {
        return MatchMode.Resources;
    }

    // Overrides the ToString method to display match details
    public override string ToString() {
        string ply1 = player1 != null ? player1.ToString() : "__";
        string ply2 = player2 != null ? player2.ToString() : "__";

        return ply1 + " vs " + ply2;
    }
}
