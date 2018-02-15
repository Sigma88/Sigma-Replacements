using System.Linq;
using UnityEngine;
using KSP.UI;
using KSP.UI.Screens;
using KSP.UI.TooltipTypes;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        internal class CustomDescription
        {
            // Descriptions
            static string displayName = "";
            static string tooltipName = "";
            static string informations = "";
            static Texture sprite = null;


            // Update item and tooltip
            internal static void Update(ListItemContainer item, TooltipController_CrewAC tooltip, ProtoCrewMember kerbal)
            {
                // Missing Kerbal Tooltip
                if (kerbal == null)
                {
                    Debug.Log("Description.Update", "Kerbal not found.");
                    return;
                }

                else

                if (tooltip == null && item == null)
                {
                    Debug.Log("Description.Update", "Couldn't find CrewListItem and Tooltip for Kerbal \"" + kerbal.name + "\".");
                    return;
                }

                // Custom Kerbal ListItem and Tooltip
                else
                {
                    LoadFor(kerbal);
                    ApplyTo(item, tooltip, kerbal);
                }
            }


            // Load informations
            static void LoadFor(ProtoCrewMember kerbal)
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


            // Apply to item and tooltip
            static void ApplyTo(ListItemContainer item, TooltipController_CrewAC tooltip, ProtoCrewMember kerbal)
            {
                Debug.Log("CustomDescription.ApplyTo", "item = " + item + ", tooltip = " + tooltip + ", kerbal = " + kerbal);

                if (tooltip != null && !string.IsNullOrEmpty(tooltipName))
                {
                    tooltip.titleString = tooltipName.PrintFor(kerbal);
                }

                if (item != null && !string.IsNullOrEmpty(displayName))
                {
                    item.name = displayName.PrintFor(kerbal);
                }

                if (item != null)
                {
                    item.sprite = sprite;
                }

                if (tooltip != null && !string.IsNullOrEmpty(informations))
                {
                    tooltip.descriptionString = informations.PrintFor(kerbal);

                    if (kerbal.type == ProtoCrewMember.KerbalType.Applicant)
                        tooltip.descriptionString += CheckForErrors();

                    UIMasterController.Instance.DespawnTooltip(tooltip);
                }
            }


            // Update kerbals
            internal static void UpdateAll(KerbalRoster kerbals)
            {
                for (int i = 0; i < kerbals?.Count; i++)
                {
                    Update(kerbals[i]);
                }
            }

            internal static void UpdateAll(ProtoCrewMember[] kerbals)
            {
                for (int i = 0; i < kerbals?.Length; i++)
                {
                    Update(kerbals[i]);
                }
            }

            internal static void Update(ProtoCrewMember kerbal)
            {
                Debug.Log("Description.Update", "kerbal = " + kerbal);
                if (kerbal == null) return;

                CrewListItem item = kerbal.crewListItem();
                Debug.Log("Description.Update", "item = " + item);
                TooltipController_CrewAC tooltip = item.GetTooltip();
                Debug.Log("Description.Update", "tooltip = " + tooltip);
                Update(new ListItemContainer(item), tooltip, kerbal);
            }


            // Check for errors
            private static string CheckForErrors()
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
