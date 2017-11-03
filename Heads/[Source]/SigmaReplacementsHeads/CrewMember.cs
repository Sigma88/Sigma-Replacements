namespace SigmaReplacements
{
    namespace Heads
    {
        internal class CrewMember : ProtoCrewMember
        {
            internal new string name = "";

            internal CrewMember(KerbalType type, string name) : base(type, name)
            {
                this.name = name;
                this.type = type;
            }

            CrewMember(KerbalType type) : base(type) { }
            CrewMember(ProtoCrewMember copyOf) : base(copyOf) { }
            CrewMember(Game.Modes mode, ConfigNode node, KerbalType crewType = KerbalType.Crew) : base(mode, node, crewType) { }
        }
    }
}
