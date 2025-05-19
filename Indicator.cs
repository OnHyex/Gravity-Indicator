using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GravityIndicator
{

    internal class Assembler
    {
        internal static async void AssembleIndicator()
        {
            while (!PLLoader.Instance.IsLoaded || PLServer.GetCurrentSector() == null)
            {
                await Task.Yield();
            }
            //PLShipInfo ship = PLEncounterManager.Instance.PlayerShip;
            //Transform Parent = ship.Exterior.transform;
            GameObject abyssShip = Resources.Load("NetworkPrefabs/AbyssSubmersible") as GameObject;
            await Task.Delay(500);
            GameObject gravityIndicatorOri = abyssShip.transform.Find("InteriorStatic").Find("Ship").Find("AbyssMap").Find("GravityDirection").gameObject;
            //Transform gravityIndicatorOri = PLAbyssShipInfo.Instance.Map.MapCompass as Transform;
            UIAddition.gravityIndicator = global::UnityEngine.Object.Instantiate<GameObject>(gravityIndicatorOri);
            UIAddition.gravityIndicator.layer = LayerMask.NameToLayer("OutsideWorldUI");
            UIAddition.gravityIndicator.tag = "Gravity Indicator";
            global::UnityEngine.Object.DontDestroyOnLoad(UIAddition.gravityIndicator);
            UIAddition.gravityIndicator.transform.localScale = GUI.Scale;
            UIAddition.gravityIndicator.transform.Find("GroundRegresentation").gameObject.GetComponent<MeshRenderer>().enabled = false;
            UIAddition.indicatorMesh = UIAddition.gravityIndicator.GetComponent<MeshRenderer>();
            UIAddition.indicatorMesh.material.shader = Shader.Find("Unlit/Texture");
            //Messaging.Echo(PLNetworkManager.Instance.LocalPlayer, "build done");
        }
    }
}
