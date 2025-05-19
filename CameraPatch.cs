using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravityIndicator
{
    [HarmonyPatch(typeof(PLCameraSystem), "RepositionCameras")]
    internal class CameraUIPatch
    {
        static void Postfix(PLCameraSystem __instance)
        {
            if (!PLLoader.Instance.IsLoaded || PLServer.Instance == null || PLEncounterManager.Instance == null || !UIAddition.Created || !UIAddition.IndicatorEnabled)
            {
                return;
            }
            if (UIAddition.gravityIndicator != null && UIAddition.indicatorMesh != null)
            {
                //UIAddition.gravityIndicator.transform.LookAt(PLCameraSystem.Instance.CurrentSubSystem.MainCameras[0].transform);
                //UIAddition.gravityIndicator.transform.eulerAngles = new Vector3((1f * Mathf.Atan(GUI.Offset.y / GUI.Offset.z) * (float)(180 / Math.PI)), PLCameraSystem.Instance.CurrentSubSystem,, (-1f * Mathf.Atan(GUI.Offset.x / GUI.Offset.z) * (float)(180 / Math.PI)));
                //Vector3 lookDir = UIAddition.gravityIndicator.transform.position - PLCameraSystem.Instance.CurrentSubSystem.MainCameraOffset.position * 2;
                //lookDir.y = 0;
                //UIAddition.gravityIndicator.transform.rotation = Quaternion.LookRotation(lookDir);

                if (GUI.ElementMode.Value == 0)
                {
                    UIAddition.gravityIndicator.transform.position = __instance.CurrentSubSystem.MainCameraOffset.position +
                    __instance.CurrentSubSystem.MainCameras[0].transform.forward * GUI.Offset.z +
                    __instance.CurrentSubSystem.MainCameras[0].transform.right * GUI.Offset.x +
                    __instance.CurrentSubSystem.MainCameras[0].transform.up * GUI.Offset.y;
                }
                else if (PLNetworkManager.Instance != null && PLNetworkManager.Instance.LocalPlayer != null && PLNetworkManager.Instance.LocalPlayer.GetPawn().CurrentShip != null)
                {
                    UIAddition.gravityIndicator.transform.position = PLNetworkManager.Instance.LocalPlayer.GetPawn().CurrentShip.Exterior.transform.position +
                    PLEncounterManager.Instance.PlayerShip.Exterior.transform.forward * GUI.Offset.z +
                    PLEncounterManager.Instance.PlayerShip.Exterior.transform.right * GUI.Offset.x +
                    PLEncounterManager.Instance.PlayerShip.Exterior.transform.up * GUI.Offset.y;
                }

                //Vector3 calcUseVector = __instance.CurrentSubSystem.MainCameraOffset.forward - Vector3.Normalize(__instance.CurrentSubSystem.MainCameras[0].transform.forward * GUI.Offset.z +
                //    __instance.CurrentSubSystem.MainCameras[0].transform.right * GUI.Offset.x +
                //    __instance.CurrentSubSystem.MainCameras[0].transform.up * GUI.Offset.y);
                //Vector3 directionVector = UIAddition.gravityIndicator.transform.position - __instance.CurrentSubSystem.MainCameraOffset.position;
                //Vector3 resultantVector = Vector3.Cross(directionVector, calcUseVector);
                //UIAddition.gravityIndicator.transform.rotation = Quaternion.LookRotation(new Vector3(-resultantVector.x,__instance.CurrentSubSystem.MainCameraOffset.rotation.y, resultantVector.z));
            }
        }
    }
}
