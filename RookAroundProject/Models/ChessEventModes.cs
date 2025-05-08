namespace RookAroundProject;

public class ChessMode : IMatchMode {
    public string Title { get; set; }
    public List<Resource> Resources { get; private set; }
    public int RegularPlayers { get; private set; }
    public bool HasGMPlayer { get; private set; }


    public ChessMode() {
        Title = "Chess";
        Resources = new List<Resource> {
            new Resource(ResourceName.Board, 1, false),
            new Resource(ResourceName.Table, 1, false),
            new Resource(ResourceName.Chair, 2, false)
        };
        RegularPlayers = 2;
        HasGMPlayer = false;
    }
}

public class MatchModeDecorator : IMatchMode {
    protected IMatchMode WrappedMatchMode { get; }

    protected MatchModeDecorator() {}

    public MatchModeDecorator(IMatchMode wrappedMatchMode) {
        WrappedMatchMode = wrappedMatchMode;
    }

    public string Title {
        get => WrappedMatchMode.Title;
        set => WrappedMatchMode.Title = value;
    }
    public int RegularPlayers => WrappedMatchMode.RegularPlayers;
    public virtual bool HasGMPlayer => WrappedMatchMode.HasGMPlayer;
    public virtual List<Resource> Resources => WrappedMatchMode.Resources;
}

public class DuckMode : MatchModeDecorator {
    private readonly List<Resource> _resources;

    protected DuckMode() {}
    public DuckMode(IMatchMode wrappedMatchMode) : base(wrappedMatchMode) {
        _resources = new List<Resource>(base.Resources);
        _resources.Add(new Resource(ResourceName.Duck, 1, false));
        Title = "Duck " + Title;
    }

    public override List<Resource> Resources => _resources;
}

public class BlindFoldedMode : MatchModeDecorator {
    private readonly List<Resource> _resources;
    protected BlindFoldedMode() {}

    public BlindFoldedMode(IMatchMode wrappedMatchMode) : base(wrappedMatchMode) {
        _resources = new List<Resource>(base.Resources);
        _resources.Add(new Resource(ResourceName.Blindfold, 2, false));
        Title = "Blindfolded " + Title;
    }

    public override List<Resource> Resources => _resources;
}

public class DrunkMode : MatchModeDecorator {
    private readonly List<Resource> _resources;
    protected DrunkMode() {}

    public DrunkMode(IMatchMode wrappedMatchMode) : base(wrappedMatchMode) {
        _resources = new List<Resource>(base.Resources);
        _resources.Add(new Resource(ResourceName.Drink, 2, true));
        Title = "Drunk " + Title;
    }

    public override List<Resource> Resources => _resources;
}

public class GmVsPlayersMode : MatchModeDecorator {
    protected GmVsPlayersMode() {}
    public GmVsPlayersMode(IMatchMode wrappedMatchMode) : base(wrappedMatchMode) {
        Title = "GM vs Players " + Title;
    }

    public override bool HasGMPlayer => true;
}