using UnityEngine;
using UnityEngine.UI;
using KSP.UI;
using KSP.UI.Screens;
using KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        class RecoveryFix : MonoBehaviour
        {
            static bool skip = true;

            void Start()
            {
                TimingManager.LateUpdateAdd(TimingManager.TimingStage.Normal, Late);
                TimingManager.LateUpdateAdd(TimingManager.TimingStage.Late, Later);
            }

            void OnEnable()
            {
                skip = false;
            }

            void Late()
            {
                if (!skip)
                {
                    CrewWidget widget = GetComponent<CrewWidget>();
                    if (widget != null)
                    {
                        CustomDescription.Update(new ListItemContainer(widget), null, widget.crew);
                        return;
                    }
                }
            }

            void Later()
            {
                skip = true;
            }
        }

        class AssignmentFix : MonoBehaviour
        {
            static int available;
            static int assigned;

            void Start()
            {
                available = 0;
                assigned = 0;

                MonoBehaviour dialog = (MonoBehaviour)GetComponent<CrewAssignmentDialog>() ?? GetComponent<MissionRecoveryDialog>();

                Debug.Log("AssigmentFix.Start", "dialog = " + dialog);

                if (dialog == null)
                {
                    Debug.Log("AssigmentFix.Start", "Couldn't find CrewAssignmentDialog.");
                    DestroyImmediate(this);
                }

                GameObject.Find("ButtonPanelCrew")?.GetComponent<Button>()?.onClick?.AddListener(ButtonPanelCrew);
            }

            void ButtonPanelCrew()
            {
                available = 0;
                assigned = 0;
            }

            void Update()
            {
                if
                (
                    CrewAssignmentDialog.Instance?.scrollListAvail?.enabled != true ||
                    CrewAssignmentDialog.Instance?.scrollListCrew?.enabled != true
                )
                {
                    available = 0;
                    assigned = 0;
                }

                else

                if
                (
                    CrewAssignmentDialog.Instance?.scrollListAvail?.Count != available ||
                    CrewAssignmentDialog.Instance?.scrollListCrew?.Count != assigned
                )
                {
                    UpdateAssignments();
                }
            }

            void UpdateAssignments()
            {
                CrewAssignmentDialog dialog = GetComponent<CrewAssignmentDialog>();
                CrewListItem[] items = dialog.GetComponentsInChildren<CrewListItem>(true);

                available = dialog.scrollListAvail.Count;
                assigned = dialog.scrollListCrew.Count;

                for (int i = 0; i < items?.Length; i++)
                {
                    CustomDescription.Update(new ListItemContainer(items[i]), items[i].GetTooltip(), items[i]?.GetCrewRef());
                }
            }
        }

        class AstronautComplexFix : MonoBehaviour
        {
            static int count = 0;

            void Start()
            {
                count = 0;
                Debug.Log("DescriptionsFixer", "Start");
                CustomDescription.UpdateAll(HighLogic.CurrentGame.CrewRoster);
            }

            void Update()
            {
                if (count < 3)
                {
                    count++;

                    if (count == 3)
                    {
                        Debug.Log("DescriptionsFixer", "Update");
                        CustomDescription.UpdateAll(HighLogic.CurrentGame.CrewRoster);
                        DestroyImmediate(this);
                    }
                }
            }
        }
    }
}
