using GDLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

#region TODO - Week 8.2
/*
 * To Do
 * ---------------
 * Bug in Pip
 * Add culling of objects outside the camera viewport
 * Discuss component draw order
 * Discuss mouse bounds
 * Discuss how to draw to menu and quiz to screen
 * Explain CameraManager::IEnumerator
 * 
 * Done
 * ---------------
 * Discuss new methods in IActor and IController
 * Add pause to the ObjectManager by catching pause/play event from eventdispatcher
 * Discuss PausableDrawableGameComponent and PausableGameComponent
 * Discuss Predicates in C#
 * Discuss new 2D components
 * Add sorting of cameras by draw depth in CameraManager
 * Finish adding cameras for PiP layout
 * Added JigLibX.dll as a reference in preparation for CDCR
 * Added drawDepth to Camera3D
 * Added support for multi-mesh models (i.e. bug which causes meshes to collapse onto the origin)
 * Added MouseManager::Bounds to enable CD/CR for menu items
 * Added SoundManager demo to Main::DemoSoundManager()
 * Added Main::DemoSetControllerPlayStatus()
 * Added Find and FindAll to ObjectManager
 * Added SetAllControllersPlayStatus() and SetControllerPlayStatus() to IActor
 * Added SetPlayStatus() and GetControllerType() to IController
 * Discuss events and delegates
 * Add support for events using EventDispatcher
 * Discuss RasterizerState and AlphaBlend 
 * Add support for transparent and opaque objects
 * Discuss SamplerState and DepthStencilState
 * Added two new classes to support pausable components - PausableDrawableGameComponent and PausableGameComponent
 * Add ThirdPersonCameraController
 * Added InputManagerParameters to ThirdPersonCameraController to enable reading keyboard
 * Scaled translation and rotation changes in ThirdPersonCameraController by gameTime
 * Fixed backward camera on ThirdPersonCameraController
 * Add ThirdPersonCameraController and demo
 * Added AppData scalars related to ThirdPersonCameraController speed and lerp multipliers 
 */

#endregion

#region TODO - Week 7.2
/*
 * To Do
 * ---------------
 * Discuss events and delegates
 * Discuss IEqualityComparer
 * Add support for events using EventDispatcher
 * Discuss PausableDrawableGameComponent and PausableGameComponent
 * Discuss Predicates in C#
 * Discuss RasterizerState and AlphaBlend 
 * Add support for transparent and opaque objects
 * Discuss SamplerState and DepthStencilState
 * Explain CameraManager::IEnumerator
 * 
 * Done
 * ---------------
 * Added two new classes to support pausable components - PausableDrawableGameComponent and PausableGameComponent
 * Add ThirdPersonCameraController
 * Added InputManagerParameters to ThirdPersonCameraController to enable reading keyboard
 * Scaled translation and rotation changes in ThirdPersonCameraController by gameTime
 * Fixed backward camera on ThirdPersonCameraController
 * Add ThirdPersonCameraController and demo
 * Added AppData scalars related to ThirdPersonCameraController speed and lerp multipliers 
 */

#endregion

#region TODO - Week 6
/*
 * To Do
 * ---------------
 * Add ThirdPersonCameraController
 * Discuss Predicates in C#
 * Discuss RasterizerState and AlphaBlend 
 * Add support for transparent and opaque objects
 * Discuss SamplerState and DepthStencilState
 * Explain CameraManager::IEnumerator
 * 
 * Done
 * --------------
 * Added demo with multiple camera layouts
 * Added Track3DController
 * Added pre-defined screen resolutions in ScreenUtility
 * Add RailCameraController
 * Add ObjectManager3D::Remove(Predicate<Camera3D>)
 * Renamed Object3DManager to ObjectManager
 * Added ScreenUtility
 * Finish DebugDrawer (i.e. FPS counter)
 * Added multi-camera demo and ScreenLayoutType enum
 * Add support for multiple camera layouts
 * Implement multi-camera viewport to support split-screen
 * Optimise commonly invoked methods
 * Discuss SpriteBatch
 * Add use of ContentDictionary to load texture and model assets
 * Call Dispose() on dicionaries in Main::UnloadContent()
 * Added Camera3D::viewport
 * Added ContentDictionary<K, V>
 * Added StringUtility::ParseNameFromPath(string path)
 * Added MathUtility::GetDistanceFromCamera(Actor3D actor, Camera3D activeCamera)
 */
#endregion

