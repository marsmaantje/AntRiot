using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;
using Objects;
using UIElements;
using Scripts;

class Player : EasyDraw
{
    private const int objectWidth = 14;
    private const int objectHeight = 22;

    //tiled object to read data from
    TiledObject obj;

    //movement speed
    private float movementSpeed = 50;
    private float stoppedMargin = 1f;

    private Scene parentScene;
    private AnimationSprite animation;

    //camera target when following the player
    public Pivot cameraTarget;
    private float cameraSize = 6f;

    ControllerScript controller;


    public Player() : base(objectWidth, objectHeight, true) { readVariables(); }

    public Player(TiledObject obj) : base(objectWidth, objectHeight, true)
    {
        this.obj = obj;
    }

    public Player(String filename, int cols, int rows, TiledObject obj) : base(objectWidth, objectHeight, true)
    {
        this.obj = obj;
    }

    /// <summary>
    /// setup the visual for the player,
    /// visual is seperate for collision reasons
    /// </summary>
    private void createAnimation()
    {
        animation = new AnimationSprite("sprites/OrangeRobot_SpriteSheet.png", 8, 5, -1, true, false);
        AddChild(animation);
        animation.SetOrigin(animation.width / 2, animation.height);
        animation.SetXY(0, 0);

        //set the animation to its idle state and add the speedIndicator
        animation.currentFrame = 0;
        animation.SetCycle(0, 5);
    }

    /// <summary>
    /// Initializes the sprite for the player and the cameraTarget
    /// </summary>
    /// <param name="parentScene">The scene this player should callback for change of camera target</param>
    public void initialize(Scene parentScene)
    {
        //controller = new ControllerScript("sprites/empty.png", 1, 1, obj);
        controller = new ControllerScript();
        controller.initialize(parentScene);
        AddChild(controller);

        this.parentScene = parentScene;
        this.SetOrigin(width / 2, height);
        createAnimation();
        this.x += animation.width / 2; //offset for the tiled player position/origin



        

    }

    /// <summary>
    /// Try to read the player variables from the tiled object, use the fallback variables if not present
    /// </summary>
    private void readVariables()
    {

    }

    void Update()
    {
        playerAnimation();
        updateCameraTarget();
        updateUI();

        if (Input.GetKey(Key.A))
            controller.callibrate();
    }

    /// <summary>
    /// checks whether the player is colliding with a CameraTrigger, if this is the case it will set the cameraTarget of the scene to that instead of the player
    /// </summary>
    private void updateCameraTarget()
    {
        GameObject[] collisions = this.GetCollisions(true, false);
        foreach (GameObject collision in collisions)
        {
            if (collision is Objects.CameraTrigger)
            {
                parentScene.setLookTarget(((CameraTrigger)collision).target);
                return;
            }
        }
        parentScene.setLookTarget(this.cameraTarget);

    }

    /// <summary>
    /// moves the player and updates all the visuals accordingly
    /// </summary>
    private void playerAnimation()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        /*
        float rotationDelta = (controller.stickPosition) / 1024f * movementSpeed;
        Console.WriteLine(rotationDelta);
        if (Mathf.Abs(rotationDelta) > stoppedMargin)
            animation.rotation += rotationDelta;
        */
        float rotationDelta = animation.rotation - controller.stickPosition * 360 / 1024;
        animation.rotation -= rotationDelta * Time.deltaTime / 1000f * 10;
        animation.rotation %= 360;
    }

    /// <summary>
    /// Updates the Energy bar at the top of the screen
    /// </summary>
    private void updateUI()
    {
        
    }

    /// <summary>
    /// Checks whether the player is over a ladder for changing the locomotion
    /// </summary>
    /// <returns>Whether the player is currently on a ladder</returns>
    private bool isOverLadder()
    {
        GameObject[] collisions = this.GetCollisions(true, false);
        foreach (GameObject collision in collisions)
        {
            if (collision is Objects.Ladder)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Adds the specified amount of energy to the player and clamps it to the max energy
    /// </summary>
    /// <param name="amount">amount of energy to add</param>
    public void addEnergy(float amount)
    {
        
    }

    /// <summary>
    /// initiates the explosion of the player death
    /// </summary>
    public void die()
    {
        
    }
}