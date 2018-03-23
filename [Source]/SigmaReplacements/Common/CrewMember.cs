using Type = ProtoCrewMember.KerbalType;
using Roster = ProtoCrewMember.RosterStatus;


namespace SigmaReplacements
{
    public class CrewMember : ProtoCrewMember
    {
        internal new string name = "";
        internal int activity = 0;

        public CrewMember(Type type, Roster rosterStatus, string name, Gender gender, string trait, bool veteran, bool isBadass, float courage, float stupidity, int experienceLevel, int activity = 0) : base(type, name)
        {
            Debug.Log("CrewMember", "new CrewMember (" + name + ") from stats");

            this.type = type;
            this.rosterStatus = rosterStatus;
            this.name = name;
            this.gender = gender;
            this.trait = trait;
            this.veteran = veteran;
            this.isBadass = isBadass;
            this.courage = courage;
            this.stupidity = stupidity;
            this.experienceLevel = experienceLevel;
            this.activity = activity;
        }

        CrewMember(Type type) : base(type) { }
        CrewMember(ProtoCrewMember copyOf) : base(copyOf) { }
        CrewMember(Type type, string name) : base(type, name) { }
        CrewMember(Game.Modes mode, ConfigNode node, Type crewType = Type.Crew) : base(mode, node, crewType) { }

        public CrewMember Load(ConfigNode node)
        {
            Info stats = new Info(node.GetNode("Stats") ?? new ConfigNode(), new ConfigNode());

            return new CrewMember
            (
                (int?)stats?.status > 3 ? 0 : (Type?)(int?)stats?.status ?? type,
                (int?)stats?.status > 3 ? (Roster)((int)stats.status - 4) : rosterStatus,
                !string.IsNullOrEmpty(stats?.name) ? stats.name : name,
                stats?.gender ?? gender,
                stats?.trait?.Length > 0 && !string.IsNullOrEmpty(stats.trait[0]) ? stats.trait[0] : trait,
                stats?.veteran ?? veteran,
                stats?.isBadass ?? isBadass,
                stats?.courage ?? courage,
                stats?.stupidity ?? stupidity,
                stats?.experienceLevel ?? experienceLevel
            );
        }
    }
}
