using System;									// System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;							// System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game
{
	Scene currentScene;
	UI ui;

    public string currentMapName = "maps/Main Menu.tmx";

    bool levelLoad = false;

    //public MyGame() : base(683, 384, true, true, 1366, 768, true)		// Create a window that's 1366x768 and fullscreen
    public MyGame() : base(683, 384, false, false, 683, 384, true)      // Create a window that's 683x384 and NOT fullscreen
    {
		createLevel(currentMapName);
		Console.WriteLine("MyGame initialized");
	}

	// For every game object, Update is called every frame, by the engine:
	void Update()
	{
		ui.clearText(); //since this is always the first update called, clear the ui text here

		//DEBUG reload level
		if(Input.GetKeyDown(Key.R))
        {
			currentScene.LateDestroy();
			ui.LateDestroy();
        }
		if(Input.GetKeyUp(Key.R))
        {
			createLevel(currentMapName);
        }

		//if we are currently loading a new level, actually do it
		if(levelLoad)
        {
			createLevel(currentMapName);
			levelLoad = false;
            Console.WriteLine("new Level loaded");
        }
	}

	/// <summary>
	/// Will create a new level and UI based on the name given
	/// </summary>
	/// <param name="mapName">fileName of the new level</param>
	void createLevel(String mapName)
    {
		createUI();
		currentScene = new Scene(ui);
		AddChild(currentScene);
		currentScene.loadMapFile(mapName);
		currentScene.createLevel();

		//set the index of the ui to be in front of the level
		this.SetChildIndex(ui, GetChildCount()) ;
		AddChild(new Cursor());
	}

	/// <summary>
	/// Will create a new UI
	/// </summary>
	void createUI()
    {
		ui = new UI(width, height);
		AddChild(ui);
    }

	/// <summary>
	/// Will initialize the loading of a new level, actual level will be created next frame
	/// </summary>
	/// <param name="newMapName"></param>
	public void loadNewLevel(string newMapName)
    {
		currentScene.LateDestroy();
		ui.LateDestroy();
		currentMapName = newMapName;
		levelLoad = true;
    }

	static void Main()							// Main() is the first method that's called when the program is run
	{
		new MyGame().Start();					// Create a "MyGame" and start it
	}
}