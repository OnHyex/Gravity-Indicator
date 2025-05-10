using System;
using System.Collections.Generic;
using PulsarModLoader.CustomGUI;
using PulsarModLoader;
using UnityEngine;
using JetBrains.Annotations;

namespace GravityIndicator
{
    internal class GUI : ModSettingsMenu
    {
        static SaveValue<float> GravityIndicatorOffsetX = new SaveValue<float>("GravityIndicatorOffsetX", 0f);
        static SaveValue<float> GravityIndicatorOffsetY = new SaveValue<float>("GravityIndicatorOffsetX", 0f);
        static SaveValue<float> GravityIndicatorOffsetZ = new SaveValue<float>("GravityIndicatorOffsetX", 100f);
        static SaveValue<float> GravityIndicatorScale = new SaveValue<float>("GravityIndicatorScale", 100f);
        public static SaveValue<int> ElementMode = new SaveValue<int>("ElementMode", 0);
        internal static Vector3 Offset = new Vector3(GravityIndicatorOffsetX, GravityIndicatorOffsetY, GravityIndicatorOffsetZ);
        internal static Vector3 Scale = new Vector3(GravityIndicatorScale.Value, GravityIndicatorScale.Value, GravityIndicatorScale.Value);
        private int AdjustmentPosition = 2;
        private string[] Modes = new string[] { "UI Element", "Ship Follow" };
        public override string Name()
        {
            return "Gravity Indicator";
        }
        public override void OnClose()
        {
            base.OnClose();
            GravityIndicatorOffsetX.Value = Offset.x;
            GravityIndicatorOffsetY.Value = Offset.y;
            GravityIndicatorOffsetZ.Value = Offset.z;
            GravityIndicatorScale.Value = Scale.x;
        }
        public override void Draw()
        {
            UIAddition.IndicatorEnabled = GUILayout.Toggle(UIAddition.IndicatorEnabled, "Toggle", Array.Empty<GUILayoutOption>());

            float[] AdjustmentList = { 0.01f, 0.1f, 1f, 10f, 100f };
            GUILayout.BeginArea(new Rect(10, 50, 400, 150));

            GUILayout.BeginHorizontal();

            GUILayout.Box($"Follow Mode: {Modes[ElementMode.Value]}");

            if(GUILayout.Button("Switch Mode"))
            {
                if(ElementMode.Value >= Modes.Length - 1)
                {
                    ElementMode.Value = 0;
                    Offset.z = 100f;
                } 
                else
                {
                    ElementMode.Value++;
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(10, 100, 400, 50));

            GUILayout.BeginHorizontal();
            if(GUILayout.RepeatButton("Up"))
            {
                Offset.y += AdjustmentList[AdjustmentPosition];
            }
            if (GUILayout.RepeatButton("Down"))
            {
                Offset.y -= AdjustmentList[AdjustmentPosition];
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(10, 125, 400, 50));

            GUILayout.BeginHorizontal();

            if (GUILayout.RepeatButton("Left"))
            {
                Offset.x -= AdjustmentList[AdjustmentPosition];
            }
            if (GUILayout.RepeatButton("Right"))
            {
                Offset.x += AdjustmentList[AdjustmentPosition];
            }
            
            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            if(ElementMode == 1)
            {
                GUILayout.BeginArea(new Rect(10, 150, 400, 200));

                GUILayout.BeginHorizontal();

                if (GUILayout.RepeatButton("Forward"))
                {
                    Offset.z += AdjustmentList[AdjustmentPosition];
                }
                if (GUILayout.RepeatButton("Back"))
                {
                    Offset.z -= AdjustmentList[AdjustmentPosition];
                }

                GUILayout.EndHorizontal();

                GUILayout.EndArea();
            }

            GUILayout.BeginArea(new Rect(10, 175, 400, 200));

            GUILayout.BeginHorizontal();

            if (GUILayout.RepeatButton("Scale Up"))
            {
                Scale += new Vector3(AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition]);
                UIAddition.gravityIndicator.transform.localScale = Scale;
            }
            if (GUILayout.RepeatButton("Scale Down"))
            {
                Scale -= new Vector3(AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition]);
                UIAddition.gravityIndicator.transform.localScale = Scale;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(10, 200, 400, 200));

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("ResetPosition"))
            {
                Offset = new Vector3(0f, 0f, 100f);
            }
            if (GUILayout.Button("ResetScale"))
            {
                Scale = new Vector3(100f, 100f, 100f);
                UIAddition.gravityIndicator.transform.localScale = Scale;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Box($"Adjustment Factor: {AdjustmentList[AdjustmentPosition]}");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Increase Adjustment Factor"))
            {
                if (AdjustmentPosition == AdjustmentList.Length - 1)
                {
                    AdjustmentPosition = AdjustmentList.Length - 1;
                }
                else
                {
                    AdjustmentPosition++;
                }
            }
            if (GUILayout.Button("Decrease Adjustment Factor"))
            {
                if (AdjustmentPosition == 0)
                {
                    AdjustmentPosition = 0;
                }
                else
                {
                    AdjustmentPosition--;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    }
}

