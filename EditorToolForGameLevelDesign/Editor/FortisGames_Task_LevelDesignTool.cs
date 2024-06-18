using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;

public class FortisGames_Task_LevelDesignTool : EditorWindow
{
    const string ToolGUISkinPath = "GUISkiin/ToolSkin";
    const string EditorToolMenuPath = "FortisGames Tools/Level Design Tool";
    const string LevelDataPath = "Assets/Resources/LevelData.asset";

    Color HeaderBGColor = new Color(34f/255f,25f/255f,41f/255f,1f);
    Color MainMenuBGColor = new Color(34f / 255f, 25f / 255f, 41f / 255f, 1f);
    Color LevelMenuBGColor = new Color(190f / 255f, 99f / 255f, 76f / 255f, 1f);

    LevelDataContainer levelDataContainer;

    Texture2D HeaderSectionTexture;
    Texture2D MiniClipLogoTexture;
    Texture2D MainMenuBGTexture;
    Texture2D LevelMenuBGTexture;

    Texture2D[] TileTexture;

    Rect HeaderSection;
    Rect MiniClipLogoRect;
    Rect MainMenuSection;
    Rect LevelMenuSection;

    GUISkin skin;

    bool toggleMainMenuLevelMenu = true; // true for main menu and false for level menu


    int selectedLevelIndex = 0;
    LevelData selectedLevelData = null;
    LevelData restoreState = null;
    int drawableTileValue = 3;
    int drawableMapDimensionX;
    int drawableMapDimensionY;
    int tileSize = 64;
    int selectionPaletteTileSize = 64;

    Vector2 scrollPosition;


    [MenuItem(EditorToolMenuPath)]
    static void OpenWindow()
    {
        FortisGames_Task_LevelDesignTool tool = (FortisGames_Task_LevelDesignTool)GetWindow(typeof(FortisGames_Task_LevelDesignTool));
        tool.minSize = new UnityEngine.Vector2 (899, 600);
        tool.maxSize = new UnityEngine.Vector2(900, 600);
        tool.Show();
    }
    /// <summary>
    /// similar to Start or Awake
    /// </summary>
    void OnEnable()
    {
        InitTextures();
        InitLevelData();
        skin = Resources.Load<GUISkin>(ToolGUISkinPath);
    }

    private void OnDisable()
    {
        Resources.UnloadAsset(skin);
        AssetDatabase.SaveAssets();
        //Resources.UnloadAsset(levelDataContainer);
    }

    void InitLevelData()
    {
        levelDataContainer = AssetDatabase.LoadAssetAtPath<LevelDataContainer>(LevelDataPath);// Resources.Load<LevelDataContainer>(LevelDataPath);
    }
    /// <summary>
    /// Initialize Texture2d Values
    /// </summary>
    void InitTextures() 
    {
        HeaderSectionTexture = new Texture2D(1, 1);
        HeaderSectionTexture.SetPixel(0, 0, HeaderBGColor);
        HeaderSectionTexture.Apply();

        MainMenuBGTexture = new Texture2D(1, 1);
        MainMenuBGTexture.SetPixel(0, 0, MainMenuBGColor);
        MainMenuBGTexture.Apply();

        LevelMenuBGTexture = new Texture2D(1, 1);
        LevelMenuBGTexture.SetPixel(0, 0, LevelMenuBGColor);
        LevelMenuBGTexture.Apply();

        MiniClipLogoTexture = Resources.Load<Texture2D>("ToolIcon");

        TileTexture = new Texture2D[4];
        TileTexture[0] = Resources.Load<Texture2D>("TilesTextures/Empty");
        TileTexture[1] = Resources.Load<Texture2D>("TilesTextures/Water");
        TileTexture[2] = Resources.Load<Texture2D>("TilesTextures/Wall");
        TileTexture[3] = Resources.Load<Texture2D>("TilesTextures/Soil");

    }

