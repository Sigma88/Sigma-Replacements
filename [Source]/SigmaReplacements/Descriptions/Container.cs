using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using KSP.UI;
using KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;
using TMPro;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        internal class ListItemContainer
        {
            CrewListItem listItem;
            CrewWidget widget;

            internal ProtoCrewMember crew
            {
                get
                {
                    if (listItem != null)
                    {
                        return listItem.GetCrewRef();
                    }
                    else
                    {
                        return widget?.crew;
                    }
                }
            }

            internal string name
            {
                set
                {
                    string newName = value;

                    if (Nyan.forever)
                        newName = newName.Replace("Kerman", "NyanNyan");

                    if (listItem != null)
                    {
                        listItem.kerbalName.text = newName;
                    }
                    else if (widget != null)
                    {
                        FieldInfo header = typeof(CrewWidget).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(k => k.Name == "header");
                        TextMeshProUGUI displayName = (TextMeshProUGUI)header.GetValue(widget);
                        displayName.text = newName;
                    }
                }
            }

            internal Texture sprite
            {
                set
                {
                    Texture newTex = NyanSprite ?? value;

                    Debug.Log("ListItemContainer.sprite", "newTex = " + newTex + ", [" + newTex?.width + "x" + newTex?.height + "]");

                    if (newTex != null)
                    {
                        if (listItem != null)
                        {
                            Image JebTheCreep = listItem.gameObject?.GetChild("kerbal Image")?.GetComponent<Image>();
                            if (JebTheCreep != null)
                                JebTheCreep.enabled = false;

                            listItem.kerbalSprite.texture = newTex;
                        }
                        else if (widget != null)
                        {
                            widget.crewIcon.sprite = Sprite.Create((Texture2D)newTex, widget.crewIcon.sprite.rect, widget.crewIcon.sprite.pivot);
                        }
                    }
                }
            }

            internal ListItemContainer(CrewListItem listItem = null, CrewWidget widget = null)
            {
                this.listItem = listItem;
                this.widget = widget;
            }

            Texture NyanSprite
            {
                get
                {
                    int hash = Math.Abs(crew.trait.GetHashCode() + (HighLogic.CurrentGame?.Seed ?? 0));

                    if (Nyan.nyan)
                    {
                        // Sprites names:
                        ///
                        ///  0 = "bob"       6 = "jack"     12 = "pirate"
                        ///  1 = "bunny"     7 = "jazz"     13 = "stpat"
                        ///  2 = "cat"       8 = "mex"      14 = "tacnayn"
                        ///  3 = "coin"      9 = "mummy"    15 = "vday"
                        ///  4 = "game"     10 = "ninja"    16 = "xmas"
                        ///  5 = "grumpy"   11 = "party"    17 = "zombie"
                        Texture[] sprites = Nyan.nyanSprites;
                        List<Texture> chooseFrom = new List<Texture>();
                        chooseFrom.Add(sprites[2]);

                        if (Nyan.forever)
                        {
                            chooseFrom.AddRange(new[] { sprites[0], sprites[3], sprites[4], sprites[5], sprites[7], sprites[14] });

                            DateTime today = DateTime.Today;

                            // New Year's Day
                            if (today.Month == 1 && today.Day == 1)
                            {
                                chooseFrom.Clear();
                                chooseFrom.Add(sprites[11]);
                            }

                            // St. Valentine's Day
                            if (today.Month == 2 && today.Day == 14)
                            {
                                chooseFrom.Clear();
                                chooseFrom.Add(sprites[15]);
                            }

                            // St. Patrick's Day
                            if (today.Month == 3 && today.Day == 17)
                            {
                                chooseFrom.Clear();
                                chooseFrom.Add(sprites[13]);
                            }

                            // Easter
                            if (easter.Contains(today))
                            {
                                chooseFrom.Clear();
                                chooseFrom.Add(sprites[1]);
                            }

                            // Cinco de Mayo
                            if (today.Month == 5 && today.Day == 5)
                            {
                                chooseFrom.Clear();
                                chooseFrom.Add(sprites[8]);
                            }

                            // Halloween
                            if (today.Month == 10 && today.Day == 31)
                            {
                                chooseFrom.Clear();
                                chooseFrom.AddRange(new[] { sprites[6], sprites[9], sprites[10], sprites[12], sprites[17] });
                            }

                            // Christmas
                            if (today.Month == 12 && today.Day == 25)
                            {
                                chooseFrom.Clear();
                                chooseFrom.Add(sprites[16]);
                            }
                        }

                        return chooseFrom[hash % chooseFrom.Count];
                    }

                    return null;
                }
            }

            static DateTime[] easter = new DateTime[]
            {
                new DateTime(2018,4,1),
                new DateTime(2019,4,21),
                new DateTime(2020,4,12),
                new DateTime(2021,4,4),
                new DateTime(2022,4,17),
                new DateTime(2023,4,9),
                new DateTime(2024,3,31),
                new DateTime(2025,4,20),
                new DateTime(2026,4,5),
                new DateTime(2027,3,28),
                new DateTime(2028,4,16),
                new DateTime(2029,4,1),
                new DateTime(2030,4,21),
                new DateTime(2031,4,13),
                new DateTime(2032,3,28),
                new DateTime(2033,4,17),
                new DateTime(2034,4,9),
                new DateTime(2035,3,25),
                new DateTime(2036,4,13),
                new DateTime(2037,4,5),
                new DateTime(2038,4,25),
                new DateTime(2039,4,10),
                new DateTime(2040,4,1)
            };
        }
    }
}
