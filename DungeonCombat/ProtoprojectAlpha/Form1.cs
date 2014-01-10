using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DungeonCombat
{
    public partial class MainForm : Form
    {
        bool canvasLoaded = false;
        Stopwatch stopWatch = null;
        double elapsedTime = 0;
        Camera camera = null;
        CombatManager combatManager = null;
        Location mouseLocation = null;

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void canvas_Load(object sender, EventArgs e)
        {
            canvasLoaded = true;
            
            stopWatch = new Stopwatch();

            WeaponManager.Initialize();
            TileRenderer.Initialize();
            AudioManager.Initialize();

            WeaponManager.LoadWeaponsFromFile(@"weaponList.dat");
            camera = new Camera();
            combatManager = new CombatManager(camera);

            combatManager.CreateCombatant("Zabuza", "ninja");
            combatManager.CreateCombatant("Dudik", "warrior");
            combatManager.CreateCombatant("Elf Lord", "elf");
            combatManager.CreateCombatant("Joey", "beserker");
            
            UpdateCurrentCombatantName();
            PopulateWeaponListBox();

            
            camera.SetTarget(combatManager.GetCurrentCombatant().location);
            camera.SetPanToTargetEnabled(true);

            canvas.Width = camera.GetWidth();
            canvas.Height = camera.GetHeight();
            
            TextRenderer.Initialize(camera);

            VisionCalculator.Initialize(combatManager, Tile.size * 6);

            SetupViewport();
            
            Application.Idle += ApplicationIsIdle;

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            stopWatch.Start();
        }

        private void PopulateWeaponListBox()
        {
            combatantWeaponListBox.Items.Clear();
            foreach (Weapon weapon in combatManager.GetCurrentCombatant().weapons)
            {
                combatantWeaponListBox.Items.Add(weapon.name);
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            if (!canvasLoaded)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Color3(Color.White);

            combatManager.Draw(camera);

            combatManager.cursor.RenderInformationAtLocation(camera);

            if (mouseLocation != null)
                TileRenderer.RenderTile("cursor", mouseLocation);

            canvas.SwapBuffers();
        }

        public void UpdateCurrentCombatantName()
        {
            combatantNameLabel.Text = combatManager.GetCurrentCombatant().name;
        }

        private void SetupViewport()
        {
            int w = canvas.Width;
            int h = canvas.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, h, 0, -1, 1);
            GL.Viewport(0, 0, w, h);
        }

        private float msPerFrame(float framesPerSecond)
        {
            return 1000.0f / framesPerSecond;
        }

        private void ApplicationIsIdle(object sender, EventArgs e)
        {
            elapsedTime += ComputeTimeSlice();
            if (elapsedTime > msPerFrame(60))
            {
                elapsedTime -= msPerFrame(60);
                //actions to perform 60 times per second
                combatantStatisticsLabel.Text = "Movement Remaining: " + combatManager.GetCurrentCombatant().movementLeft.ToString();
                combatantStateLabel.Text = combatManager.currentCombatantState.ToString();
                combatantWeaponLabel.Text = "Equipped Weapon: " + ((combatManager.GetCurrentCombatant().equippedWeapon != null) ? combatManager.GetCurrentCombatant().equippedWeapon.name : "Nothing");
                camera.PanToTarget();
                RedrawCanvas();
            }
        }

        private void RedrawCanvas()
        {
            canvas.Invalidate();
        }

        private double ComputeTimeSlice()
        {
            stopWatch.Stop();
            double timeslice = stopWatch.Elapsed.TotalMilliseconds;
            stopWatch.Reset();
            stopWatch.Start();
            return timeslice;
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
            //don't use this
            //use glControl1_PreviewKeyDown
        }

        private void canvas_Resize(object sender, EventArgs e)
        {
            SetupViewport();
            canvas.Invalidate();
        }

        private void canvas_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;

            if (e.KeyCode == Keys.Escape) Application.Exit();

            //camera controls
            if (e.KeyCode == Keys.D) camera.PanRight();
            else if (e.KeyCode == Keys.A) camera.PanLeft();
            if (e.KeyCode == Keys.W) camera.PanUp();
            else if (e.KeyCode == Keys.S) camera.PanDown();

            if (e.KeyCode == Keys.C) camera.TogglePanToTargetEnabled();
            if (e.KeyCode == Keys.R) combatManager.GetCurrentCombatant().ResetMovementLeft();
            if (e.KeyCode == Keys.T) combatManager.cursor.Toggle(camera);
            if (e.KeyCode == Keys.Delete) combatManager.CreateCombatant("Beserker", "beserker");

            if (e.KeyCode == Keys.NumPad5) AudioManager.Play("laugh");
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void EndTurnButton_Click(object sender, EventArgs e)
        {
            combatManager.EndCurrentCombatantsTurn();
            camera.SetTarget(combatManager.GetCurrentCombatant().location);
            UpdateCurrentCombatantName();
            combatManager.currentCombatantState = CombatantState.Moving;
            PopulateWeaponListBox();
        }
                
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocation = new Location(e.Y / Tile.size, e.X / Tile.size);
        }
                
        private void canvasClicked(object sender, EventArgs e)
        {
            Location dungeonLocationClicked = camera.CurrentViewToDungeonLocation(mouseLocation);
            
            switch (combatManager.currentCombatantState)
            {
                case CombatantState.Moving:
                    if (combatManager.GetCurrentCombatant().HasMovementLeft() &&
                        CombatManager.LocationInListOfLocations(combatManager.GetCurrentCombatant().GetOpenLocationsAroundMe(), dungeonLocationClicked))
                    {
                        combatManager.GetCurrentCombatant().MoveDirectlyToLocation(dungeonLocationClicked);
                        camera.SetTarget(dungeonLocationClicked);
                    }
                    break;
                case CombatantState.Attacking:
                    if (combatManager.CombatantIsHere(dungeonLocationClicked))
                    {
                        combatManager.GetCurrentCombatant().equippedWeapon.Attack(combatManager.GetCombatantAt(dungeonLocationClicked));
                    }
                    break;
            }
        }

        private void canvas_Click(object sender, EventArgs e)
        {
            canvasClicked(sender, e);
        }

        private void canvas_DoubleClick(object sender, EventArgs e)
        {
            canvasClicked(sender, e);
        }

        private void AttackButton_Click(object sender, EventArgs e)
        {
            combatManager.currentCombatantState = CombatantState.Attacking;
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {
            combatManager.currentCombatantState = CombatantState.Moving;
        }

        private void EquipWeaponButton_Click(object sender, EventArgs e)
        {
            combatManager.GetCurrentCombatant().EquipWeapon((string)combatantWeaponListBox.SelectedItem);
        }

        

    }
}