    /// <summary>
    /// trigerred on any interaction with the window
    /// </summary>
    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        if (toggleMainMenuLevelMenu)
            DrawMainMenu();
        else
            DrawLevelMenu();
    }

    void DrawLayouts()
    {
        
    }

    void DrawHeader()
    {
        HeaderSection.x = 0;
        HeaderSection.y = 0;
        HeaderSection.width = this.maxSize.x ;
        HeaderSection.height = 75;

        MiniClipLogoRect.width = 400;
        MiniClipLogoRect.height = 75;
        MiniClipLogoRect.x = HeaderSection.width / 2 - (MiniClipLogoRect.width / 2);
        MiniClipLogoRect.y = HeaderSection.height / 2 - (MiniClipLogoRect.height / 2);

        GUI.DrawTexture(HeaderSection, HeaderSectionTexture);
        GUI.DrawTexture(MiniClipLogoRect, MiniClipLogoTexture);

    }

    void DrawMainMenu()
    {
        MainMenuSection.x = 0;
        MainMenuSection.y = HeaderSection.height;
        MainMenuSection.width = Screen.width;
        MainMenuSection.height = 240;
        GUI.DrawTexture(MainMenuSection, MainMenuBGTexture);

        GUILayout.BeginArea(MainMenuSection);

        //Space
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(20);
        EditorGUILayout.EndVertical();

        //Level Selection
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Choose Level to Edit:   ",skin.GetStyle("MainMenu"));
        selectedLevelIndex = EditorGUILayout.Popup(selectedLevelIndex, levelDataContainer.Levels.Select(level => (level as LevelData).LevelID.ToString()).ToArray(), GUILayout.Width(200));
        EditorGUILayout.EndHorizontal();

        
       
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = levelDataContainer.Levels.Count != 0;
        GUILayout.Space(250);
        if (GUILayout.Button("Load Level", skin.GetStyle("DeleteBtn")))
        {
            selectedLevelData = (LevelData)levelDataContainer.Levels[selectedLevelIndex].Clone(); //new LevelData() { LevelID = levelDataContainer.levels[selectedLevelIndex].LevelID, DimentionX = levelDataContainer.levels[selectedLevelIndex].DimentionX, DimentionY = levelDataContainer.levels[selectedLevelIndex].DimentionY, TilesData = levelDataContainer.levels[selectedLevelIndex].TilesData };
            drawableMapDimensionX = selectedLevelData.DimensionX ;
            drawableMapDimensionY = selectedLevelData.DimensionY;
            toggleMainMenuLevelMenu = false;
            restoreState = (LevelData)selectedLevelData.Clone();
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Delete Level", skin.GetStyle("DeleteBtn")))
        {
            if (selectedLevelIndex < levelDataContainer.Levels.Count)
            {
                levelDataContainer.Levels.RemoveAt(selectedLevelIndex);
                EditorUtility.SetDirty(levelDataContainer);
                AssetDatabase.SaveAssetIfDirty(levelDataContainer);
            }

         }
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(240); GUILayout.Label("OR", skin.GetStyle("MainMenu"));
        EditorGUILayout.EndHorizontal();

        //Create new level button
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(350); 
        if (GUILayout.Button("Create new level", skin.GetStyle("button")))
        {
            drawableMapDimensionX = 4;
            drawableMapDimensionY = 4;
            if (levelDataContainer.Levels.Count == 0)
            {
                selectedLevelData = new LevelData(0, drawableMapDimensionX, drawableMapDimensionY);// { LevelID = 0 , DimentionX = 4, DimentionY = 4 , TilesData = new List<int>() };
            }
            else
                selectedLevelData = new LevelData(levelDataContainer.Levels.Max(lvl => lvl.LevelID) + 1, drawableMapDimensionX, drawableMapDimensionY);

            selectedLevelIndex = selectedLevelData.LevelID;

            toggleMainMenuLevelMenu = false;
        }
        EditorGUILayout.EndHorizontal();

        //Border
        EditorGUI.DrawRect(new Rect(0, 160, Screen.width, 2), Color.white);

        EditorGUILayout.Space(10);

        //Serialize Button Area
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(150);
        if (GUILayout.Button("Serialize To File", skin.GetStyle("button")))
        {
            string path = EditorUtility.SaveFilePanel("Save Level Data To", "","FortixGameLevelData", "txt");
            if (path.Length != 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in levelDataContainer.Levels)
                {
                    builder.Append(item.Serialize());
                    builder.Append('@');
                }

                //string data = EditorJsonUtility.ToJson(levelDataContainer);
                File.WriteAllText(path, builder.ToString());
                EditorUtility.DisplayDialog("Complete", $"File written on path {path}", "OK");
                // texture.LoadImage(fileContent);
            }
        }
        GUILayout.Space(-100);
        if (GUILayout.Button("Load File", skin.GetStyle("button")))
        {
            try
            {
                string path = EditorUtility.OpenFilePanel("Load Level Data", "", "txt");
                if (path.Length != 0)
                {
                    var fileContent = File.ReadAllText(path);
                    string[] levels = fileContent.Split('@');
                    LevelDataContainer allData = ScriptableObject.CreateInstance<LevelDataContainer>();
                    foreach (var level in levels)
                    {
                        if (level != string.Empty)
                        {
                            LevelData readLevel = LevelData.DeSerialize(level);
                            if (readLevel != null)
                            {
                                allData.Levels.Add(readLevel);
                            }
                        }

                    }

                    if (allData != null)
                    {
                        levelDataContainer.Levels.Clear();
                        foreach (var item in allData.Levels)
                        {
                            levelDataContainer.Levels.Add(item);
                        }
                        EditorUtility.SetDirty(levelDataContainer);
                        AssetDatabase.SaveAssets();
                    }
                    // texture.LoadImage(fileContent);
                }
            }
            catch (System.Exception)
            {
                EditorUtility.DisplayDialog("Error", "UnSupported file Loaded", "OK");
            }
           
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void DrawLevelMenu()
    {
        LevelMenuSection.x = 0;
        LevelMenuSection.y = HeaderSection.height + 5;
        LevelMenuSection.width = Screen.width;
        LevelMenuSection.height = 500;

        GUI.DrawTexture(LevelMenuSection, LevelMenuBGTexture);

        GUILayout.BeginArea(LevelMenuSection);
        EditorGUILayout.Space(5);
        //Level ID
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label($"Level ID : {selectedLevelData.LevelID}", skin.GetStyle("textarea"),GUILayout.Width(100), GUILayout.Height(20));

        //XY coordinates input
        GUILayout.Space(30);
        EditorGUIUtility.labelWidth=20;
        drawableMapDimensionX = EditorGUILayout.IntField("X  :", drawableMapDimensionX, skin.GetStyle("textarea"), GUILayout.Height(20), GUILayout.Width(50));
        GUILayout.Space(30);
        drawableMapDimensionY = EditorGUILayout.IntField("Y:", drawableMapDimensionY, skin.GetStyle("textarea"), GUILayout.Height(20), GUILayout.Width(50));
        //Update Grid Button
        GUILayout.Space(30);
        if (GUILayout.Button("Update Grid", GUILayout.Width(150), GUILayout.Height(20)))
        {
            if (drawableMapDimensionX != selectedLevelData.DimensionX )
            {
                selectedLevelData.UpdateCol(drawableMapDimensionX, 2);
                selectedLevelData.DimensionX  = drawableMapDimensionX;
            }
            if (drawableMapDimensionY != selectedLevelData.DimensionY)
            {
                selectedLevelData.UpdateRow(drawableMapDimensionY, 2);
                selectedLevelData.DimensionY = drawableMapDimensionY;
            }

        }
        //Tile Size Input
        GUILayout.Space(250);
        EditorGUIUtility.labelWidth = 50;
        tileSize = EditorGUILayout.IntField("TileSize:", tileSize, GUILayout.Height(20), GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        //Space
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(20);
        EditorGUILayout.EndVertical();

        Rect contentArea = new Rect(50, 40, 810 - 10, 311 - 10);

        // Draw the border using EditorGUI.DrawRect
        EditorGUI.DrawRect(new Rect(45, 35, 810, 310), Color.white);
        EditorGUI.DrawRect(contentArea, Color.black);

        GUILayout.BeginArea(contentArea);
        //Grid
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(); 
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300), GUILayout.Width(790));
        for (int y = 0; y < selectedLevelData.DimensionY; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < selectedLevelData.DimensionX ; x++)
            {
                Rect cellRect = GUILayoutUtility.GetRect(tileSize, tileSize, GUILayout.Height(tileSize), GUILayout.Width(tileSize));
                GUI.DrawTexture(cellRect, TileTexture[selectedLevelData.GetValue(x, y)], ScaleMode.ScaleToFit);
                Event e = Event.current;
                if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && cellRect.Contains(e.mousePosition))
                {
                    selectedLevelData.SetValue(x, y, drawableTileValue);
                    e.Use();
                }
                GUILayout.Space(.05f*tileSize);
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(.05f * tileSize);
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        GUILayout.EndArea();

        //Space
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(320);
        EditorGUILayout.EndVertical();

        //available Tile Palette
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Chose here to Draw :", skin.GetStyle("textarea"),GUILayout.Width(150), GUILayout.Height(20));
        Rect emptyRect = GUILayoutUtility.GetRect(selectionPaletteTileSize, selectionPaletteTileSize, GUILayout.Height(selectionPaletteTileSize), GUILayout.Width(selectionPaletteTileSize));
        GUI.DrawTexture(emptyRect, TileTexture[0], ScaleMode.ScaleToFit);
        GUILayout.Space(10);
        Rect WaterRect = GUILayoutUtility.GetRect(selectionPaletteTileSize, selectionPaletteTileSize, GUILayout.Height(selectionPaletteTileSize) ,GUILayout.Width(selectionPaletteTileSize));
        GUI.DrawTexture(WaterRect, TileTexture[1], ScaleMode.ScaleToFit);
        GUILayout.Space(10);
        Rect WallRect = GUILayoutUtility.GetRect(selectionPaletteTileSize, selectionPaletteTileSize, GUILayout.Height(selectionPaletteTileSize), GUILayout.Width(selectionPaletteTileSize));
        GUI.DrawTexture(WallRect, TileTexture[2], ScaleMode.ScaleToFit);
        GUILayout.Space(10);
        Rect GrassRect = GUILayoutUtility.GetRect(selectionPaletteTileSize, selectionPaletteTileSize, GUILayout.Height(selectionPaletteTileSize), GUILayout.Width(selectionPaletteTileSize));
        GUI.DrawTexture(GrassRect, TileTexture[3], ScaleMode.ScaleToFit);

        GUILayout.Space(-50);
        //Save Button
        if (GUILayout.Button("Save", skin.GetStyle("LevelMenuButton"), GUILayout.Width(120), GUILayout.Height(32)))
        {
        bool _isupdate = false;
        for (int i = 0; i < levelDataContainer.Levels.Count; i++)
            {
                if (levelDataContainer.Levels[i].LevelID == selectedLevelData.LevelID)
                    {
                        levelDataContainer.Levels[i] = (LevelData)selectedLevelData.Clone();
                        _isupdate = true; break;
                    }
            }
            if (!_isupdate)
                levelDataContainer.Levels.Add((LevelData)selectedLevelData.Clone());
            EditorUtility.SetDirty(levelDataContainer);
            toggleMainMenuLevelMenu = true;
        }

        GUILayout.Space(20);

        //Cancel button
        if (GUILayout.Button("Cancel", skin.GetStyle("LevelMenuButton"), GUILayout.Width(120), GUILayout.Height(32)))
        {
            if (restoreState!= null)
            {
                selectedLevelData = (LevelData)restoreState.Clone();
                restoreState = null;
                EditorUtility.SetDirty(levelDataContainer);

            }
            toggleMainMenuLevelMenu = true;
        }

        //Event handdler for tile selection
        Event evt = Event.current;
        if (evt.type == EventType.MouseDown )
        {
            if (WaterRect.Contains(evt.mousePosition))
            {
                Debug.Log("water");
                drawableTileValue = 1;
            }
            else if (WallRect.Contains(evt.mousePosition))
            {
                Debug.Log("wall");
                drawableTileValue = 2;

            }
            else if (GrassRect.Contains(evt.mousePosition))
            {
                Debug.Log("grass");
                drawableTileValue = 3;
            }
            else if (emptyRect.Contains(evt.mousePosition))
            {
                Debug.Log("delete");
                drawableTileValue = 0;
            }

            evt.Use();
        }
 
        EditorGUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    void SaveDataPreset()
    {
        LevelData existinglevel = levelDataContainer.Levels.First(x => x.LevelID == selectedLevelData.LevelID);
        if (existinglevel == null)
            levelDataContainer.Levels.Add(selectedLevelData);
        else
            existinglevel = selectedLevelData;
        EditorUtility.SetDirty(levelDataContainer);
        toggleMainMenuLevelMenu = true;
    }
}
