using System.Reflection;
using System.Text.Json;
namespace RookAroundProject;
public class UI{   


    public static void populateDB(EFDataManager dataManager){
        // ----- POPULATE DB -----

        // ----- MANAGER -----
        Manager juan = new Manager("Juan", "pwd", "Juan", "Badel");
        Manager radin = new Manager("Radin", "pwd", "Radin", "Democri");
        Manager david = new Manager("David", "pwd", "David", "Cabanas");
        dataManager.AddManagers(new List<Manager> {juan, radin, david});


        // ----- GMs -----
        // List<Player> GMs = dataManager.LoadPlayers().Where(p => p is GMPlayer).ToList();
        GMPlayer magnus = new GMPlayer(
            "GM1", "123",
            "Magnus", "Carlsen", 
            "The best chess player",
            2800,
            new TimeSpan(9, 0, 0),
            new TimeSpan(17, 0, 0),
            new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday });
        GMPlayer hikaru = new GMPlayer(
            "GM2", "1234",
            "Hikaru", "Nakamura", 
            "The second best chess player",
            2700,
            new TimeSpan(9, 0, 0),
            new TimeSpan(17, 0, 0),
            new List<DayOfWeek> { DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });
        dataManager.AddPlayer(magnus);
        dataManager.AddPlayer(hikaru);
        

