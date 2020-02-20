using Controller;
using Model;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    /// <summary>
    /// This windows form class acts as the 'view' portion of the game within the MVC architecture.
    /// </summary>
    public partial class GameForm : Form
    {
        /// <summary>
        /// The hardcoded width of the scoreboard.
        /// </summary>
        private const int SCOREBOARD_WIDTH = 200;

        /// <summary>
        /// How far down the world and scoreboard are drawn from the top of the window
        /// </summary>
        private const int TOOLBAR_HEIGHT = 35;

        /// <summary>
        /// The game controller
        /// </summary>
        private ClientController controller;

        private bool customUpload;

        /// <summary>
        /// The model for the world.
        /// Note that the controller creates this and passes it to the view, the view
        /// is not to modify the world, only to reference and display it.
        /// </summary>
        private SpaceWarsWorld world;

        /// <summary>
        /// Bitmap storage if the user whats a custom ship
        /// </summary>
        private Bitmap customShip;

        /// <summary>
        /// The panel on which the world (game) is drawn.
        /// </summary>
        private DrawingPanel drawingPanel;

        /// <summary>
        /// The panel on which the scorebord is drawn.
        /// </summary>
        private ScoreboardPanel scoreboardPanel;

        /// <summary>
        /// Create a new (form) view for the game.
        /// </summary>
        public GameForm()
        {
            InitializeComponent();
            this.controller = new ClientController();
            this.world = null;
            this.customUpload = false;

            // register handlers for form events
            this.KeyDown += KeyDownEventHandler;
            this.KeyUp += KeyUpEventHandler;

            // register handlers for events
            // register our own handlers for the controller's events which the view must respond to
            controller.ConnectionFailed += ConnectionFailedHandler;
            controller.ModelChangedEvent += UpdateViewHandler;
            controller.ConnectionEstablished += ConnectionEstablishedHandler;

            // set 'localhost' as default text and select name box
            this.serverTextBox.Text = "localhost";
            this.nameTextBox.Select();
        }

        /// <summary>
        /// Update the view.
        /// 
        /// <remarks>
        /// This method is registered to the ModelChangedEvent event in the controller, so this
        /// method is called by the controller whenever the model is updated. The controller
        /// will receive  updates from the server and update the model. Every time this happens
        /// the view will erase its contents and redraw everything.
        /// </remarks>
        /// </summary>
        private void UpdateViewHandler()
        {
            // invalidating this form will cause it to be repainted and updated to the new model
            MethodInvoker invoker = () => this.Invalidate(true);
            try
            {
                this.Invoke(invoker);
            }
            catch (ObjectDisposedException)
            {
                MethodInvoker CloseAppInvoker = () => this.Close();
                this.Invoke(CloseAppInvoker);
            }
        }

        /// <summary>
        /// Informs the user that their connection to the server failed and gives details as to why.
        /// </summary>
        /// <param name="errorMessage"></param>
        private void ConnectionFailedHandler(string errorMessage)
        {
            MessageBox.Show("Unable to connect to server. Error occured: " + errorMessage);

            // re-enable text inputs and button so the user can try again.
            MethodInvoker ResetClientInvoker = () =>
            {
                this.serverTextBox.Enabled = true;
                this.nameTextBox.Enabled = true;
                this.connectButton.Enabled = true;
                this.CustomShipButton.Enabled = true;
                // We may also need to dispose of the drawing panel
                if (!ReferenceEquals(drawingPanel, null)) {
                    drawingPanel.Dispose();
                }
            };
            this.Invoke(ResetClientInvoker);
        }

        /// <summary>
        /// Handle the connection being made.
        /// </summary>
        /// <param name="world">
        /// The world object created by the controller. The controller will follow
        /// a startup sequence and get information from the server about the world
        /// and construct the world object. After this sequence is complete, the 
        /// controller will trigger the ConnectionEstablished event and pass the
        /// world into the view, where we will use it to display the world.
        /// </param>
        private void ConnectionEstablishedHandler(SpaceWarsWorld world, string name, int id)
        {
            // setup the model
            this.world = world;
            
            // resize the form to accomadate the world and scoreboard
            MethodInvoker invoker = new MethodInvoker(() => ClientSize = new Size(world.Size + SCOREBOARD_WIDTH, world.Size));
            this.Invoke(invoker);

            // setup the drawing panel
            this.drawingPanel = new DrawingPanel(world);
            if (!ReferenceEquals(customShip, null))
            {
                drawingPanel.RegisterCustomShip(customShip);
            }
            // Add user info
            drawingPanel.RegisterUsername(name);
            drawingPanel.RegisterId(id);
            this.drawingPanel.Location = new Point(0, TOOLBAR_HEIGHT);
            this.drawingPanel.Size = new Size(this.ClientSize.Width - SCOREBOARD_WIDTH, this.ClientSize.Height);
            // put it on the form
            invoker = new MethodInvoker(() => this.Controls.Add(drawingPanel));
            this.Invoke(invoker);

            // setup the scoreboard
            this.scoreboardPanel = new ScoreboardPanel(world);
            this.scoreboardPanel.Location = new Point(this.world.Size, TOOLBAR_HEIGHT);   // position to left of world drawing panel
            this.scoreboardPanel.Size = new Size(SCOREBOARD_WIDTH, this.ClientSize.Height);
            // put it on the form
            invoker = new MethodInvoker(() => this.Controls.Add(scoreboardPanel));
            this.Invoke(invoker);
        }
        
        /// <summary>
        /// Triggered every time a key is pressed down in the form.
        /// 
        /// <remarks>
        /// This method handles the control of the player's ship, if any 'control' key is pressed
        /// down, this method will trigger the correct events in the controller.
        /// 
        /// Controls:
        ///   W, Up Arrow: Thrust
        ///   A, Left Arrow: Turn Left
        ///   D, Right Arrow: Turn Right
        ///   Space, Down Arrow, S: Fire
        /// </remarks>
        /// </summary>
        private void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            // get the key pressed and send to client via event
            // As per protocol thrust goes to 'T', fire to 'F', left to 'L',  right to 'R'
            // the xml doc shows which physical keys get mapped to which actions
            String actionCode;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                actionCode = "T";
                this.controller.KeyPressed(actionCode);
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                actionCode = "L";
                this.controller.KeyPressed(actionCode);
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                actionCode = "R";
                this.controller.KeyPressed(actionCode);
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S || e.KeyCode == Keys.Space)
            {
                actionCode = "F";
                this.controller.KeyPressed(actionCode);
            }
            // Get rid of the annoying sound every time a key is pressed
            e.Handled = true;
            e.SuppressKeyPress = true;
            // at this point, the key pressed was other key. Do nothing
        }

        /// <summary>
        /// Triggered every time a key is released in the form.
        /// 
        /// <remarks>
        /// This method handles the control of the player's ship, if any 'control' key is pressed
        /// down, this method will trigger the correct events in the controller.
        /// 
        /// Controls:
        ///   W, Up Arrow: Thrust
        ///   A, Left Arrow: Turn Left
        ///   D, Right Arrow: Turn Right
        ///   Space, Down Arrow, S: Fire
        /// </remarks>
        /// </summary>
        private void KeyUpEventHandler(object sender, KeyEventArgs e)
        {
            // get the key pressed and send to client via event
            // As per protocol thrust goes to 'T', fire to 'F', left to 'L',  right to 'R'
            // the xml doc shows which physical keys get mapped to which actions
            String actionCode;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                actionCode = "T";
                this.controller.KeyReleased(actionCode);
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                actionCode = "L";
                this.controller.KeyReleased(actionCode);
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                actionCode = "R";
                this.controller.KeyReleased(actionCode);
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S || e.KeyCode == Keys.Space)
            {
                actionCode = "F";
                this.controller.KeyReleased(actionCode);
            }
            // at this point, the key pressed was other key. Do nothing
        }

        /// <summary>
        /// Triggered when the 'Connect' button is pushed in the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButtonPushed_Handler(object sender, EventArgs e)
        {
            if (!customUpload)
            {
                var confirmResult = MessageBox.Show("You haven't uploaded a custom skin yet. Are you sure you want to connect? You won't be able to change it during a game.", "Upload Custom Skin?", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.No)
                {
                    // Don't remind the user again if they say no
                    customUpload = true;
                    return;
                }
                else
                {
                    customShip = null;
                }
            }

            // get info
            String server = serverTextBox.Text;
            String name = nameTextBox.Text;

            // give errors if invalid
            if (server == "" || name == "")
            {
                MessageBox.Show("Please enter a nonempty name and server address.");
                return;
            }

            // disable text inputs and button so keystrokes are sent to game.
            this.serverTextBox.Enabled = false;
            this.nameTextBox.Enabled = false;
            this.connectButton.Enabled = false;
            this.CustomShipButton.Enabled = false;

            // pass to controller
            this.controller.Connect(server, name);
        }

        /// <summary>
        /// This event will open a file dialog that will allow the user to choose their own bitmap for their ship! (locally)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomShipButton_Click(object sender, EventArgs e)
        {
            // displays a OpenFileDialog for loading a spreadsheet from file
            OpenFileDialog openCustomShip = new OpenFileDialog();

            // Filter for what files are displayed, defaults to '.sprd' extension but can select all files
            openCustomShip.Filter = "Image Files (*.bmp; *.jpg; *.jpeg; *.png)| *.bmp; *.jpg; *.jpeg; *.png; *.gif | All files(*.*) | *.*";
            openCustomShip.Title = "Open Custom Ship File";
            openCustomShip.ShowDialog();

            // If the file name is not an empty string, then create a new Form class with the file path
            // this will open a new spreadsheet window
            if (openCustomShip.FileName != "")
            {
                try
                {
                    customShip = new Bitmap(openCustomShip.FileName);
                    customUpload = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Failed to upload custom ship: " + ex.Message);
                }
                openCustomShip.Dispose();
            }
        }
    }
}
