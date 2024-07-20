using System.Collections.Generic;
using TapAndRun.Configs;
using TapAndRun.MVP.Levels.Model;
using TapAndRun.MVP.Levels.View;
using TapAndRun.Tools.LevelConstructor.BuildCommands;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace TapAndRun.Tools.LevelConstructor
{
    public class LevelContructorTool : EditorWindow
    {
        private float _fadePanel;
        private int _difficulty;
        private string _levelName;
        
        private LevelView _creatingLevel;

        private List<InteractType> _levelInteractions;

        private Stack<AbstractSegmentBuilder> _buildersStack;
        private LevelConstructorToolConfig _config;

        [MenuItem("My Tool/Show LevelConstructor")]
        public static void ShowWindow()
        {
            GetWindow<LevelContructorTool>(true, "Level Constructor");
        }
        private void OnEnable()
        {
            _buildersStack = new Stack<AbstractSegmentBuilder>();
            _levelInteractions = new List<InteractType>();
            _config = AssetDatabase.LoadAssetAtPath<LevelConstructorToolConfig>("Assets/_GameAssets/Configs/LevelConstructorToolConfig.asset");

            _fadePanel = 0f;
        }
        private void OnDestroy()
        {
            if(_creatingLevel) DestroyImmediate(_creatingLevel.gameObject, false);
        }

        private void CreateStartLevel()
        {
            _creatingLevel = new GameObject("Level").AddComponent<LevelView>();
            var startSegment = Instantiate(_config.StartSegment, _creatingLevel.transform);
            
            _creatingLevel.Segments.Add(startSegment);
            _creatingLevel.FinishSegment = startSegment;
            _creatingLevel.LevelNumberText = startSegment.GetComponentInChildren<TMP_Text>();
        }

        private void CreateFinishLevel()
        {
            var finishSegment = Instantiate(_config.FinishSegment, _creatingLevel.FinishSegment.SnapPoint);

            _creatingLevel.Segments.Add(finishSegment);
            _creatingLevel.FinishSegment = finishSegment;
        }

        private void ActivateBuilder(AbstractSegmentBuilder builder)
        {
            builder.Build();
            _buildersStack.Push(builder);
        }

        private void DeactivateBuilder()
        {
            if (_buildersStack.TryPop(out var builder))
            {
                builder.Debuild();
            }
            else
            {
                Debug.Log("Все сегменты уже удалены!");
            }
        }
        
        private void SaveLevelToPrefab()
        {
            string savePath = "Assets/_GameAssets/Prefabs/Levels/" + _levelName + ".prefab";

            savePath = AssetDatabase.GenerateUniqueAssetPath(savePath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(_creatingLevel.gameObject, savePath, InteractionMode.AutomatedAction);

            DestroyImmediate(_creatingLevel.gameObject ,false);
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Level Constructor - Tap And Run");

            if (GUILayout.Button("Create new level"))
            {
                _fadePanel = 1f;

                CreateStartLevel();
            }

            EditorGUILayout.Space(20);
            //---------------------------------------------------------------------------//  START MENU

            if (EditorGUILayout.BeginFadeGroup(_fadePanel))
            {
                //-----------------------------------------------------------------------//  ROAD SEGMENTS
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Small Road", GUILayout.Height(30)))
                {
                    var builder = new RoadSegmentBuilder(_config, _creatingLevel, 1);

                    ActivateBuilder(builder);
                }
                if (GUILayout.Button("Middle Road", GUILayout.Height(30)))
                {
                    var builder = new RoadSegmentBuilder(_config, _creatingLevel, 2);

                    ActivateBuilder(builder);
                }
                if (GUILayout.Button("Long Road", GUILayout.Height(30)))
                {
                    var builder = new RoadSegmentBuilder(_config, _creatingLevel, 3);

                    ActivateBuilder(builder);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(15);

                //---------------------------------------------------------------------------// TURN SEGMENTS
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Left Turn", GUILayout.Height(50)))
                {
                    var builder = new TurnLeftSegmentBuilder(_config, _creatingLevel);

                    ActivateBuilder(builder);
                }
                if (GUILayout.Button("Right Turn", GUILayout.Height(50)))
                {
                    var builder = new RightTurnSegmentBuilder(_config, _creatingLevel);

                    ActivateBuilder(builder);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(15);

                //---------------------------------------------------------------------------// JUMP SEGMENTS
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Start Jump", GUILayout.Height(30)))
                {
                    var builder = new StartJumpSegmentBuilder(_config, _creatingLevel);

                    ActivateBuilder(builder);
                }
                if (GUILayout.Button("Single Jump", GUILayout.Height(30)))
                {
                    var builder = new JumpSegmentBuilder(_config, _creatingLevel);

                    ActivateBuilder(builder);
                }
                if (GUILayout.Button("End Jump", GUILayout.Height(30)))
                {
                    var builder = new EndJumpSegmentBuilder(_config, _creatingLevel);

                    ActivateBuilder(builder);
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(30);
                //---------------------------------------------------------------------------// SEMGENTS DELETING

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Debuild Last Segment", GUILayout.Height(30)))
                {
                    DeactivateBuilder();
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(30);
                //---------------------------------------------------------------------------// DIFFICULTY SETTING

                EditorGUILayout.BeginHorizontal();

                _difficulty = EditorGUILayout.IntSlider(_difficulty, 1, 5);
                EditorGUILayout.LabelField("Difficulty");

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(15);
                //---------------------------------------------------------------------------// LEVEL NAME

                EditorGUILayout.BeginHorizontal();

                _levelName = EditorGUILayout.TextField(_levelName);
                EditorGUILayout.LabelField("Write level name");

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(15);
                //---------------------------------------------------------------------------// ЗАВЕРШЕНИЕ СОЗДАНИЯ УРОВНЯ

                if (GUILayout.Button("Finish Level"))
                {
                    CreateFinishLevel();
                    _creatingLevel.Difficulty = _difficulty;

                    SaveLevelToPrefab();

                    _fadePanel = 0;
                }
            }
            EditorGUILayout.EndFadeGroup();
        }
    }
}
