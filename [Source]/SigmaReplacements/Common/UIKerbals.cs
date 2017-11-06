using UnityEngine;


namespace SigmaReplacements
{
    internal class UIKerbals : MonoBehaviour
    {
        // Main Menu
        static CrewMember mun1 = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Bob Kerman", ProtoCrewMember.Gender.Male, "Scientist", true, false, 0.3f, 0.1f, 0);
        static CrewMember orbit1 = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Bill Kerman", ProtoCrewMember.Gender.Male, "Engineer", true, false, 0.5f, 0.8f, 0);
        static CrewMember orbit2 = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Bob Kerman", ProtoCrewMember.Gender.Male, "Scientist", true, false, 0.3f, 0.1f, 0);
        static CrewMember orbit3 = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Jebediah Kerman", ProtoCrewMember.Gender.Male, "Pilot", true, true, 0.5f, 0.5f, 0);
        static CrewMember orbit4 = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Valentina Kerman", ProtoCrewMember.Gender.Female, "Pilot", true, true, 0.55f, 0.4f, 0);
        // Instructors
        static CrewMember gene = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Gene Kerman", ProtoCrewMember.Gender.Male, "Instructor", false, false, 0.6f, 0.45f, 0);
        static CrewMember werner = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Wernher von Kerman", ProtoCrewMember.Gender.Male, "Instructor", false, false, 0.25f, 0.25f, 0);
        // Strategy
        static CrewMember mort = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Mortimer Kerman", ProtoCrewMember.Gender.Male, "StockBroker", false, false, 0.65f, 0.35f, 0);
        static CrewMember linus = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Linus Kerman", ProtoCrewMember.Gender.Male, "Researcher", false, false, 0.35f, 0.3f, 0);
        static CrewMember walt = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Walt Kerman", ProtoCrewMember.Gender.Male, "Marketer", false, false, 0.45f, 0.9f, 0);
        static CrewMember gus = new CrewMember(ProtoCrewMember.KerbalType.Crew, "Gus Kerman", ProtoCrewMember.Gender.Male, "Mechanic", false, false, 0.45f, 0.45f, 0);

        internal static CrewMember[] menuKerbals = new[] { mun1, orbit1, orbit2, orbit3, orbit4 };
        internal static CrewMember[] instructors = new[] { gene, werner };
        internal static CrewMember[] strategy = new[] { mort, linus, walt, gus };
    }

    public class UIKerbalMenu : MonoBehaviour
    {
        public CrewMember crewMember;
    }

    internal class UIKerbalWerner : MonoBehaviour
    {
        internal CrewMember crewMember { get { return UIKerbals.instructors[1]; } }
    }

    internal class UIKerbalGene : MonoBehaviour
    {
        internal CrewMember crewMember { get { return UIKerbals.instructors[0]; } }
    }

    internal class UIKerbalStrategy : MonoBehaviour
    {
        static string[] names = new string[] { "Strategy_Mortimer(Clone)", "Strategy_ScienceGuy(Clone)", "Strategy_PRGuy(Clone)", "Strategy_MechanicGuy(Clone)" };

        internal CrewMember crewMember
        {
            get
            {
                for (int i = 0; i < names.Length; i++)
                {
                    if (gameObject?.name == names[i])
                        return UIKerbals.strategy[i];
                }
                return null;
            }
        }

        internal void Apply()
        {
            CustomObject obj = GetComponent<CustomObject>();
            if (obj != null) obj.Apply();
        }
    }

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    internal class UIKerbalLoader : MonoBehaviour
    {
        void Awake()
        {
            Debug.Log("UIKerbalLoader", "Awake");

            ConfigNode[] MenuKerbals = UserSettings.ConfigNode.GetNodes("MenuKerbal");

            for (int i = 0; i < MenuKerbals?.Length; i++)
            {
                if (int.TryParse(MenuKerbals[i]?.GetValue("index"), out int index) && index < UIKerbals.menuKerbals?.Length)
                {
                    UIKerbals.menuKerbals.Load(MenuKerbals[i], index);
                }
            }


            ConfigNode[] Instructors = UserSettings.ConfigNode.GetNodes("Instructor");

            for (int i = 0; i < Instructors?.Length; i++)
            {
                if (int.TryParse(Instructors[i]?.GetValue("index"), out int index) && index < UIKerbals.instructors?.Length)
                {
                    UIKerbals.instructors.Load(Instructors[i], index);
                }
            }


            ConfigNode[] StrategyKerbals = UserSettings.ConfigNode.GetNodes("StrategyKerbal");

            for (int i = 0; i < StrategyKerbals?.Length; i++)
            {
                if (int.TryParse(StrategyKerbals[i]?.GetValue("index"), out int index) && index < UIKerbals.strategy?.Length)
                {
                    UIKerbals.strategy.Load(StrategyKerbals[i], index);
                }
            }
        }
    }
}
