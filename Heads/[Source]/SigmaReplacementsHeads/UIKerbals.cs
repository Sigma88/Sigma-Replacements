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
                        Load(MenuKerbals[i], index);
                    }
                }
            }

            void Load(ConfigNode node, int index)
            {
                CrewMember kerbal = UIKerbal.menuKerbals[index];
                HeadInfo stats = new HeadInfo(node.GetNode("Stats"), new ConfigNode());

                UIKerbal.menuKerbals[index] = new CrewMember
                (
                    stats.rosterStatus ?? kerbal.type,
                    !string.IsNullOrEmpty(stats.name) ? stats.name : kerbal.name,
                    kerbal.gender = stats.gender ?? kerbal.gender,
                    kerbal.trait = stats.trait?.Length > 0 && !string.IsNullOrEmpty(stats.trait[0]) ? stats.trait[0] : kerbal.trait,
                    kerbal.veteran = stats.veteran ?? kerbal.veteran,
                    kerbal.isBadass = stats.isBadass ?? kerbal.isBadass,
                    kerbal.courage = stats.courage ?? kerbal.courage,
                    kerbal.stupidity = stats.stupidity ?? kerbal.stupidity,
                    kerbal.experienceLevel = stats.experienceLevel ?? kerbal.experienceLevel
                );
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
