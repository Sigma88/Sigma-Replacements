using System;
using System.Linq;
using UnityEngine;
using KSP.UI;
using KSP.UI.Screens;
using KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
        internal class KSCTriggers : MonoBehaviour
        {
            static ProtoCrewMember newCrew = null;

            void Start()
            {
                // Crew Assignment Dialog
                Resources.FindObjectsOfTypeAll<CrewAssignmentDialog>().FirstOrDefault().gameObject.AddOrGetComponent<AssignmentFix>();

                // Mission Recovery Dialog
                Resources.FindObjectsOfTypeAll<MissionRecoveryDialog>().FirstOrDefault().gameObject.AddOrGetComponent<AssignmentFix>();

                // TEST
                CrewWidget[] widgets = Resources.FindObjectsOfTypeAll<CrewWidget>();
                for (int i = 0; i < widgets?.Length; i++)
                {
                        widgets[i].gameObject.AddOrGetComponent<RecoveryFix>();
                }

                // Astronaut Complex
                Resources.FindObjectsOfTypeAll<AstronautComplex>().FirstOrDefault().gameObject.AddOrGetComponent<AstronautComplexFix>();
                GameEvents.OnCrewmemberHired.Add(HireApplicant);
                GameEvents.OnCrewmemberSacked.Add(FireCrew);
            }

            void HireApplicant(ProtoCrewMember kerbal, int n)
            {
                newCrew = kerbal;
                TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, HireApplicant);
            }
            void HireApplicant()
            {
                Debug.Log("HIRE_UPDATE");
                CustomDescription.Update(newCrew);
                CustomDescription.UpdateAll(HighLogic.CurrentGame.CrewRoster.Applicants.ToArray());
                Debug.Log("HIRE_UPDATE_FINISH");
                TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, HireApplicant);
            }

            void FireCrew(ProtoCrewMember kerbal, int n)
            {
                TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, FireCrew);
            }
            void FireCrew()
            {
                Debug.Log("FIRE_UPDATE");
                ProtoCrewMember[] applicants = HighLogic.CurrentGame.CrewRoster.Applicants.ToArray();
                CustomDescription.UpdateAll(applicants);
                Debug.Log("FIRE_UPDATE_FINISH");
                TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, FireCrew);
            }
        }

        [KSPAddon(KSPAddon.Startup.EditorAny, false)]
        internal class EditorTriggers : MonoBehaviour
        {
            void Start()
            {
                // Crew Assignment Dialog
                Resources.FindObjectsOfTypeAll<CrewAssignmentDialog>().FirstOrDefault().gameObject.AddOrGetComponent<AssignmentFix>();
            }
        }

        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class NyanSettings : MonoBehaviour
        {
            void Start()
            {
                string[] args = Environment.GetCommandLineArgs();

                Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (args.Contains("-nyan-nyan") && !args.Contains("-nyan-not") || args.Contains("-nyan-descr"));
                Nyan.forever = Nyan.nyan && (args.Contains("-nyan-4ever") || args.Contains("-nyan-descr"));
            }
        }
    }
}
