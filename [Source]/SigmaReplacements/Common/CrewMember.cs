using Type = ProtoCrewMember.KerbalType;
using Roster = ProtoCrewMember.RosterStatus;


namespace SigmaReplacements
{
    public class CrewMember : ProtoCrewMember
    {
        internal new string name = "";
        internal int activity = 0;

        public CrewMember(Type type, Roster status, string name, Gender gender, string trait, bool veteran, bool isBadass, float courage, float stupidity, int experienceLevel, int activity = 0) : base(type, name)
        {
            Debug.Log("CrewMember", "new CrewMember from stats");

            this.type = type;
            this.rosterStatus = status;
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
    }
}
