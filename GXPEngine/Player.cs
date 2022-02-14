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

class Player : Pivot
{
    //local variables
    private float rotationSmoothSpeed = 8;
    private float shieldMovementSpeed = 5; //degrees per  second
    private int lives = 3; //amount of lives the player has before death

    //objects
    TiledObject obj;
    private Scene parentScene;
    public AnimationSprite shooterAnimation;
    ControllerScript controller;
    public Shield shield;
    LivesCounter livesCounter;

    //camera target when following the player
    public Pivot cameraTarget;



    public Player() : base() { readVariables(); }

    public Player(TiledObject obj) : base()
    {
        this.obj = obj;
        readVariables();
    }

    public Player(String filename, int cols, int rows, TiledObject obj) : base()
    {
        this.obj = obj;
        readVariables();
    }

    /// <summary>
    /// setup the visual for the player,
    /// visual is seperate for collision reasons
    /// </summary>
    private void createAnimation()
    {
        shooterAnimation = new AnimationSprite("sprites/slingshot1.png", 1, 1, -1, true, true);
        AddChild(shooterAnimation);
        shooterAnimation.SetOrigin(shooterAnimation.width / 2, shooterAnimation.height);//middle bottom
        shooterAnimation.SetXY(0, 0);

        //set the animation to its idle state and add the speedIndicator
        shooterAnimation.currentFrame = 0;
        shooterAnimation.SetCycle(0, 5);
    }

    /// <summary>
    /// Initializes the sprite for the player and the cameraTarget
    /// </summary>
    /// <param name="parentScene">The scene this player should callback for change of camera target</param>
    public void initialize(Scene parentScene)
    {
        //initialize the player input
        controller = new ControllerScript();
        controller.initialize(parentScene);
        AddChild(controller);

        this.parentScene = parentScene;

        //create the players
        createAnimation();
        shield = new Shield("sprites/shieldall.png");
        AddChild(shield);
        shield.SetXY(0, 0);
        shield.Initialize(parentScene);
        shield.rotation = 0;
        shield.length = 5;

        //setup the camera
        this.SetScaleXY(1, 1);
        Pivot lookTarget = new Pivot();
        AddChild(lookTarget);
        lookTarget.SetXY(0, 0);
        lookTarget.SetScaleXY(1f, 1f);
        parentScene.setLookTarget(lookTarget);
        parentScene.jumpToTarget();

        //setup the livesCounter
        livesCounter = new LivesCounter("sprites/heart.png", new Vector2(0,0), new Vector2(300,0), 0.6f, 5);
        parentScene.ui.addElement(livesCounter, "livesCounter", 0, 0);

    }

    /// <summary>
    /// Try to read the player variables from the tiled object, use the fallback variables if not present
    /// </summary>
    private void readVariables()
    {

    }

    void Update()
    {
        shooterPlayerInput();
        defenderPlayerInput();
        playerAnimation();
        updateUI();

        if(livesCounter.currentLives != lives)
        {
            livesCounter.currentLives = lives;
        }

        if (Input.GetKeyDown(Key.PLUS))
        {
            shield.length++;
            lives++;
        }
        if (Input.GetKeyDown(Key.MINUS))
        {
            shield.length--;
            lives--;
        }
    }

    /// <summary>
    /// moves the player and updates all the visuals accordingly
    /// </summary>
    private void playerAnimation()
    {

    }

    private void shooterPlayerInput()
    {
        float rotationDelta = shooterAnimation.rotation - controller.shooterStickPosition;
        
        if(rotationDelta > 180)
            rotationDelta = shooterAnimation.rotation - controller.shooterStickPosition - 360;
        else if (rotationDelta < -180)
            rotationDelta = shooterAnimation.rotation - controller.shooterStickPosition + 360;
        
        shooterAnimation.rotation -= rotationDelta * Time.deltaTime / 1000f * rotationSmoothSpeed;
        shooterAnimation.rotation -= Mathf.Floor(shooterAnimation.rotation / 360) * 360; //keep the rotation between 0 and 360, non negative
        

        //if you want to shoot
        //if(Input.GetKeyDown(Key.SPACE))
        if(controller.shootButtonDown)
        {
            //spawn bullet
            Bullet bullet = new Bullet("sprites/ant.png", 1, 1, 1, 90);
            bullet.SetOrigin(bullet.width / 2, bullet.height);
            parentScene.AddChild(bullet);
            bullet.SetXY(this.x, this.y);
            bullet.rotation = shooterAnimation.rotation;
            bullet.initialize(parentScene);
        }
    }

    private void defenderPlayerInput()
    {
        float rotationDelta = shield.rotation - controller.defenderStickPosition;

        if (rotationDelta > 180)
            rotationDelta = shield.rotation - controller.defenderStickPosition - 360;
        else if (rotationDelta < -180)
            rotationDelta = shield.rotation - controller.defenderStickPosition + 360;

        shield.rotation -= rotationDelta * Time.deltaTime / 1000f * rotationSmoothSpeed;
        shield.rotation -= Mathf.Floor(shield.rotation / 360) * 360; //keep the rotation between 0 and 360, non negative

    }

    /// <summary>
    /// Updates the Energy bar at the top of the screen
    /// </summary>
    private void updateUI()
    {

    }

    /// <summary>
    /// Adds the specified amount of energy to the player and clamps it to the max energy
    /// </summary>
    /// <param name="amount">amount of energy to add</param>
    public void addEnergy(float amount)
    {
        
    }

    public void takeDamage(int damage = 1)
    {
        lives -= damage;
        if (lives <= 0)
            die();
    }

    /// <summary>
    /// loads the main menu
    /// </summary>
    public void die()
    {
        ((MyGame)game).loadNewLevel("maps/Main menu.tmx");
    }
}