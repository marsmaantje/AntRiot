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
    private int lives = 4; //amount of lives the player has before death

    private float defenderShootSpeed = 300;
    public int nextShieldBash = 0;
    public int shieldCooldownPeriod = 5000;

    public int specialCooldown = 8000;
    public int prevSpecialTime = 0;
    private bool specialFiring = false;
    private int specialDuration = 2000;
    private float specialRotationSpeed = 720;

    //objects
    TiledObject obj;
    private Scene parentScene;
    public AnimationSprite shooterAnimation;
    ControllerScript controller;
    public Shield shield;
    LivesCounter livesCounter;
    ScoreDisplay scoreDisplay;
    RechargeIndicator shieldRecharge;
    RechargeIndicator specialRecharge;
    AnimationSprite specialAnimation;
    Random ran = new Random();

    //Sounds
    Sound damage = new Sound("sounds/Damage.wav");
    Sound shoot = new Sound("sounds/SlingShot.wav");
    Sound special = new Sound("sounds/Queen Scream + PowerUp.wav");



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
        shooterAnimation = new AnimationSprite("sprites/slingshot animation spritesheet.png", 1,7, -1, true, false);
        AddChild(shooterAnimation);
        shooterAnimation.SetOrigin(shooterAnimation.width / 2, shooterAnimation.height - 40);//middle bottom
        shooterAnimation.SetXY(0, 0);

        //set the animation to its idle state and add the speedIndicator
        shooterAnimation.currentFrame = 0;
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

        //find the recharge and health UIElements
        RechargeIndicator[] indicators = parentScene.FindObjectsOfType<RechargeIndicator>();
        foreach (RechargeIndicator indicator in indicators)
        {
            Console.WriteLine(indicator.obj.Name);
            if (indicator.obj.Name == "pillBug") 
                shieldRecharge = indicator;
            if (indicator.obj.Name == "special")
                specialRecharge = indicator;
        }

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
        livesCounter = new LivesCounter("sprites/heart.png", new Vector2(10,30), new Vector2(120,0), 0.6f, 5);
        parentScene.ui.addElement(livesCounter, "livesCounter", 0, 0);

        //setup the score Display
        scoreDisplay = new ScoreDisplay(95, 55);
        scoreDisplay.TextAlign(CenterMode.Center, CenterMode.Min);
        scoreDisplay.TextSize(24);
        parentScene.ui.addElement(scoreDisplay, "scoreDisplay", game.width - 100, game.height - 60);
        scoreDisplay.setText(""+Globals.score);

        //setup the specialAnimation
        specialAnimation = new AnimationSprite("sprites/queen.png", 8, 2, addCollider: false);
        AddChild(specialAnimation);
        specialAnimation.SetOrigin(specialAnimation.width / 2, specialAnimation.height);
        specialAnimation.SetXY(0, 20);
    }

    /// <summary>
    /// Try to read the player variables from the tiled object, use the fallback variables if not present
    /// </summary>
    private void readVariables()
    {

    }

    public void Update()
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
        int frame = shooterAnimation.currentFrame;
        if(shooterAnimation.currentFrame != 0)
            shooterAnimation.Animate(Globals.animationFramerate * Time.deltaTime / 1000f);
        if (shooterAnimation.currentFrame == 6 && shooterAnimation.currentFrame != frame)
        {
            fire();
            shoot.Play().Frequency = ran.Next(38000, 48000);
        }
        if (specialFiring)
            specialAnimation.currentFrame = (int)Mathf.Min(specialAnimation.frameCount, Mathf.Max(0,
                Globals.map(Time.time, prevSpecialTime, prevSpecialTime + specialDuration, 0, specialAnimation.frameCount)));
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
        if(controller.shootButtonDown && shooterAnimation.currentFrame == 0)
        {
            shooterAnimation.currentFrame = 1;
        }

        //if you can activate the special
        if((controller.prevDefenderSpecial && controller.prevShootSpecial && (controller.defenderSpecialDown || controller.shootSpecialDown))
            && Time.time > prevSpecialTime + specialCooldown)
        {
            special.Play();
            specialFiring = true;
            prevSpecialTime = Time.time;
        }

        if (specialFiring)
            screenClear();
    }

    private void defenderPlayerInput()
    {
        float rotationDelta = 0;
        if(!shield.isShooting)
            rotationDelta = shield.rotation - controller.defenderStickPosition;

        if (rotationDelta > 180)
            rotationDelta = shield.rotation - controller.defenderStickPosition - 360;
        else if (rotationDelta < -180)
            rotationDelta = shield.rotation - controller.defenderStickPosition + 360;

        shield.rotation -= rotationDelta * Time.deltaTime / 1000f * rotationSmoothSpeed;
        shield.rotation -= Mathf.Floor(shield.rotation / 360) * 360; //keep the rotation between 0 and 360, non negative

        if(controller.defenderButtonDown && !shield.isShooting && Time.time > nextShieldBash)
        {
            shield.fire(defenderShootSpeed);
            nextShieldBash = Time.time + shieldCooldownPeriod;
        }

    }

    /// <summary>
    /// Clears the screen by shooting many bullets
    /// </summary>
    void screenClear()
    {
        if (prevSpecialTime + specialDuration < Time.time)
            specialFiring = false;
        float bulletRotation = Time.time / 1000f * specialRotationSpeed % 360;
        fire(bulletRotation);
        fire((bulletRotation + 180) % 360);
    }

    /// <summary>
    /// Fires a single bullet
    /// </summary>
    /// <param name="rotation">direction to fire the bullet in, gets the current direction of the player if null</param>
    void fire(float rotation = -1)
    {
            //spawn bullet
            Bullet bullet = new Bullet("sprites/ant.png", 1, 1, 1, 150);
            bullet.SetOrigin(bullet.width / 2, bullet.height);
            parentScene.AddChild(bullet);
            bullet.SetXY(this.x, this.y);
            bullet.rotation = rotation < 0 ? shooterAnimation.rotation : rotation;
            bullet.initialize(parentScene);
            bullet.Move(0, 30);
    }

    /// <summary>
    /// Updates the Energy bar at the top of the screen
    /// </summary>
    private void updateUI()
    {
        scoreDisplay.setText("" + Globals.score);
        if (shieldRecharge != null)
            shieldRecharge.progress = (Mathf.Min(1, Mathf.Max(0, Globals.map(Time.time, nextShieldBash - shieldCooldownPeriod, nextShieldBash, 0, 1))));
        if (specialRecharge != null)
            specialRecharge.progress = (Mathf.Min(1, Mathf.Max(0, Globals.map(Time.time, prevSpecialTime, prevSpecialTime + specialCooldown, 0, 1))));
    }

    public void takeDamage(int damage = 1)
    {
        this.damage.Play().Frequency = ran.Next(38000, 48000);
        lives -= damage;
        if (lives <= 0)
            die();
    }

    /// <summary>
    /// loads the main menu
    /// </summary>
    public void die()
    {
        Globals.highScore = Math.Max(Globals.score, Globals.highScore);
        Globals.score = 0;
        ((MyGame)game).loadNewLevel("maps/Main menu.tmx");
    }
}