using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
    {
        internal class UIKerbal : MonoBehaviour
        {
            static CrewMember mun1 = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Bob Kerman", ProtoCrewMember.Gender.Male, "Scientist", true, false, 0.3f, 0.1f, 0);
            static CrewMember orbit1 = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Bill Kerman", ProtoCrewMember.Gender.Male, "Engineer", true, false, 0.5f, 0.8f, 0);
            static CrewMember orbit2 = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Bob Kerman", ProtoCrewMember.Gender.Male, "Scientist", true, false, 0.3f, 0.1f, 0);
            static CrewMember orbit3 = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Jebediah Kerman", ProtoCrewMember.Gender.Male, "Pilot", true, true, 0.5f, 0.5f, 0);
            static CrewMember orbit4 = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Valentina Kerman", ProtoCrewMember.Gender.Female, "Pilot", true, true, 0.55f, 0.4f, 0);

            internal static CrewMember[] menuKerbals = new[] { mun1, orbit1, orbit2, orbit3, orbit4 };
            internal static CrewMember[] rndKerbals = new[] { mun1, orbit1, orbit2, orbit3, orbit4 };

            internal CrewMember crewMember;
        }

        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        internal class UIKerbalLoader : MonoBehaviour
        {
            void Awake()
            {
                ConfigNode[] MenuKerbals = UserSettings.ConfigNode.GetNodes("MenuKerbal");

                for (int i = 0; i < MenuKerbals?.Length; i++)
                {
                    if (int.TryParse(MenuKerbals[i]?.GetValue("index"), out int index) && index < UIKerbal.menuKerbals?.Length)
                    {
                        UIKerbal.menuKerbals.Load(MenuKerbals[i], index);
                    }
                }

                ConfigNode[] RnDKerbals = UserSettings.ConfigNode.GetNodes("RnDKerbal");

                for (int i = 0; i < RnDKerbals?.Length; i++)
                {
                    if (int.TryParse(RnDKerbals[i]?.GetValue("index"), out int index) && index < UIKerbal.rndKerbals?.Length)
                    {
                        UIKerbal.rndKerbals.Load(RnDKerbals[i], index);
                    }
                }
            }
        }

        internal class CrewMember : ProtoCrewMember
        {
            internal new string name = "";

            internal CrewMember(KerbalType type, string name, Gender gender, string trait, bool veteran, bool isBadass, float courage, float stupidity, int experienceLevel) : base(type, name)
            {
                this.type = type;
                this.name = name;
                this.gender = gender;
                this.trait = trait;
                this.veteran = veteran;
                this.isBadass = isBadass;
                this.courage = courage;
                this.stupidity = stupidity;
                this.experienceLevel = experienceLevel;
            }

            CrewMember(KerbalType type) : base(type) { }
            CrewMember(ProtoCrewMember copyOf) : base(copyOf) { }
            CrewMember(KerbalType type, string name) : base(type, name) { }
            CrewMember(Game.Modes mode, ConfigNode node, KerbalType crewType = KerbalType.Crew) : base(mode, node, crewType) { }
        }
    }
}
