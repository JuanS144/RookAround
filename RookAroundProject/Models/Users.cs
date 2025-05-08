namespace RookAroundProject;

using System.Security.Cryptography;

public abstract class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    private string _pwdHash;
    public string Pwd
    {
        get => _pwdHash;
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && !IsHashed(value))
            {
                _pwdHash = HashPassword(value);
            }
            else
            {
                _pwdHash = value;
            }
        }
    }

    public string Firstname { get; set; }
    public string Lastname { get; set; }

    protected User() { }

    protected User(string username, string pwd, string firstname, string lastname)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.");
        if (string.IsNullOrWhiteSpace(pwd))
            throw new ArgumentException("Pwd cannot be empty.");

        Username = username;
        Pwd = pwd;
        Firstname = firstname;
        Lastname = lastname;
    }

    public bool VerifyPassword(string attemptedPassword)
    {
        var parts = _pwdHash.Split('.');
        if (parts.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        string attemptedHash = HashPassword(attemptedPassword, salt);

        return _pwdHash == attemptedHash;
    }

    private static string HashPassword(string password, byte[]? salt = null)
    {
        salt ??= GenerateSalt();

        const int iterations = 1000;
        using var key = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] hash = key.GetBytes(32);

        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[8];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    private static bool IsHashed(string value)
    {
        return value.Contains('.') && value.Split('.').Length == 2;
    }
}

public class Manager : User
{
    public Manager() : base() { }

    public Manager(string username, string pwd, string firstname, string lastname)
        : base(username, pwd, firstname, lastname) { }
}


public abstract class Player : User
{
    private string _description;
    public string Description
    {
        get => _description;
        set => _description = value ?? string.Empty;
    }

    public int Elo { get; private set; }

    public List<Tournament> ParticipatingTournaments { get; } = new();

    protected Player() : base() { }

    protected Player(
        string username,
        string pwd,
        string firstname,
        string lastname,
        string description,
        int elo
    ) : base(username, pwd, firstname, lastname)
    {
        if (elo < 100)
            throw new ArgumentException("Elo rating must be at least 100.");
        Description = description;
        Elo = elo;
    }

    public void RemoveTournament(int tournamentId)
    {
        ParticipatingTournaments.RemoveAll(t => t.Id == tournamentId);
    }

    public override string ToString()
    {
        return $"{Firstname} {Lastname} ({Elo})";
    }
}

public class BasicPlayer : Player {

    public List<int> TournamentIds {get; set;} = new List<int>();
    protected BasicPlayer() {}
    public BasicPlayer(String username, String pwd, String firstname, String lastname, string description,  int elo) :
        base(username, pwd, firstname, lastname, description, elo){
    }

    public override string ToString(){
        return Username;
    }

    public override bool Equals(object? obj) {
        if (obj == null || obj.GetType() != typeof(BasicPlayer))
            return false;

        BasicPlayer other = (BasicPlayer)obj;
        return this.Firstname.Equals(other.Firstname) && 
               this.Lastname.Equals(other.Lastname);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Firstname, Lastname);
    }

}

public class GMPlayer : Player {
    public int Id {get;set;}
    public TimeSpan StartTime {get;set;}
    public TimeSpan EndTime {get;set;}
    public IReadOnlyList<DayOfWeek> Availabilities => _availabilities;
    private List<DayOfWeek> _availabilities = new();
    protected GMPlayer() {}
    public GMPlayer( string Username, string pwd, string firstname, 
        string lastname, string description, int elo, TimeSpan starttime,
        TimeSpan endtime, List<DayOfWeek> availabilities) : base(Username, pwd, firstname, lastname, description, elo){
        if (endtime <= starttime)
            throw new ArgumentException("End time must be after start time.");
        if (elo < 2500){
            //NOTE might handle this differently in the future
            throw new ArgumentException("Elo must be at least 2500 for GM.");
        }
        this.StartTime = starttime;
        this.EndTime = endtime;
        this._availabilities = new List<DayOfWeek>(availabilities) ?? throw new ArgumentNullException(nameof(availabilities));

    }

    public override string ToString(){
        return base.ToString() + " (GM)";
    }
}
