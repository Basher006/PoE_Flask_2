using Drinker.DataGrab;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drinker.BotLogic
{
    internal static class FlasksUse
    {
        public delegate void FlaksUpdate(FlasksData data);

        private static FlaskGroupUseContext group1 = new FlaskGroupUseContext(FlasksGroupsManager.flasksGroup1);
        private static FlaskGroupUseContext group2 = new FlaskGroupUseContext(FlasksGroupsManager.flasksGroup2);
        private static List<FlaksUpdate> flaksUpdates = new List<FlaksUpdate>();


        static FlasksUse()
        {
            SetFlaksGroupes_forUpdate();
        }

        
        public static void UseFlasks(FlasksData res)
        {
            //foreach (var item in flaksUpdates)
            //{
            //    item(res);
            //}

            List<Action> actions = new List<Action>();
            foreach (var item in flaksUpdates)
            {
                actions.Add(() => item(res));
            }
            Parallel.Invoke(actions.ToArray());
        }

        public static void SetFlaksGroupes_forUpdate()
        {
            flaksUpdates.Clear();
            foreach (var flask in FlasksGroupsManager.nonGroupFlasks)
            {
                flaksUpdates.Add(flask.Update);
            }
            flaksUpdates.Add(group1.Update);
            flaksUpdates.Add(group2.Update);
        }

        public static void UpdateFlaskGroupes_forUpdate()
        {
            group1 = new FlaskGroupUseContext(FlasksGroupsManager.flasksGroup1);
            group2 = new FlaskGroupUseContext(FlasksGroupsManager.flasksGroup2);
            SetFlaksGroupes_forUpdate();
        }
    }
}
