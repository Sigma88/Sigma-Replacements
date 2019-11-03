using UnityEngine;
using Type = ProtoCrewMember.KerbalType;
using Roster = ProtoCrewMember.RosterStatus;
using Gender = ProtoCrewMember.Gender;


namespace SigmaReplacements
{
    internal class UIKerbals
    {
        // Instructors
        static CrewMember gene = new CrewMember(Type.Unowned, Roster.Available, "Gene Kerman", Gender.Male, "Instructor", false, false, 0.6f, 0.45f, 0);
        static CrewMember werner = new CrewMember(Type.Unowned, Roster.Available, "Wernher von Kerman", Gender.Male, "Instructor", false, false, 0.25f, 0.25f, 0);

        // Strategy
        static CrewMember mort = new CrewMember(Type.Unowned, Roster.Available, "Mortimer Kerman", Gender.Male, "StrategyKerbal", false, false, 0.65f, 0.35f, 0);
        static CrewMember linus = new CrewMember(Type.Unowned, Roster.Available, "Linus Kerman", Gender.Male, "StrategyKerbal", false, false, 0.35f, 0.3f, 0);
        static CrewMember walt = new CrewMember(Type.Unowned, Roster.Available, "Walt Kerman", Gender.Male, "StrategyKerbal", false, false, 0.45f, 0.9f, 0);
        static CrewMember gus = new CrewMember(Type.Unowned, Roster.Available, "Gus Kerman", Gender.Male, "StrategyKerbal", false, false, 0.45f, 0.45f, 0);

        // Arrays
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

        internal void Apply()
        {
            CustomObject[] objs = GetComponents<CustomObject>();

            for (int i = 0; i < objs?.Length; i++)
            {
                if (objs[i] != null)
                {
                    objs[i].LoadFor(crewMember);
                    objs[i].ApplyTo(crewMember);
                }
            }
        }
    }

    internal class UIKerbalStrategy : MonoBehaviour
    {
        static string[] names = new string[] { "Strategy_Mortimer", "Strategy_ScienceGuy", "Strategy_PRGuy", "Strategy_MechanicGuy" };

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
            CustomObject[] objs = GetComponents<CustomObject>();

            for (int i = 0; i < objs?.Length; i++)
            {
                if (objs[i] != null)
                {
                    objs[i].LoadFor(crewMember);
                    objs[i].ApplyTo(crewMember);
                }
            }
        }
    }

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    internal class UIKerbalLoader : MonoBehaviour
    {
        static bool loaded = false;

        void Awake()
        {
            if (!loaded)
            {
                loaded = true;
                Debug.Log("UIKerbalLoader", "Awake");

                ConfigNode[] Instructors = UserSettings.ConfigNode.GetNodes("Instructor");

                for (int i = 0; i < Instructors?.Length; i++)
                {
                    if (int.TryParse(Instructors[i]?.GetValue("index"), out int index) && index < UIKerbals.instructors?.Length)
                    {
                        UIKerbals.instructors[index] = UIKerbals.instructors[index].Load(Instructors[i]);
                    }
                }


                ConfigNode[] StrategyKerbals = UserSettings.ConfigNode.GetNodes("StrategyKerbal");

                for (int i = 0; i < StrategyKerbals?.Length; i++)
                {
                    if (int.TryParse(StrategyKerbals[i]?.GetValue("index"), out int index) && index < UIKerbals.strategy?.Length)
                    {
                        UIKerbals.strategy[index] = UIKerbals.strategy[index].Load(StrategyKerbals[i]);
                    }
                }
            }
        }
    }
}
