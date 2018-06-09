﻿using UnityEditor;
using UnityEngine;


namespace Runningtap
{
    public partial class Substance
    {
        public MaterialProperty RoughnessDetailMap;
        public MaterialProperty RoughnessSpread;
        public MaterialProperty RoughnessContrast;
    }

    public class PBRBlendRough : ShaderGUI
    {
        Substance S01 = new Substance();
        MaterialEditor matEditor;

        float noiseZoom = 1;

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Common.FindProperties(S01, 1, properties);
            matEditor = materialEditor;
            Material material = materialEditor.target as Material;

            ShaderPropertiesGUI(material);
        }

        public void ShaderPropertiesGUI(Material material)
        {
            EditorGUIUtility.labelWidth = 0f;

            EditorGUI.BeginChangeCheck();
            {
                Rect logoArea = GUILayoutUtility.GetRect(128, 32, GUILayout.ExpandWidth(false));
                GUI.DrawTexture(logoArea, Common.runningtapLogo, ScaleMode.ScaleToFit, true, 0);

                EditorGUILayout.Space();
                EditorGUILayout.Space();

                Common.DropAreaGUI(S01);

                EditorGUILayout.Space();

                matEditor.TexturePropertySingleLine(Styles.baseColorText, S01.BaseColorMap, S01.BaseColor);
                matEditor.TexturePropertySingleLine(Styles.metallicMapText, S01.MetallicMap);
                matEditor.TexturePropertySingleLine(Styles.normalMapText, S01.NormalMap);
                matEditor.TexturePropertySingleLine(Styles.heightMapText, S01.HeightMap, S01.HeightMap.textureValue != null ? S01.HeightScale : null);
                matEditor.TexturePropertySingleLine(Styles.occlusionMapText, S01.OcclusionMap);

                EditorGUILayout.Space();

                EditorGUI.BeginChangeCheck();
                matEditor.TextureScaleOffsetProperty(S01.BaseColorMap);
                if (EditorGUI.EndChangeCheck())
                {
                    S01.MetallicMap.textureScaleAndOffset = S01.BaseColorMap.textureScaleAndOffset;
                    S01.NormalMap.textureScaleAndOffset = S01.BaseColorMap.textureScaleAndOffset;
                    S01.HeightMap.textureScaleAndOffset = S01.BaseColorMap.textureScaleAndOffset;
                    S01.OcclusionMap.textureScaleAndOffset = S01.BaseColorMap.textureScaleAndOffset;
                }
                

                EditorGUILayout.Space();

                matEditor.TexturePropertySingleLine(Styles.roughnessMapText, S01.RoughnessDetailMap);
                matEditor.RangeProperty(S01.RoughnessSpread, "Spread");
                matEditor.RangeProperty(S01.RoughnessContrast, "Contrast");

                EditorGUILayout.Space();

                EditorGUI.BeginChangeCheck();
                    matEditor.TextureScaleOffsetProperty(S01.RoughnessDetailMap);
                EditorGUI.EndChangeCheck();

                EditorGUILayout.Space();
                matEditor.TexturePropertySingleLine(Styles.blendNoiseMapText, S01.BlendNoiseMap);
                if (S01.BlendNoiseMap.textureValue != null)
                {
                    noiseZoom = EditorGUILayout.FloatField("Noise Zoom", noiseZoom);
                    S01.BlendNoiseMap.textureScaleAndOffset = new Vector2(noiseZoom, noiseZoom);
                }

                EditorGUILayout.Space();
                matEditor.ShaderProperty(S01.Debug, "Debug");
                EditorGUILayout.Space();
            }
        }
    }
}