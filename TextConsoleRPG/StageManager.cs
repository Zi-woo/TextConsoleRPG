using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConsoleRPG
{
    class StageManager
    {
        public int CurStage { get; private set; } = 1;
        public int TopStage { get; private set; } = 1;

        public void NextStage()
        {
            CurStage++;
            if (CurStage > TopStage)
            {
                TopStage = CurStage;
            }
        }

        public void SetStage(int stage)
        {
            CurStage = stage;
        }
    }

}