#region TODO - Week 5
/*
 * To Do
 * --------------
 * SecurityCameraController, RailCameraController
 * Viewports, CameraManager, multi-camera
 * Discuss Predicates in C#
 * Add ObjectManager3D::RemoveBy(Predicate<Camera3D>)
 * Discuss RasterizerState, SamplerState, AlphaBlend and DepthStencilState
 * Optimise commonly invoked methods
 * 
 * Done
 * ---------------
 * Add Curve classes
 * Finish Camera3D::ToString()
 */
#endregion

#region TODO - Week 4
/*
 * Done
 * ---------------
 * Add Unit tests
 * Test Clone()
 * Finish SkyBox
 * Add Transform3D methods RotateBy() and TranslateBy()
 * Add FlightCamera: Camera3D
 * Refactor ModelObject3D - rename, consider flexibility of current constructor parameters
 * Add MathUtility::Round(Vector3, int)
 * Set graphics resolution - does this affect Camera3D?
 * Add input from keyboard and mouse - GetViewPort(), GetDeltaFromCentre()
 */
#endregion

#region TODO - Week 3
/*
 * To Do
 * --------------
 * Add Unit tests
 * Test Clone()
 * Finish Camera3D::ToString()
 */
#endregion

namespace GDApp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.Game
    {
        #region Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private BasicEffect modelEffect;
        private ObjectManager object3DManager;
        private KeyboardManager keyboardManager;
        private MouseManager mouseManager;
        private BasicEffect skyBoxEffect;
        private Integer2 resolution;
        private Integer2 screenCentre;
        private InputManagerParameters inputManagerParameters;
        private CameraManager cameraManager;
        private ContentDictionary<Model> modelDictionary;
        private ContentDictionary<Texture2D> textureDictionary;
        private ContentDictionary<SpriteFont> fontDictionary;
        private Dictionary<string, RailParameters> railDictionary;
        private Dictionary<string, Track3D> track3DDictionary;
        private ModelObject drivableModelObject;
        private EventDispatcher eventDispatcher;
        private SoundManager soundManager;
        private MenuManager menuManager;
        private bool coat;
        #endregion

        #region Constructors
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion

        #region Initialization
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //add the component to handle all system events
            this.eventDispatcher = new EventDispatcher(this, 20);
            Components.Add(this.eventDispatcher);

            this.resolution = ScreenUtility.XVGA;
            this.screenCentre = this.resolution / 2;

            InitializeGraphics(); 
            InitializeEffects();
            InitializeDictionaries();

            LoadAssets();

            //Keyboard
            this.keyboardManager = new KeyboardManager(this);
            Components.Add(this.keyboardManager);

            //Mouse
            bool bMouseVisible = true;
            this.mouseManager = new MouseManager(this, bMouseVisible);
            this.mouseManager.SetPosition(this.screenCentre);
            Components.Add(this.mouseManager);

            //bundle together for easy passing
            this.inputManagerParameters = new InputManagerParameters(this.mouseManager, this.keyboardManager);

            //this is a list that updates all cameras
            this.cameraManager = new CameraManager(this, 3);
            Components.Add(this.cameraManager);

            //Object3D
            this.object3DManager = new ObjectManager(this, this.cameraManager, this.eventDispatcher, StatusType.Drawn | StatusType.Update);
            Components.Add(this.object3DManager);

            //Sound
            this.soundManager = new SoundManager(this, this.eventDispatcher, StatusType.Update, "Content/Assets/Audio/", "Demo2DSound.xgs", "WaveBank1.xwb", "SoundBank1.xsb");
            Components.Add(this.soundManager);

            //Menu
            this.menuManager = new MenuManager(this, this.inputManagerParameters,
                this.cameraManager, this.spriteBatch, this.eventDispatcher,
                StatusType.Off);
            Components.Add(this.menuManager);


            int scale = 1250;
            InitializeGround(scale); 
            InitializeSkyBox(scale); 
            InitializeTorusDecorator();
            InitializeSemiTransparentDecorators();
            InitializeDrivableObject();
            InitialiseDecoratorClones();

            InitializeCameras(ScreenLayoutType.Pip);

            InitializeMainMenu();
            InitializeOptionMenu();
            this.menuManager.SetActiveMenu(AppData.MenuMainID);
            //since debug needs sprite batch then call here
            InitializeDebug(true);

            base.Initialize();
        }

        private void InitializeOptionMenu()
        {
            Transform2D transform = null;
            UITextureObject textureObject = null;

             transform = new Transform2D(1.5f*Vector2.One);

            textureObject = new UITextureObject("op1", ActorType.UITexture,
                StatusType.Drawn | StatusType.Update, transform,
                Color.White, SpriteEffects.None, 1,
                this.textureDictionary["ice"]);

            this.menuManager.Add("options", textureObject);

            transform = new Transform2D(new Vector2(200, 300), 0, new Vector2(1, 0.5f), Vector2.Zero, new Integer2(600, 160));

            textureObject = new UITextureObject("slider1", ActorType.UITexture,
                StatusType.Drawn | StatusType.Update, transform,
                Color.White, SpriteEffects.None, 0.5f,
                this.textureDictionary["sliderBar"]);

            this.menuManager.Add("options", textureObject);

            transform = new Transform2D(new Vector2(300, 290), 0, new Vector2(1,0.7f), Vector2.Zero, new Integer2(180, 150));

            textureObject = new UITextureObject("tracker1", ActorType.UITexture,
                StatusType.Drawn | StatusType.Update, transform,
                Color.White, SpriteEffects.None, 0,
                this.textureDictionary["sliderTracker"]);

            this.menuManager.Add("options", textureObject);



        }

        private void InitializeMainMenu()
        {
            Transform2D transform = null;
            UITextureObject menuObject = null;
            Vector2 midPoint = Vector2.Zero;

            midPoint = new Vector2(this.textureDictionary["menuButton"].Width / 2.0f,
                this.textureDictionary["menuButton"].Height / 2.0f);

            transform = new Transform2D(new Vector2(500, 300),
              0,  0.5f * Vector2.One, midPoint, new Integer2(1000, 367));

            menuObject = new UITextureObject("menu1", ActorType.UITexture,
                StatusType.Drawn | StatusType.Update, transform,
                Color.White, SpriteEffects.None, 0,
                this.textureDictionary["menuButton"]);

            menuObject.AttachController(
                new MouseButtonController("mbc1", ControllerType.UIMouse,this.mouseManager));

            //add to manager - menu manager
            this.menuManager.Add(AppData.MenuMainID, menuObject);

            midPoint = new Vector2(this.textureDictionary["exitButton"].Width / 2.0f,
                this.textureDictionary["exitButton"].Height / 2.0f);

            transform = new Transform2D(new Vector2(500, 500),
              0, 0.5f * Vector2.One, midPoint, new Integer2(1000,367));

            menuObject = new UITextureObject("exit1", ActorType.UITexture,
                StatusType.Drawn | StatusType.Update, transform,
                Color.White, SpriteEffects.None, 0,
                this.textureDictionary["exitButton"]);

            menuObject.AttachController(
                new MouseButtonController("mbc1", ControllerType.UIMouse, this.mouseManager));

            //add to manager - menu manager
            this.menuManager.Add(AppData.MenuMainID, menuObject);


            //set active menu
            // this.menuManager.SetActiveMenu(AppData.MenuMainID);
        }

        private void InitializeDictionaries()
        {
            //textures, models, fonts
            this.modelDictionary = new ContentDictionary<Model>("model dictionary", this.Content);
            this.textureDictionary = new ContentDictionary<Texture2D>("texture dictionary", this.Content);
            this.fontDictionary = new ContentDictionary<SpriteFont>("font dictionary", this.Content);

            //rail, transform3Dcurve               
            this.railDictionary = new Dictionary<string, RailParameters>();
            this.track3DDictionary = new Dictionary<string, Track3D>();

            //sound

        }

        private void InitializeDebug(bool v)
        {
            Components.Add(new DebugDrawer(this, this.cameraManager, this.spriteBatch,
                this.fontDictionary["debugFont"], new Vector2(20, 20),
                36.1f, 0.3f,
                Color.White, this.eventDispatcher,
                StatusType.Drawn | StatusType.Update));

            //add this.coat (bool type) for toggling the coat on/off
            this.coat = false;
        }

        #endregion

        #region Assets
        private void LoadAssets()
        {
            LoadTextures();
            LoadModels();
            LoadFonts();
            LoadSounds();

            LoadRails();
            LoadTracks();
        }

        private void LoadSounds()
        {
            //TODO...
        }

        private void LoadFonts()
        {
            this.fontDictionary.Load("hudFont", "Assets/Fonts/hudFont");
            this.fontDictionary.Load("debugFont", "Assets/Debug/Fonts/debugFont");
        }

        private void LoadModels()
        {
            this.modelDictionary.Load("box", "Assets/Models/box");
            this.modelDictionary.Load("fart", "Assets/Models/torus");
            this.modelDictionary.Load("plane1", "Assets/Models/plane1");
        }

        private void LoadTextures()
        {
            this.textureDictionary.Load("grass1",
                "Assets/Textures/Foliage/Ground/grass1");
            this.textureDictionary.Load("ice",
                "Assets/Textures/Foliage/Ground/iceSheet");

            this.textureDictionary.Load("Assets/Textures/Skybox/front");
            this.textureDictionary.Load("Assets/Textures/Skybox/back");
            this.textureDictionary.Load("Assets/Textures/Skybox/left");
            this.textureDictionary.Load("Assets/Textures/Skybox/right");
            this.textureDictionary.Load("sky",
                                        "Assets/Textures/Skybox/sky");

            this.textureDictionary.Load(
                    "Assets/Textures/Props/Crates/crate1");
            this.textureDictionary.Load(
                    "Assets/Textures/Props/Crates/crate2");


            this.textureDictionary.Load("Assets/Textures/Menu/exitButton");
            this.textureDictionary.Load("Assets/Textures/Menu/menuButton");
            this.textureDictionary.Load("Assets/Textures/Menu/sliderBar");
            this.textureDictionary.Load("Assets/Textures/Menu/sliderTracker");

            //debug
            this.textureDictionary.Load("Assets/Debug/Textures/checkerboard");

            this.textureDictionary.Load("Assets/Textures/UI/Menu/Backgrounds/controlsmenu");
        }
 
        private void LoadRails()
        {
            RailParameters railParameters = null;

            //create a simple rail that gains height as the target moves on +ve X-axis - try different rail vectors
            railParameters = new RailParameters("battlefield 1", new Vector3(0, 10, 50), new Vector3(50, 50, 50));
            this.railDictionary.Add(railParameters.ID, railParameters);

            //add more rails here...
            railParameters = new RailParameters("battlefield 2", new Vector3(-50, 20, 20), new Vector3(50, 80, 100));
            this.railDictionary.Add(railParameters.ID, railParameters);
        }

        private void LoadTracks()
        {
            Track3D track3D = null;

            //starts away from origin, moves forward and rises, then ends closer to origin and looking down from a height
            track3D = new Track3D(CurveLoopType.Oscillate);
            track3D.Add(new Vector3(0, 10, 200), -Vector3.UnitZ, Vector3.UnitY, 0);
            track3D.Add(new Vector3(0, 20, 150), -Vector3.UnitZ, Vector3.UnitY, 2);
            track3D.Add(new Vector3(0, 40, 100), -Vector3.UnitZ, Vector3.UnitY, 4);

            //set so that the camera looks down at the origin at the end of the curve
            Vector3 finalPosition = new Vector3(0, 80, 50);
            Vector3 finalLook = Vector3.Normalize(Vector3.Zero - finalPosition);

            track3D.Add(finalPosition, finalLook, Vector3.UnitY, 6);
            this.track3DDictionary.Add("push forward 1", track3D);

            //add more transform3D curves here...
        }

        #endregion

        #region Graphics & Effects
        private void InitializeEffects()
        {
            #region 3D Filled Objects
            this.skyBoxEffect = new BasicEffect(graphics.GraphicsDevice);
            this.skyBoxEffect.TextureEnabled = true;


            this.modelEffect = new BasicEffect(graphics.GraphicsDevice);
            this.modelEffect.EnableDefaultLighting();
            this.modelEffect.PreferPerPixelLighting = true;
            this.modelEffect.TextureEnabled = true;

            #endregion
        }

        private void InitializeGraphics()
        {
            this.graphics.PreferredBackBufferWidth = resolution.X;
            this.graphics.PreferredBackBufferHeight = resolution.Y;

            //solves the skybox border problem
            SamplerState samplerState = new SamplerState();
            samplerState.AddressU = TextureAddressMode.Clamp;
            samplerState.AddressV = TextureAddressMode.Clamp;
            this.graphics.GraphicsDevice.SamplerStates[0] = samplerState;

            //enable alpha transparency - see ColorParameters
            this.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            this.graphics.ApplyChanges();
        }
        #endregion

        #region Cameras
        private void InitializeCameras(ScreenLayoutType screenLayoutType)
        {
            Viewport viewport = new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            float aspectRatio = (float)this.resolution.X / this.resolution.Y;
            ProjectionParameters projectionParameters = new ProjectionParameters(MathHelper.PiOver4, aspectRatio, 1, 2500);

            if (screenLayoutType == ScreenLayoutType.FirstPerson)
            {
                AddFirstPersonCamera(viewport, projectionParameters);
            }
            else if (screenLayoutType == ScreenLayoutType.ThirdPerson)
            {
                AddThirdPersonCamera(viewport, projectionParameters);
            }
            else if (screenLayoutType == ScreenLayoutType.Flight)
            {
                AddFlightCamera(viewport, projectionParameters);
            }
            else if (screenLayoutType == ScreenLayoutType.Rail)
            {
                AddRailCamera(viewport, projectionParameters);
            }
            else if (screenLayoutType == ScreenLayoutType.Track)
            {
                AddTrack3DCamera(viewport, projectionParameters);
            }
            else if (screenLayoutType == ScreenLayoutType.Pip)
            {
                AddMainAndPipCamera(viewport, projectionParameters);
            }

            else if (screenLayoutType == ScreenLayoutType.Multi1x4) //splits the screen vertically x4
            {
                viewport = new Viewport(0, 0, (int)(graphics.PreferredBackBufferWidth/4.0f), graphics.PreferredBackBufferHeight);
                AddFirstPersonCamera(viewport, projectionParameters);

                viewport.X += viewport.Width; //move the next camera over to start at x = 1/4 screen width
                AddRailCamera(viewport, projectionParameters);

                viewport.X += viewport.Width; //move the next camera over to start at x = 2/4 screen width
                AddTrack3DCamera(viewport, projectionParameters);

                viewport.X += viewport.Width; //move the next camera over to start at x = 3/4 screen width
                AddSecurityCamera(viewport, projectionParameters);
            }
            else if (screenLayoutType == ScreenLayoutType.Multi2x2) //splits the screen in 4 equal parts
            {
                //top left
                viewport = new Viewport(0, 0, (int)(graphics.PreferredBackBufferWidth / 2.0f), (int)(graphics.PreferredBackBufferHeight / 2.0f));
                AddFirstPersonCamera(viewport, projectionParameters);

                //top right
                viewport.X = viewport.Width; 
                AddRailCamera(viewport, projectionParameters);

                ////bottom left
                viewport.X = 0;
                viewport.Y = viewport.Height;
                AddTrack3DCamera(viewport, projectionParameters);

                ////bottom right
                viewport.X = viewport.Width;
                viewport.Y = viewport.Height;
                AddSecurityCamera(viewport, projectionParameters);
            }
            else //in all other cases just add a security camera - saves us having to implement all enum options at the moment
            {
                AddSecurityCamera(viewport, projectionParameters);
            }
        }

        private void AddMainAndPipCamera(Viewport viewport, ProjectionParameters projectionParameters)
        {
            Camera3D camera3D = null;
            Transform3D transform = null;

            //security camera
            transform = new Transform3D(new Vector3(0, 40, 0),
                Vector3.Zero, Vector3.One, -Vector3.UnitY, Vector3.UnitZ);

            int width = 240;
            int height = 180;
            int xPos = this.resolution.X - width - 10;
            Viewport pipViewport = new Viewport(xPos, 10, width, height);

            camera3D = new Camera3D("sc1", ActorType.Camera,
                transform, projectionParameters, pipViewport, 0.8f);

            camera3D.AttachController(new SecurityCameraController("scc1", ControllerType.Security, 15, 2, Vector3.UnitX));

            this.cameraManager.Add(camera3D);

            //1st person
            transform = new Transform3D(
                 new Vector3(0, 10, 100), Vector3.Zero,
                 Vector3.One, -Vector3.UnitZ, Vector3.UnitY);

            camera3D = new Camera3D("fpc1", ActorType.Camera,
                transform, projectionParameters, viewport, 0.4f);

            camera3D.AttachController(new FirstPersonCameraController(
              "fpcc1", ControllerType.FirstPerson,
              AppData.CameraMoveKeys, AppData.CameraMoveSpeed,
              AppData.CameraStrafeSpeed, AppData.CameraRotationSpeed, this.inputManagerParameters, this.screenCentre));




            //put controller later!
            this.cameraManager.Add(camera3D);
        }

        private void AddTrack3DCamera(Viewport viewport, ProjectionParameters projectionParameters)
        {
            //doesnt matter where the camera starts because we reset immediately inside the Transform3DCurveController
            Transform3D transform = Transform3D.Zero; 

            Camera3D camera3D = new Camera3D("curve camera 1", ActorType.Camera, transform, projectionParameters, viewport);

            camera3D.AttachController(new Track3DController("tcc1", ControllerType.Track,
                this.track3DDictionary["push forward 1"], PlayStatusType.Play));

            this.cameraManager.Add(camera3D);
        }

        private void AddRailCamera(Viewport viewport, ProjectionParameters projectionParameters)
        {
            //doesnt matter where the camera starts because we reset immediately inside the RailController
            Transform3D transform = Transform3D.Zero;

            Camera3D camera3D = new Camera3D("rail camera 1", ActorType.Camera, transform, projectionParameters, viewport);

            camera3D.AttachController(new RailController("rc1", ControllerType.Rail, 
                this.drivableModelObject, this.railDictionary["battlefield 1"]));

            this.cameraManager.Add(camera3D);

        }

        private void AddThirdPersonCamera(Viewport viewport, ProjectionParameters projectionParameters)
        {
            Transform3D transform = Transform3D.Zero;

            Camera3D camera3D = new Camera3D("third person camera 1", ActorType.Camera, 
                transform, projectionParameters, viewport);

            camera3D.AttachController(new ThirdPersonController("tpcc1", ControllerType.ThirdPerson,
                this.drivableModelObject, AppData.CameraThirdPersonDistance,
                AppData.CameraThirdPersonScrollSpeedDistanceMultiplier,
                AppData.CameraThirdPersonElevationAngleInDegrees,
                AppData.CameraThirdPersonScrollSpeedElevationMultiplier,
                LerpSpeed.Slow, LerpSpeed.VerySlow, this.inputManagerParameters));

            this.cameraManager.Add(camera3D);

        }

        private void AddSecurityCamera(Viewport viewport, ProjectionParameters projectionParameters)
        {
            Transform3D transform = new Transform3D(new Vector3(50, 10, 10), Vector3.Zero, Vector3.Zero, -Vector3.UnitX, Vector3.UnitY);

            Camera3D camera3D = new Camera3D("security camera 1", ActorType.Camera, transform, projectionParameters, viewport);

            camera3D.AttachController(new SecurityCameraController("scc1", ControllerType.Security, 15, 2, Vector3.UnitX));

            this.cameraManager.Add(camera3D);

        }

        private void AddFlightCamera(Viewport viewport, ProjectionParameters projectionParameters)
        {
            Transform3D transform = new Transform3D(new Vector3(0, 10, 30), Vector3.Zero, Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);

            Camera3D camera3D = new Camera3D("flight camera 1", ActorType.Camera, transform, projectionParameters, viewport);

            camera3D.AttachController(new FlightCameraController("flight camera controller 1", 
                ControllerType.Flight, AppData.CameraMoveKeys_Alt1, AppData.CameraMoveSpeed,
                AppData.CameraStrafeSpeed, AppData.CameraRotationSpeed, this.inputManagerParameters, this.screenCentre));

            this.cameraManager.Add(camera3D);
        }

        private void AddFirstPersonCamera(Viewport viewport, ProjectionParameters projectionParameters)
        {
            Transform3D transform = new Transform3D(new Vector3(0, 10, 80), Vector3.Zero, Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);

            Camera3D camera3D = new Camera3D("first person camera 1", ActorType.Camera, transform,
                projectionParameters, viewport);

            camera3D.AttachController(new FirstPersonCameraController(
                "fpcc1", ControllerType.FirstPerson,
                AppData.CameraMoveKeys, AppData.CameraMoveSpeed, 
                AppData.CameraStrafeSpeed, AppData.CameraRotationSpeed, this.inputManagerParameters, this.screenCentre));

            this.cameraManager.Add(camera3D);

        }
        #endregion

        #region Drawn Objects
        private void InitializeSkyBox(int scale)
        {
            Transform3D transform = null;
            ModelObject clone = null;
            ModelObject archetype = null;

            #region Plane Archetype
            transform = new Transform3D(Vector3.Zero,
                Vector3.Zero, scale * Vector3.One, Vector3.UnitZ, Vector3.UnitY);

            ColorParameters colorParameters = new ColorParameters(
                   this.textureDictionary["checkerboard"]);

            archetype = new ModelObject("arch plane",
                ActorType.Decorator, transform,
               this.modelDictionary["plane1"],
                colorParameters, this.skyBoxEffect);
            #endregion

            #region Back
            clone = archetype.Clone() as ModelObject;
            clone.Transform.Translation = new Vector3(0, 0, -scale / 2.0f);
            clone.ColorParameters = new ColorParameters(
                this.textureDictionary["back"]);
            this.object3DManager.Add(clone);
            #endregion

            #region Left
            clone = archetype.Clone() as ModelObject;
            clone.Transform.Translation = new Vector3(-scale / 2.0f, 0, 0);
            clone.Transform.Rotation = new Vector3(0, 90, 0);
            clone.ColorParameters = new ColorParameters(
                  this.textureDictionary["left"]);
            this.object3DManager.Add(clone);
            #endregion

            #region Right
            clone = archetype.Clone() as ModelObject;
            clone.Transform.Translation = new Vector3(scale / 2.0f, 0, 0);
            clone.Transform.Rotation = new Vector3(0, -90, 0);
            clone.ColorParameters = new ColorParameters(
                 this.textureDictionary["right"]);
            this.object3DManager.Add(clone);
            #endregion

            #region Top
            clone = archetype.Clone() as ModelObject;
            clone.Transform.Translation = new Vector3(0, scale / 2.0f, 0);
            clone.Transform.Rotation = new Vector3(90, 270, 0);
            clone.ColorParameters = new ColorParameters(
                  this.textureDictionary["sky"]);
            this.object3DManager.Add(clone);
            #endregion

            #region Front
            clone = archetype.Clone() as ModelObject;
            clone.Transform.Translation = new Vector3(0, 0, scale / 2.0f);
            clone.Transform.Rotation = new Vector3(0, 180, 0);
            clone.ColorParameters = new ColorParameters(
                  this.textureDictionary["front"]);
            this.object3DManager.Add(clone);
            #endregion
        }

        private void InitializeGround(int scale)
        {
            Transform3D transform = null;

            #region Grass
            transform = new Transform3D(
                Vector3.Zero,
                -90 * Vector3.UnitX,
                scale * Vector3.One,
                Vector3.UnitX, Vector3.UnitY);

            this.object3DManager.Add(new ModelObject("ground",
               ActorType.Decorator, transform,
               this.modelDictionary["plane1"],
               new ColorParameters(
                   this.textureDictionary["grass1"]),
               this.skyBoxEffect));
            #endregion
        }

        private void InitializeTorusDecorator()
        {
            Transform3D transform = null;
            ModelObject modelObject = null;

            #region Torus
            transform = new Transform3D(
                new Vector3(0, 10, 0),
                Vector3.Zero, new Vector3(0.1f, 0.4f, 0.05f),
                Vector3.UnitX, Vector3.UnitY);

            modelObject = new ModelObject("torus 1",
                ActorType.Decorator, transform,
                this.modelDictionary["fart"],
                new ColorParameters(Color.Yellow,
                this.textureDictionary["crate1"], 1f),
                this.modelEffect);

            //attach a rotation controller to see torus spin
            modelObject.AttachController(new RotationController("torus rotation controller 1",
                ControllerType.Rotation,
                new Vector3(0, 20 / 60.0f, 0))); //rotate around +ve Y-axis 0.016 degrees each update or 1 degree/second
            this.object3DManager.Add(modelObject);
            #endregion
        }

        private void InitializeSemiTransparentDecorators()
        {
            Transform3D transform = null;
            ModelObject modelObject = null;

            #region Box 1
            transform = new Transform3D(
             new Vector3(10, 5, 0),
             new Vector3(0, 0, 0),
             new Vector3(4, 16, 2),
             Vector3.UnitX, Vector3.UnitY);


            modelObject = new ModelObject("box 1",
                ActorType.Decorator, transform,
                this.modelDictionary["box"],
                new ColorParameters(Color.PaleVioletRed,
                    this.textureDictionary["crate1"], 0.7f),
                this.modelEffect);

            modelObject.AttachController(new TranslationSineLerpController(
                "tsl1", ControllerType.SineTranslation,
                new Vector3(0, 1, 0), new TrigonometricParameters(10, 0.1f, 0)));
            this.object3DManager.Add(modelObject);
            #endregion

            #region Box 2
            transform = new Transform3D(
             new Vector3(30, 5, 0),
             new Vector3(0, 0, 0),
             new Vector3(4, 16, 8),
             Vector3.UnitX, Vector3.UnitY);

            modelObject = new ModelObject("box 2",
                ActorType.Decorator, transform,
                 this.modelDictionary["box"],
                new ColorParameters(Color.LightBlue,
                    this.textureDictionary["crate1"], 0.3f),
                this.modelEffect);

            this.object3DManager.Add(modelObject);
            #endregion
        }

        private void InitializeDrivableObject()
        {
            Transform3D transform = null;

            transform = new Transform3D(
             new Vector3(-20, 8, 0),
             new Vector3(0, 0, 0),
             new Vector3(4, 16, 4),
             Vector3.UnitX, Vector3.UnitY);

            //add as a field so we can set a camera on a rail to target this car
            this.drivableModelObject = new ModelObject("car 1",
                ActorType.Decorator, transform,
                 this.modelDictionary["box"],
                new ColorParameters(Color.LightGreen,
                    this.textureDictionary["crate1"], 1),
                this.modelEffect);


            this.drivableModelObject.AttachController(new DriveController("dc1", ControllerType.Drive,
                AppData.ObjectMoveKeys, 
                AppData.CarMoveSpeed, AppData.CarStrafeSpeed, AppData.CarRotationSpeed,
                this.inputManagerParameters));

            this.object3DManager.Add(this.drivableModelObject);
        }

        private void InitialiseDecoratorClones()
        {
            //TODO...
        }
        #endregion

        #region Load/Unload, Draw, Update
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //// Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            ////since debug needs sprite batch then call here
            //InitializeDebug(true);
        }
 
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            this.modelDictionary.Dispose();
            this.fontDictionary.Dispose();
            this.textureDictionary.Dispose();

            //only C# dictionary so no Dispose() method to call
            this.railDictionary.Clear();
            this.track3DDictionary.Clear();

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if(MouseButtonController.exit)
            {
                Exit();
            }

            DemoSetControllerPlayStatus();

            DemoCoat(); // add DemoCoat() method

            DemoSoundManager();

            DemoToggleMenu();

            DemoSlippingIce();

            DemoMenuChange();

           base.Update(gameTime);
        }

        private void DemoCoat()
        {
            // if the Enter key is pressed and the coat is not on
            if (this.keyboardManager.IsFirstKeyPress(Keys.Enter) && !coat)
            {   //publish an EventData with EventActionType.OnCoat and EventCategory.Coat
                EventDispatcher.Publish(new EventData(EventActionType.OnCoat, EventCategoryType.Coat));
                coat = true;// set the coat to true
            }
            else if (this.keyboardManager.IsFirstKeyPress(Keys.Enter) && coat)
            {	//publish an EventData with EventActionType.OffCoat and EventCategory.Coat
                EventDispatcher.Publish(new EventData(EventActionType.OffCoat, EventCategoryType.Coat));
                coat = false; //  set the coat to false
            }
        }

        private void DemoMenuChange()
        {
            if(this.keyboardManager.IsFirstKeyPress(Keys.F7))
            {
                EventDispatcher.Publish(new EventData(AppData.MenuMainID, null, EventActionType.OnNewMenu, EventCategoryType.Menu));
            }
            else if (this.keyboardManager.IsFirstKeyPress(Keys.F8))
            {
                EventDispatcher.Publish(new EventData("options", null, EventActionType.OnNewMenu, EventCategoryType.Menu));
            }
        }

        public void DemoSlippingIce()
        {
            if (this.keyboardManager.IsFirstKeyPress(Keys.I))
                EventDispatcher.Publish(new EventData(EventActionType.EnterIce, EventCategoryType.Ice));
        }

        private void DemoToggleMenu()
        {
            if (this.keyboardManager.IsFirstKeyPress(Keys.Escape))
                EventDispatcher.Publish(new EventData(EventActionType.OnToggle, EventCategoryType.Menu));
        }

        private void DemoSoundManager()
        {
            if (this.keyboardManager.IsFirstKeyPress(Keys.B))
            {
                this.soundManager.PlayCue("boing");
            }
        }

        private void DemoSetControllerPlayStatus()
        {
            DrawnActor3D torusActor = this.object3DManager.Find(actor => actor.ID.Equals("torus 1"));
            if (torusActor != null && this.keyboardManager.IsFirstKeyPress(Keys.O))
            {
                torusActor.SetControllerPlayStatus(PlayStatusType.Pause, controller => controller.GetControllerType() == ControllerType.Rotation);
            }
            else if (torusActor != null && this.keyboardManager.IsFirstKeyPress(Keys.P))
            {
                torusActor.SetControllerPlayStatus(PlayStatusType.Play, controller => controller.GetControllerType() == ControllerType.Rotation);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
        #endregion
    }
}

