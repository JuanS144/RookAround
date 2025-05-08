    namespace RookAroundProject;
    // Tournament Class
    public class Tournament {
        public int Id { get; set; }
        // public IMatchMode MatchMode { get; set; }

        public string MatchModeName { get; set;}
        public bool MatchModeHasGM { get; set;}


        public string Title { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AvailableSpots { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public GMPlayer? GM { get; set; }
        public List<Resource> TotalResources { get; set; } = new List<Resource>();
        public Venue? Venue {get;set;}


        protected Tournament() {}
        // Constructor
        // TODO : Add validation for start and end dates, and check if the Referee / Venue schedules collides with the tournament dates
        public Tournament(int Id, string title, DateTime startDate, DateTime endDate, Venue venue, Match match, int numberOfMatches) {
            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date.");
            if (startDate < DateTime.Today)
                throw new ArgumentException("Start date must not be in the past.");
            if (title == null) throw new ArgumentNullException(nameof(title));
            if (venue == null) throw new ArgumentNullException(nameof(venue));
            if (match == null) throw new ArgumentNullException(nameof(match));
            if (numberOfMatches <= 0)
                throw new ArgumentException("There must be at least one match.");
            if (venue.HostingTournaments.Any(t =>
                startDate < t.EndDate && endDate > t.StartDate)) {
                throw new InvalidOperationException("Venue is already booked during this time.");
            }
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Tournament title cannot be empty.");

            this.Id = Id;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Matches = new List<Match>();

            MatchModeHasGM = match.MatchMode.HasGMPlayer;
            MatchModeName = match.MatchMode.Title;

            Venue = venue;
            AvailableSpots = numberOfMatches*match.MatchMode.RegularPlayers;

            CreateMatches(match, numberOfMatches);
            CalculateTotalResources();
        }

        public void CreateMatches(Match baseMatch, int numberOfMatches) {
            for (int i = 0; i < numberOfMatches; i++) {
                var matchCopy = new Match(baseMatch.MatchMode);
                Matches.Add(matchCopy);
            }
        }

        public int GetGmPlayerCount() {
            int gmCount = 0;
            foreach (Match match in Matches) {
                if (match.MatchMode.HasGMPlayer) {
                    gmCount++;
                }
            }
            return gmCount;
        }

        // Calculate total resources needed for tournament across all matches
        private void CalculateTotalResources() {
            TotalResources.Clear();
            foreach (Match match in Matches) {
                List<Resource> resources = match.GetResources();
                foreach (Resource resource in resources) {
                    AddResource(resource);
                }
            }
        }

        // Helper method to add or update a resource in TotalResources
        private void AddResource(Resource resource) {
            foreach (Resource totalResource in TotalResources) {
                if (totalResource.Name == resource.Name) {
                    totalResource.Amount += resource.Amount;
                    return;
                }
            }
            TotalResources.Add(new Resource(resource.Name, resource.Amount, resource.IsPerishable));
        }

        // Add players to tournament and check if there is any spot available
        public bool AddPlayers(List<Player> players){
            if (players == null)
                return false;
            

            // Count how many regular players are in the incoming list (excludes GM)
            int incomingRegularCount = players.Count(p => p is not GMPlayer);
            int maxRegularPlayers = (Matches.Count * 2) - GetGmPlayerCount();
            int remainingSpots = maxRegularPlayers - Players.Count;
            if (incomingRegularCount > remainingSpots)
                return false;
            foreach (Player player in players){
                if(player is BasicPlayer){
                    BasicPlayer currentPlayer = (BasicPlayer)player;
                    currentPlayer.TournamentIds.Add(this.Id);
                }

                if (player is GMPlayer gmPlayer){
                    if (GM != null | !MatchModeHasGM)
                    //There already is a gm player
                        return false;
                    GM = gmPlayer;
                }
                else{
                    if (Players.Contains(player))
                        return false;
                    Players.Add(player);
                }
            }

            PairPlayers();
            return true;
        }

        
        // Pairs up players -> Add an actual GM Next milestone
        public bool PairPlayers(){
            ClearAllMatchPlayers();
            if (Players.Count == 0){
                return false;
            }
            if (MatchModeHasGM){
                PairGMMatches();
            }
            else{
                PairRegularMatches();
            }
                return true;
        }

        private void ClearAllMatchPlayers(){
            foreach (Match match in Matches){
                match.ClearPlayers();
            }
        }

        // Pair players for regular matches
        private bool PairRegularMatches(){
            if (Players.Count % 2 != 0){
                return false;
            }

            int playerIndex = 0;
            foreach (Match match in Matches){
                if (playerIndex + 1 >= Players.Count)
                    break;

                match.AddPlayer(Players[playerIndex++]);  
                match.AddPlayer(Players[playerIndex++]);
            }
            return true;
        }

        //NOTE: in our current code GM is not a player, but an employee
        //Match has a player1 and player2
        private bool PairGMMatches(){
            if (GM == null || Players.Count == 0){
                return false;
            }
            int playerIndex = 0;
            foreach (Match match in Matches)
            {
                if (playerIndex >= Players.Count){
                    break;
                }

                match.AddPlayer(GM);
                match.AddPlayer(Players[playerIndex++]);
            }
            return true;
        }


        // Remove players from tournament
        public bool RemovePlayer(Player player){
            if (!Players.Contains(player)) {
                return false;
            }
            if (player is GMPlayer && GM == player) {
                GM = null;
            } else {
                Players.Remove(player);
            }
            return true;
        }

        //Remove all Players
        public void RemovePlayers(){
            foreach(var player in Players){
                player.RemoveTournament(this.Id);
            }
            GM = null;
        }
        //Remove all venues
        public void RemoveVenue(){
            Venue.RemoveTournament(this.Id);
            Venue = null;
        }


        public string GetRequiredResources() {
            if (TotalResources.Count == 0) {
                return "No resources required.";
            }

            string output = "";
            foreach (Resource resource in TotalResources) {
                output += resource.Name+": "+resource.Amount+"\n";
            }

            return output;
        }
        public string DisplayTournamentDetails() {
            string schedule = $"{Title}\n";
            schedule += $"Date: from {StartDate} to {EndDate}\n";
            schedule += $"Match Type: {MatchModeName} \n";
            schedule += $"GM: {(GM != null ? (GM.Firstname + " " + GM.Lastname) : 
            "None - Note: Players will not be paired if the tournament needs a GM and one has not been added yet.\n")}\n";
            schedule += $"Available Spots: {AvailableSpots}\n";
            schedule += $"\nRequired Resources: \n{GetRequiredResources()}\n";
            schedule += "Matches Schedule:\n";

            foreach (Match match in Matches) {
                schedule += $"  {match}\n";
            }

            return schedule;
        }
    }
