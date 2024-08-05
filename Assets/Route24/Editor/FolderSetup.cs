using UnityEditor;
using UnityEngine;
using System.IO;

namespace Route24.Editor
{
    public static class FolderSetup
    {
        [MenuItem("Tools/Route 24/Setup Folders")]
        public static void SetupFolderStructure()
        {
            var continueFolderSetup = EditorUtility.DisplayDialog(
                "Setup Folders",
                "Would you like to setup the folder structure?",
                "Yes",
                "Cancel");

            if (continueFolderSetup)
            {
                const string rootFolder = "_Content";
                string[] folders = new string[]
                {
                    "Art",
                    "Audio",
                    "Animations",
                    "Editor",
                    "Prefabs",
                    "Materials",
                    "Textures",
                    "Shaders",
                    "Resources",
                    "ScriptableObjects",
                    "ScriptableObjects/Data",
                    "ScriptableObjects/Configs",
                    "ScriptableObjects/Events",
                    "UI",
                    "_Scripts",
                    "_Scripts/Components",
                    "_Scripts/Controllers",
                    "_Scripts/Enums",
                    "_Scripts/EventHub",
                    "_Scripts/Input",
                    "_Scripts/Interfaces",
                    "_Scripts/Managers",
                    "_Scripts/ScriptableObjects",
                    "_Scripts/Systems",
                    "_Scripts/Tests",
                    "_Scripts/UI",
                    "_Scripts/Utilities",
                };  
                
                Create(rootFolder, folders);
                AssetDatabase.Refresh();
                
                Move(rootFolder, "Scenes");
                Move(rootFolder, "Settings");
                Delete("TutorialInfo");
                AssetDatabase.Refresh();
                
                const string pathToReadme = "Assets/Readme.asset";
                AssetDatabase.DeleteAsset(pathToReadme);
            }
        }

        private static void Create(string root, string[] folders)
        {
            var rootPath = Path.Combine(Application.dataPath, root);
            
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            
            foreach (var folder in folders)
            {
                CreateSubFolder(rootPath, folder);
            }
        }

        private static void CreateSubFolder(string rootPath, string folderHierachy)
        {
            var folders = folderHierachy.Split("/");
            var currentPath = rootPath;

            foreach (var subfolder in folders)
            {
                currentPath = Path.Combine(currentPath, subfolder);
                
                if (!Directory.Exists(currentPath))
                {
                    Directory.CreateDirectory(currentPath);
                }
            }
        }

        public static void Move(string newParent, string folderName)
        {
            string sourcePath = $"Assets/{folderName}";
            
            if (AssetDatabase.IsValidFolder(sourcePath))
            {
                string destinationPath = $"Assets/{newParent}/{folderName}";
                string error = AssetDatabase.MoveAsset(sourcePath, destinationPath);

                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError($"Error moving {folderName} to {newParent}: {error}");
                }
            }
        }
        
        public static void Delete(string folderName)
        {
            string sourcePath = $"Assets/{folderName}";
            
            if (AssetDatabase.IsValidFolder(sourcePath))
            {
                AssetDatabase.DeleteAsset(sourcePath);
            }
        }
    }   
}
