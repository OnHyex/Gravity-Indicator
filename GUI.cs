using System;
using System.Collections.Generic;
using PulsarModLoader.CustomGUI;
using PulsarModLoader;
using UnityEngine;
using JetBrains.Annotations;
using System.Runtime.Serialization.Json;

namespace GravityIndicator
{
    internal class GUI : ModSettingsMenu
    {
        static SaveValue<float> GravityIndicatorOffsetX = new SaveValue<float>("GravityIndicatorOffsetX", 0f);
        static SaveValue<float> GravityIndicatorOffsetY = new SaveValue<float>("GravityIndicatorOffsetX", 0f);
        static SaveValue<float> GravityIndicatorOffsetZ = new SaveValue<float>("GravityIndicatorOffsetX", 100f);
        static SaveValue<float> GravityIndicatorUIOffsetX = new SaveValue<float>("GravityIndicatorUIOffsetX", 0f);
        static SaveValue<float> GravityIndicatorUIOffsetY = new SaveValue<float>("GravityIndicatorUIOffsetY", 0f);
        static SaveValue<float> GravityIndicatorUIOffsetZ = new SaveValue<float>("GravityIndicatorUIOffsetZ", 0f);
        static SaveValue<float> GravityIndicatorScale = new SaveValue<float>("GravityIndicatorScale", 100f);
        static SaveValue<float> GravityIndicatorUIScale = new SaveValue<float>("GravityIndicatorUIScale", 100f);
        internal static SaveValue<bool> EnabledInSensorDish = new SaveValue<bool>("EnabledInSensorDish", false);
        internal static SaveValue<int> ElementMode = new SaveValue<int>("ElementMode", 0);
        internal static Vector3 Offset = new Vector3(GravityIndicatorOffsetX, GravityIndicatorOffsetY, GravityIndicatorOffsetZ);
        internal static Vector3 UIOffset = new Vector3(GravityIndicatorUIOffsetX, GravityIndicatorUIOffsetY, GravityIndicatorUIOffsetZ);
        internal static Vector3 Scale = new Vector3(GravityIndicatorScale.Value, GravityIndicatorScale.Value, GravityIndicatorScale.Value);
        internal static Vector3 UIScale = new Vector3(GravityIndicatorUIScale.Value, GravityIndicatorUIScale.Value, GravityIndicatorUIScale.Value);
        private int AdjustmentPosition = 2;
        private string[] Modes = new string[] { "UI Element", "Ship Follow" };
        private float[] AdjustmentList = { 0.01f, 0.1f, 1f, 10f, 100f };
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
            GravityIndicatorUIOffsetX.Value = UIOffset.x;
            GravityIndicatorUIOffsetY.Value = UIOffset.y;
            GravityIndicatorUIOffsetZ.Value = UIOffset.z;
            GravityIndicatorScale.Value = Scale.x;
            GravityIndicatorUIScale.Value = UIScale.x;
        }
        public override void Draw()
        {
            GUILayout.BeginHorizontal();

            IndicatorManager.IndicatorEnabled = GUILayout.Toggle(IndicatorManager.IndicatorEnabled, "Enabled Indicator", Array.Empty<GUILayoutOption>());

            EnabledInSensorDish.Value = GUILayout.Toggle(EnabledInSensorDish.Value, "Enabled in Sensor Dish");

            GUILayout.EndHorizontal();
            GUILayout.BeginArea(new Rect(10, 50, 400, 150));

            GUILayout.BeginHorizontal();

            GUILayout.Box($"Follow Mode: {Modes[ElementMode.Value]}");

            if(GUILayout.Button("Switch Mode"))
            {
                if(ElementMode.Value >= Modes.Length - 1)
                {
                    ElementMode.Value = 0;
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
                if (ElementMode == 0)
                {
                    UIOffset.y += AdjustmentList[AdjustmentPosition];
                }
                else
                {
                    Offset.y += AdjustmentList[AdjustmentPosition];
                }
                
            }
            if (GUILayout.RepeatButton("Down"))
            {
                if (ElementMode == 0)
                {
                    UIOffset.y -= AdjustmentList[AdjustmentPosition];
                }
                else
                {
                    Offset.y -= AdjustmentList[AdjustmentPosition];
                }
                
            }
            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(10, 125, 400, 50));

            GUILayout.BeginHorizontal();

            if (GUILayout.RepeatButton("Left"))
            {
                if (ElementMode == 0)
                {
                    UIOffset.x -= AdjustmentList[AdjustmentPosition];
                }
                else
                {
                    Offset.x -= AdjustmentList[AdjustmentPosition];
                }
                
            }
            if (GUILayout.RepeatButton("Right"))
            {
                if (ElementMode == 0)
                {
                    UIOffset.x += AdjustmentList[AdjustmentPosition];
                }
                else
                {
                    Offset.x += AdjustmentList[AdjustmentPosition];
                }
                
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
                if (ElementMode == 0)
                {
                    UIScale += new Vector3(AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition]);
                    
                }
                else
                {
                    Scale += new Vector3(AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition]);
                }
                    
            }
            if (GUILayout.RepeatButton("Scale Down"))
            {
                if (ElementMode == 0)
                {
                    UIScale -= new Vector3(AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition]);
                }
                else
                {
                    Scale -= new Vector3(AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition], AdjustmentList[AdjustmentPosition]);
                }
                    
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(10, 200, 400, 200));

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("ResetPosition"))
            {
                if (ElementMode.Value == 0)
                {
                    UIOffset = new Vector3(0f, 0f, 100f);
                }
                else
                {
                    Offset = new Vector3(0f, 0f, 0f);
                }
            }
            if (GUILayout.Button("ResetScale"))
            {
                if (ElementMode.Value == 0)
                {
                    UIScale = new Vector3(100f, 100f, 100f);
                }
                else
                {
                    Scale = new Vector3(100f, 100f, 100f);
                }
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
            if (ElementMode == 0)
            {
                IndicatorManager.gravityIndicator.transform.localScale = UIScale;
            }
            else
            {
                IndicatorManager.gravityIndicator.transform.localScale = Scale;
            }
            
        }
    }
}

