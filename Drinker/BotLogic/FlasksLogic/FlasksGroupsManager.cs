using FlaskSetup;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drinker.BotLogic
{
    internal static class FlasksGroupsManager
    {
        public static FlaskUseContext[] flasks = new FlaskUseContext[]
            {
                new FlaskUseContext(data.Flask_1, 1),
                new FlaskUseContext(data.Flask_2, 2),
                new FlaskUseContext(data.Flask_3, 3),
                new FlaskUseContext(data.Flask_4, 4),
                new FlaskUseContext(data.Flask_5, 5)
            };

        public static List<FlaskUseContext> nonGroupFlasks = new List<FlaskUseContext>();
        public static List<FlaskUseContext> flasksGroup1 = new List<FlaskUseContext>();
        public static List<FlaskUseContext> flasksGroup2 = new List<FlaskUseContext>();


        private static FlasksSetupData data;

        static FlasksGroupsManager()
        {
            //data = SaveLoadData.TryLoadData();
            DataUpdate();
            SetGroupSetUp();
        }

        public static void DataUpdate()
        {
            data = SaveLoadData.TryLoadData();
            flasks[0].UpdFlaskSetUp(data.Flask_1);
            flasks[1].UpdFlaskSetUp(data.Flask_2);
            flasks[2].UpdFlaskSetUp(data.Flask_3);
            flasks[3].UpdFlaskSetUp(data.Flask_4);
            flasks[4].UpdFlaskSetUp(data.Flask_5);
        }

        public static void GroupSetUpUpdate()
        {
            List<int> Group_1_FlasksIndexes = (List<int>)FlaskSetup.Program.form1.Invoke(FlaskSetup.Program.form1.GetGroup1List);
            ManageFlasksGroup(Group_1_FlasksIndexes, ref flasksGroup1);

            List<int> Group_2_FlasksIndexes = (List<int>)FlaskSetup.Program.form1.Invoke(FlaskSetup.Program.form1.GetGroup2List);
            ManageFlasksGroup(Group_2_FlasksIndexes, ref flasksGroup2);

            ManageFlasksNonGroup(flasksGroup1, flasksGroup2, ref nonGroupFlasks);
        }

        private static void SetGroupSetUp()
        {

            List<int> group_1_indexes = new List<int>();
            List<int> group_2_indexes = new List<int>();

            var flasksData = data.ToArray();
            for (int i = 0; i < flasksData.Length; i++)
            {
                if (flasksData[i].Group != FlasksMetrics.GroupDropBoxValues[(int)FlasksMetrics.groupDB.Not] &&
                    flasksData[i].ActivationType != FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.Not])
                {
                    if (flasksData[i].Group == FlasksMetrics.GroupDropBoxValues[(int)FlasksMetrics.groupDB.Group1])
                        group_1_indexes.Add(i);
                    else if (flasksData[i].Group == FlasksMetrics.GroupDropBoxValues[(int)FlasksMetrics.groupDB.Group2])
                        group_2_indexes.Add(i);
                }
            }
            ManageFlasksGroup(group_1_indexes, ref flasksGroup1);
            ManageFlasksGroup(group_2_indexes, ref flasksGroup2);
            ManageFlasksNonGroup(flasksGroup1, flasksGroup2, ref nonGroupFlasks);
        }

        private static void ManageFlasksGroup(List<int> Group_FlasksIndexes, ref List<FlaskUseContext> flasksGroup)
        {
            bool groupIsAlreadyContaintThatFlask;
            bool ThatFlaskInGroup;
            for (int i = 0; i < flasks.Length; i++)
            {
                groupIsAlreadyContaintThatFlask = flasksGroup.Contains(flasks[i]);
                ThatFlaskInGroup = Group_FlasksIndexes.Contains(flasks[i].flaskNumber - 1);
                if (ThatFlaskInGroup && groupIsAlreadyContaintThatFlask)
                    continue; // flask in group, and should be in group, all ok continue.
                else if (groupIsAlreadyContaintThatFlask && !ThatFlaskInGroup)
                    flasksGroup.Remove(flasks[i]); // flask in group but should not, remove they.
                else if (!groupIsAlreadyContaintThatFlask && ThatFlaskInGroup)
                    flasksGroup.Add(flasks[i]);  // flask not in group but should, add they.
            }
        }

        private static void ManageFlasksNonGroup(List<FlaskUseContext> flasksGroup1, List<FlaskUseContext> flasksGroup2, ref List<FlaskUseContext> flasksNonGroup)
        {
            bool flaksIsInGroup1;
            bool flaksIsInGroup2;
            bool NonGroupFlasks_isAlredyHaveFlask;
            for (int i = 0; i < flasks.Length; i++)
            {
                flaksIsInGroup1 = flasksGroup1.Contains(flasks[i]);
                flaksIsInGroup2 = flasksGroup2.Contains(flasks[i]);
                NonGroupFlasks_isAlredyHaveFlask = flasksNonGroup.Contains(flasks[i]);

                if ((!flaksIsInGroup1 && !flaksIsInGroup2) && !NonGroupFlasks_isAlredyHaveFlask)
                    flasksNonGroup.Add(flasks[i]);          // if not in any group and not in non group list > add to non group list.
                else if ((!flaksIsInGroup1 && !flaksIsInGroup2) && NonGroupFlasks_isAlredyHaveFlask)
                    continue;                               // if not any group, but already in non group list > all ok, continue.
                else if ((flaksIsInGroup1 || flaksIsInGroup2) && NonGroupFlasks_isAlredyHaveFlask)
                    flasksNonGroup.Remove(flasks[i]);       // if flask in some group, and in non group list > remove they from non group list.
            }
        }
    }
}
