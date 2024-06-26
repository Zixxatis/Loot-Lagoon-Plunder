using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CGames.CustomEditors
{
    public class SpritesMetaChanger : EditorWindow
    {
        private const int DEFAULT_PPU = 32;

        private string folderPathInAssets;
        private int newPixelsPerUnit;
        private List<string> pngMetaFiles;

        private bool shouldChangeCompression;
        private bool shouldChangeFilterMode;

        private string FullPathToRootFolder => Path.Combine(Path.GetDirectoryName(Application.dataPath), folderPathInAssets);

        [MenuItem("Tools/Editor Windows/Sprites Meta Changer")]
        public static void Init()
        {
            SpritesMetaChanger spritePPUChangerWindow = GetWindow<SpritesMetaChanger>("Sprites Meta Changer");

            spritePPUChangerWindow.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Reset values"))
                Reset();
            
            GUILayout.Space(5);

            GUILayout.Label("Enter ROOT Folder Path (from Assets):");
            folderPathInAssets = EditorGUILayout.TextArea(folderPathInAssets);

            GUILayout.Label("Enter new Pixels per Unit:");
            newPixelsPerUnit = EditorGUILayout.IntField(newPixelsPerUnit);

            GUILayout.Label("Should change compression to \"None\"?");
            shouldChangeCompression = EditorGUILayout.Toggle(shouldChangeCompression);

            GUILayout.Label("Should change filter mode to \"None (Point)\"?");
            shouldChangeFilterMode = EditorGUILayout.Toggle(shouldChangeFilterMode);

            GUILayout.Space(5);

            if (GUILayout.Button("Change .png files' meta'"))
            {
                if (Directory.Exists(FullPathToRootFolder) == false)
                {
                    Debug.LogError($"Given directory {FullPathToRootFolder} doesn't exist.");
                    return;
                }

                if (newPixelsPerUnit <= 0)
                {
                    Debug.LogError("Please set Pixels per Unit to be greater than zero.");
                    return;
                }

                FindAllMetaFiles();

                int changed = 0;

                foreach (string filePath in pngMetaFiles)
                {
                    string[] lines = File.ReadAllLines(filePath);

                    ChangePPU(lines, filePath);

                    if(shouldChangeCompression)
                        ChangeCompression(lines, filePath);

                    if(shouldChangeFilterMode)
                        ChangeFilterMode(lines, filePath);

                    File.WriteAllLines(filePath, lines);
                    changed ++;
                }

                Debug.Log($"Changed x{changed} \".png.meta\" files.");

                AssetDatabase.Refresh();
            }
        }

        private void FindAllMetaFiles()
        {
            pngMetaFiles = new();

            FindPngMetaFilesInDirectory(FullPathToRootFolder);

            Debug.Log($"Found x{pngMetaFiles.Count} \".png.meta\" files.");
        }

        private void FindPngMetaFilesInDirectory(string path)
        {
            try
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    if (file.EndsWith(".png.meta"))
                    {
                        pngMetaFiles.Add(file);
                    }
                }

                foreach (string directory in Directory.GetDirectories(path))
                {
                    FindPngMetaFilesInDirectory(directory);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }

        private void ChangePPU(string[] lines, string filePath)
        {
            if(TryFindLineIndex(lines, "spritePixelsToUnits: ", out int index))
                ChangeLine(lines, $": {newPixelsPerUnit}", index);
            else
                Debug.LogWarning($"Meta file doesn't contain \"spritePixelsToUnits\". (Path: {filePath})");
        }

        private void ChangeCompression(string[] lines, string filePath)
        {
            if(TryFindLineIndex(lines, "textureCompression: ", out int index))
                ChangeLine(lines, ": 0", index);
            else
                Debug.LogWarning($"Meta file doesn't contain \"textureCompression\". (Path: {filePath})");
        }

        private void ChangeFilterMode(string[] lines, string filePath)
        {
            if(TryFindLineIndex(lines, "filterMode: ", out int index))
                ChangeLine(lines, ": 0", index);
            else
                Debug.LogWarning($"Meta file doesn't contain \"filterMode\". (Path: {filePath})");
        }
        
        private bool TryFindLineIndex(string[] lines, string keyWord, out int index)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(keyWord))
                {
                    index = i;
                    return true;
                } 
            }
            
            index = -1;
            return false;
        }

        private static void ChangeLine(string[] lines, string newValue, int index)
        {
            string[] parts = lines[index].Split(':');
            parts[1] = newValue;

            lines[index] = parts[0] + parts[1];
        }

        private void Reset()
        {
            folderPathInAssets = string.Empty;
            newPixelsPerUnit = DEFAULT_PPU;
            pngMetaFiles = new();

            shouldChangeCompression = false;
        }
    }
}