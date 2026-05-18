namespace VanguardLeagueSystems.Domain.Permissions;

/// <summary>
/// Defines the complete set of granular capabilities (claims) within Vanguard League Systems.
/// These are used by Tenant Admins to build dynamic roles.
///
/// NOTE: The permission structure is: [Project].[Module].[Action]
/// </summary>
public static class VanguardPermissions
{
    // These permissions are universal across all tenants/sports.
    public static class System
    {
        public const string ViewDashboard = "Vanguard.System.ViewDashboard";
        public const string ManageSubscriptions = "Vanguard.System.ManageSubscriptions";
    }

    public static class Leagues
    {
        public const string Create = "Vanguard.Leagues.Create";
        public const string Update = "Vanguard.Leagues.Update";
        public const string Delete = "Vanguard.Leagues.Delete";
        public const string View = "Vanguard.Leagues.View";
    }

    public static class Seasons
    {
        public const string Create = "Vanguard.Seasons.Create";
        public const string Update = "Vanguard.Seasons.Update";
        public const string View = "Vanguard.Seasons.View";
        // Officiating/Ref assigning will be added here later
    }

    public static class Teams
    {
        public const string Create = "Vanguard.Teams.Create";
        public const string Update = "Vanguard.Teams.Update";
        public const string View = "Vanguard.Teams.View";
        public const string ManageRosters = "Vanguard.Teams.ManageRosters";
    }

    public static class Players
    {
        public const string Register = "Vanguard.Players.Register";
        public const string Update = "Vanguard.Players.Update";
        public const string ViewSensitiveInfo = "Vanguard.Players.ViewSensitiveInfo"; // E.g. DOB/Email
    }

    // Sport-specific permissions (handled conceptually here)
    // E.g., Soccer Scorekeeping vs. Hockey Scorekeeping might have different UI requirements,
    // but both fall under this generalized capability.
    public static class MatchOps
    {
        public const string InputResults = "Vanguard.MatchOps.InputResults";
        public const string EditPastGames = "Vanguard.MatchOps.EditPastGames";
    }
}