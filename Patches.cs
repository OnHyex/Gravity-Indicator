using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace GravityIndicator
{
    class Patches
    {
        [HarmonyPatch(typeof(PLUIMainMenu), "Start")]
        class CreateIndicatorManager
        {
            static void Postfix()
            {
                if (PLNetworkManager.Instance != null)
                {
                    PLNetworkManager.Instance.gameObject.AddComponent<IndicatorManager>();
                }
            }
        }
    }
}