        // ----- VENUES -----
        Venue venue = new Venue(
            1,
            new TimeSpan(5,0,0), 
            new TimeSpan(9, 0, 0),
            50,
            new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday });
        Venue venue2 = new Venue(
            2,
            new TimeSpan(5,0,0), 
            new TimeSpan(9, 0, 0),
            100,
            new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Thursday, DayOfWeek.Friday });
        dataManager.AddVenues(new List<Venue> { venue, venue2 });


        // ----- RESOURCES -----
        Resource tables =  new Resource(ResourceName.Table, 20);
        Resource chairs = new Resource(ResourceName.Chair, 50);
        Resource boards = new Resource(ResourceName.Board, 20);
        Resource clocks = new Resource(ResourceName.Clock, 20);
        Resource ducks = new Resource(ResourceName.Duck, 10);
        Resource blindfolds = new Resource(ResourceName.Blindfold, 10);

        dataManager.AddResources(new List<Resource> { tables, chairs, boards, clocks, ducks, blindfolds });


        // ----- TOURNAMENT -----
        Tournament t1 = new Tournament(
            1, "title", 
            DateTime.SpecifyKind(new DateTime(2025, 4, 25, 9, 0, 0), DateTimeKind.Utc),
            DateTime.SpecifyKind(new DateTime(2025, 4, 25, 17, 0, 0), DateTimeKind.Utc),
            venue, new Match(new ChessMode()), 
            3);
        dataManager.AddTournament(t1);

    }

    public static void Main(string[] args){

        // ----- DB SETUP -----
        RookAroundContext context = new RookAroundContext();
        EFDataManager dataManager = new EFDataManager(context);
        //Must remove this line if we want database to persist
        bool ask = true;
        while (ask){
            Console.WriteLine("Do you want to restart the database? (yes/no)");
            string? input = Console.ReadLine()?.Trim().ToLower();
            
            if (input == "yes"){
                Console.WriteLine("Database cleared. Populating with test data...");
                ask = false;
                dataManager.ClearDatabase();

                populateDB(dataManager);
            }
            else if (input == "no"){
                Console.WriteLine("Database not cleared. Keeping existing data...");
                ask = false;
            }
            else{
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
            }
        }

        // ----- FESTIVAL -----
        Festival chessFestival = new Festival(dataManager);

        // ----- USER INTERACTION -----
        Manager? currentuser;
        bool bowl = true;
        while(bowl){
            Console.Clear();
            Console.WriteLine("Press 1 to go to manager login, 2 for player login, 3 for signup and 4 for shutting off");
            Console.WriteLine("Only manager works for now...");
            string input = GetValidInput(["1","4"]);
            if (input == null){
                continue;
            } else if(input=="4"){
                bowl=false;
                continue;
            }
            // here would be a switchcase depending on the input but it's only manager for now
            Console.Clear();
            
            currentuser = ManagerLogin(dataManager);
            if (currentuser == null){
                continue;
            }

            do
            {
                Console.Clear();
                Console.WriteLine("Press 1 to view players, 2 for festivals, 3 to add manager, 4 to change passwords, and 5 to disconnect");

                input = GetValidInput(["1", "2", "3", "4", "5", "6"]);

                if (input == null)
                    continue;

                switch (input)
                {
                        case "1":
                            Console.WriteLine("Imagine this is a list of players");
                            Console.WriteLine("Press 1 to add a player, 2 to remove a player, and 3 to go back");
                            break;
                        case "2":
                            Console.WriteLine("Press 1 to add a festival, 2 to remove a festival, 3 to view a festival's tournaments, and 4 to go back");
                            Console.WriteLine("1. Festival A");
                            input = GetValidInput(["3"]);
                            Console.WriteLine("Choose a Festival");
                            input = GetValidInput(["1"]);
                            Console.Clear();
                            Console.WriteLine("Press 1 to add a tournament, 2 to remove a tournament, 4 to view a tournament's players and 4 to go back");
                            chessFestival.DisplaySchedule();
                            GetValidInput(["1"]);
                            Console.Clear();
                            Console.WriteLine("Tell me the title of your tournament");
                            string title = Console.ReadLine()?.Trim();
                            IMatchMode chessMode = new ChessMode();
                            Console.WriteLine("This is your tournament's match type: " + chessMode.Title);
                            Console.WriteLine("What type of match do you want to add?");
                            Console.WriteLine("0. To finish 1. Gm 2. Duck");
                            input = GetValidInput(["1"]);
                            chessMode = new GmVsPlayersMode(chessMode);
                            Console.WriteLine("This is your tournament's match type: " + chessMode.Title);
                            Console.WriteLine("What type of match do you want to add?");
                            Console.WriteLine("0. To finish 1. Gm 2. Duck");
                            input = GetValidInput(["2"]);
                            chessMode = new DuckMode(chessMode);
                            Console.WriteLine("This is your tournament's match type: " + chessMode.Title);
                            Console.WriteLine("What type of match do you want to add?");
                            Console.WriteLine("0. To finish 1. Gm 2. Duck");
                            input = GetValidInput(["0"]);
                            Console.WriteLine("How many of this match mode do you want to add?");
                            input = GetValidInput(["3"]);
                            Console.WriteLine("Do you want to add other matches? yes or no");    
                            input = GetValidInput(["no"]);
                            Console.Clear();
                            Console.Write("Enter the date the tournament is starting and ending (comma separated in YYYY-MM-DD(HH-MM-SS) format): ");
                            GetValidInput(["2025-4-2(06-00-00)"]);
                            GetValidInput(["2025-4-2(08-30-00)"]);
                            DateTime StartDate = new DateTime(2025, 4, 3, 6, 0, 0);
                            DateTime EndDate = new DateTime(2025, 4, 3, 8, 0, 0);
                            List<Venue> venues2 = chessFestival.GetVenuesAvailable(StartDate, EndDate, 3);
                            Console.WriteLine("Choose a Venue by id");
                            PrintVenues(venues2);
                            input = GetValidInput(["1"]);
                            chessFestival.AddTournament(new Tournament(2, title, StartDate, EndDate, venues2[0], new Match(chessMode), 3));
                            Console.WriteLine("Tournament Completed Successfully!");
                            Console.ReadLine();
                            break;
                        case "3":
                            CreateManager(dataManager);
                            break;
                        case "4":
                            Console.WriteLine("Confirm current password");
                            if(GetValidInput([currentuser.Pwd]) != null){
                                Console.WriteLine("Write a new password");
                                currentuser.Pwd = Console.ReadLine()?.Trim();
                                dataManager.UpdateManagerInfo(currentuser);
                            }
                            break;
                        case "5":
                            Console.WriteLine("Disconnecting...");
                            currentuser = null;
                            break;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                }
            } while (input != "5" && currentuser != null);
            
            if (currentuser == null){
                Console.WriteLine("You have been disconnected.");
            }

        }
    }

    public  static void PrintVenues(List<Venue> venues)
    {
        if (venues == null || venues.Count == 0)
        {
            Console.WriteLine("No venues available.");
            return;
        }

        foreach (var venue in venues)
        {
            Console.WriteLine($"Venue ID: {venue.Id}");
            Console.WriteLine($"Capacity: {venue.Capacity}");
            Console.WriteLine($"Available from {venue.StartTime} to {venue.EndTime}");
            Console.Write("Available Days: ");
            
            if (venue.Availabilities.Count == 0)
            {
                Console.WriteLine("No specific days.");
            }
            else
            {
                Console.WriteLine(string.Join(", ", venue.Availabilities));
            }

            Console.WriteLine(new string('-', 30)); // Separator
        }
    }


    public static void CreateManager(IDataManager dataManager){
        Console.WriteLine("Enter the manager details:");

        // Get username
        Console.Write("Username: ");
        string username = Console.ReadLine()?.Trim();

        // Get password
        Console.Write("Password: ");
        string password = Console.ReadLine()?.Trim();

        // Get first name
        Console.Write("First Name: ");
        string firstName = Console.ReadLine()?.Trim();

        // Get last name
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine()?.Trim();

        dataManager.AddManager(new Manager(username, password, firstName, lastName));
    }

    
    static Manager? ManagerLogin(IDataManager dataManager)
    {
        List<Manager> managerList = dataManager.LoadManagers();

        var usernames = managerList.Select(manager => manager.Username).ToList();
        var passwords = managerList.Select(manager => manager.Pwd).ToList();

        Console.WriteLine("Enter Username");
        string? username = GetValidInput(usernames);
        if (username == null)
            return null;

        Console.WriteLine("Enter Password");
        if (GetValidInput(passwords) == null)
            return null;

        // Retrieve the manager member by username
        return dataManager.GetManagerByUsername(username);
    }

    static List<DateTime> GenerateDateList(DateTime startDate, int count, TimeSpan interval)
    {
        List<DateTime> dates = new List<DateTime>();

        for (int i = 0; i < count; i++)
        {
            dates.Add(startDate.Add(interval * i));
        }

        return dates;
    }
    static string? GetValidInput(List<string> validOptions, int maxAttempts=3)
    {
        string? input;
        int attempts = 0;

        do
        {
            Console.Write($"Enter a valid option ({string.Join(", ", validOptions)}): ");
            input = Console.ReadLine()?.Trim();
            attempts++;

            if (validOptions.Contains(input))
                return input;

            int attemptsLeft = maxAttempts - attempts;
            if (attemptsLeft > 0)
                Console.WriteLine($"Invalid input. Attempts used: {attempts}/{maxAttempts}. {attemptsLeft} attempts left.");
        }
        while (attempts < maxAttempts);

        return null;
    }

    
}