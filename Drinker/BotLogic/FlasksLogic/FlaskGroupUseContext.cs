using Drinker.DataGrab;
using FlaskSetup;
using System.Collections.Generic;

namespace Drinker.BotLogic
{
    internal class FlaskGroupUseContext
    {
        public delegate void UpdFlasksGroup(List<FlaskUseContext> flasksGroup);
        public UpdFlasksGroup GroupUpdate;


        private List<FlaskUseContext> flasksGroup;

        private int currentFlaskInSequence;
        private int previusFlaskInSequence;
        private int flasksCount;

        public FlaskGroupUseContext(List<FlaskUseContext> flasksGroup)
        {
            this.flasksGroup = flasksGroup;
            currentFlaskInSequence = 0;
            previusFlaskInSequence = 0;
            flasksCount = flasksGroup.Count;
            GroupUpdate = UpdateFlasksGroup;
        }


        public void Update(FlasksData data)
        {
            if (flasksGroup.Count == 0)
                return;

            //if (currentFlaskInSequence == previusFlaskInSequence)
            //{

            //}

            if (currentFlaskInSequence == flasksCount)
            {
                currentFlaskInSequence = 0;
                previusFlaskInSequence = flasksCount - 1;
            }
                

            if (flasksGroup[currentFlaskInSequence].setUp.ActivationType != FlasksMetrics.ActivatorDropBoxValues[(int)FlasksMetrics.ActivationType.KD])
            {
                if (flasksGroup[currentFlaskInSequence].FlaskIsNeedUsed(data) && flasksGroup[previusFlaskInSequence].FlaskChekTimer())
                {
                    flasksGroup[currentFlaskInSequence].UseFlask();
                    previusFlaskInSequence = currentFlaskInSequence;
                    currentFlaskInSequence++;
                }
            }
            else
            {
                if (flasksGroup[previusFlaskInSequence].FlaskIsNeedUsed(data))
                {
                    flasksGroup[currentFlaskInSequence].UseFlask();
                    previusFlaskInSequence = currentFlaskInSequence;
                    currentFlaskInSequence++;

                }
            }
        }


        private void UpdateFlasksGroup(List<FlaskUseContext> flasksGroup)
        {
            this.flasksGroup = flasksGroup;
        }

    }
}
