using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using KSP.UI;
using KSP.UI.Screens;
using KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;
using KSP.UI.TooltipTypes;
using Type = ProtoCrewMember.KerbalType;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        internal class CustomDescription : MonoBehaviour
        {
            // Descriptions
            string displayName = "";
            string tooltipName = "";
            string informations = "";
            Texture sprite = null;
            ProtoCrewMember crew;

            // Update Triggers
            void Awake()
            {
                Events.onAstronautComplexEnter.Add(UpdateItem);
                GameEvents.OnCrewmemberHired.Add(CrewHired);
                GameEvents.OnCrewmemberSacked.Add(CrewFired);
                Button button = GetComponent<CrewListItem>()?.suitVariantBtn;
                if (button != null)
                {
                    button.onClick.AddListener(UpdateItem);
                }
            }

            void Start()
            {
                UpdateItem();
            }

            void CrewHired(ProtoCrewMember kerbal, int n)
            {
                UpdateItem(kerbal, Type.Applicant);
            }

            void CrewFired(ProtoCrewMember kerbal, int n)
            {
                UpdateItem(kerbal, Type.Applicant);
            }

            void UpdateItem()
            {
                UpdateItem(null, null);
            }

            void UpdateItem(ProtoCrewMember kerbal)
            {
                UpdateItem(kerbal, null);
            }

            void UpdateItem(Type? type)
            {
                UpdateItem(null, type);
            }

            void UpdateItem(ProtoCrewMember kerbal, Type? type)
            {
                var container = new ListItemContainer(GetComponent<CrewListItem>(), GetComponent<CrewWidget>());
                crew = container?.crew;

                if (crew == null) return;

                if ((kerbal == null && type == null) || kerbal == crew || type == crew.type)
                {
                    LoadFor(crew);
                    ApplyTo(container);
                }
            }

            // Load informations
            void LoadFor(ProtoCrewMember kerbal)
            {
                Debug.Log("CustomDescription.LoadFor", "kerbal = " + kerbal);

                Info.hash = "";
                string collection = "";
                displayName = "";
                tooltipName = "";
                informations = "";
                sprite = null;
                int index = 0;
                int? indexChance = null;

                for (int i = 0; i < DescriptionInfo.DataBase?.Count; i++)
                {
                    DescriptionInfo info = (DescriptionInfo)DescriptionInfo.DataBase[i].GetFor(kerbal);

                    if (info != null)
                    {
                        Debug.Log("CustomDescription.LoadFor", "Matching description index = " + info.index + " to current index = " + index);

                        if (info.index == null || info.index == index)
                        {
                            Debug.Log("CustomDescription.LoadFor", "Matching description collection = " + info.collection + " to current collection = " + collection);

                            if (string.IsNullOrEmpty(collection) || collection == info.collection)
                            {
                                if (info.useChance != 1 && indexChance == null)
                                    indexChance = kerbal.Hash(info.useGameSeed) % 100;

                                Debug.Log("CustomDescription.LoadFor", "Matching description useChance = " + info.useChance + " to current index chance = " + indexChance + " %");

                                if (info.useChance == 1 || indexChance < info.useChance * 100)
                                {
                                    Debug.Log("CustomDescription.LoadFor", "Matched all requirements.");

                                    // Collection
                                    collection = info.collection;
                                    Debug.Log("CustomDescription.LoadFor", "Current collection = " + collection);


                                    // Unique
                                    if (info.unique)
                                    {
                                        displayName = tooltipName = informations = "";
                                        sprite = null;
                                    }

                                    // displayName
                                    if (string.IsNullOrEmpty(displayName))
                                        displayName = info.displayName;

                                    // tooltipName
                                    if (string.IsNullOrEmpty(tooltipName))
                                        tooltipName = info.tooltipName;

                                    // sprite
                                    sprite = sprite ?? info.sprite;

                                    // informations
                                    if (info.informations?.Length > 0)
                                    {
                                        Debug.Log("CustomDescription.LoadFor", "Adding text to informations field, index = " + index);

                                        if (info.informations.Length == 1)
                                            informations += info.informations[0];
                                        else
                                            informations += info.informations[kerbal.Hash(info.useGameSeed) % info.informations.Length];

                                        Debug.Log("CustomDescription.LoadFor", "Updated informations field = " + informations);
                                    }

                                    if (info.unique || info.last)
                                        break;

                                    index++;
                                    indexChance = null;
                                    Debug.Log("CustomDescription.LoadFor", "Index bumped to = " + index);
                                }
                            }
                        }
                    }
                }
            }

            // Change the List Item
            void ApplyTo(ListItemContainer container)
            {
                if (container == null) return;

                if (!string.IsNullOrEmpty(displayName))
                {
                    container.name = displayName.PrintFor(crew);
                }

                if (sprite != null)
                {
                    container.sprite = sprite;
                }

                update = 0;
                TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, ApplyTooltip);
            }

            // Wait a few frames before changing the tooltip
            static int wait = 2;
            int update = 0;
            void ApplyTooltip()
            {
                if (update++ == wait)
                {
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, ApplyTooltip);
                    ApplyTo(GetComponent<TooltipController_CrewAC>());
                }
            }

            // Change the tooltip
            void ApplyTo(TooltipController_CrewAC tooltip)
            {
                if (tooltip == null) return;

                if (!string.IsNullOrEmpty(tooltipName))
                {
                    tooltip.titleString = tooltipName.PrintFor(crew);
                }

                if (!string.IsNullOrEmpty(informations))
                {
                    tooltip.descriptionString = informations.PrintFor(crew);

                    if (crew.type == ProtoCrewMember.KerbalType.Applicant)
                        tooltip.descriptionString += CheckForErrors();

                    UIMasterController.Instance.DespawnTooltip(tooltip);
                }
            }

            // Check for errors (e.g. cannot hire kerbals)
            static string CheckForErrors()
            {
                AstronautComplex complex = Resources.FindObjectsOfTypeAll<AstronautComplex>()?.FirstOrDefault();
                KerbalRoster roster = HighLogic.CurrentGame?.CrewRoster;
                if (complex == null || roster == null) return "";

                int? active = roster?.GetActiveCrewCount();

                if (active < complex?.crewLimit())
                {
                    if (GameVariables.Instance?.GetRecruitHireCost((int)active) > Funding.Instance?.Funds)
                    {
                        return TooltipErrors.OutOfFunds;
                    }
                }
                else
                {
                    return TooltipErrors.AtCapacity;
                }

                return "";
            }
        }
    }
}
