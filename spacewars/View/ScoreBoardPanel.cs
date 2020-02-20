// This is a panel class designed to display each player's name, score, and health in a given SpaceWards game on a single server.
// Written by Joshua Cragun (u1025691) on 11/15/2018
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    class ScoreboardPanel : Panel
    {
        /// <summary>
        /// used to obtain player information to display. Never modified, only read from.
        /// </summary>
        private SpaceWarsWorld world;

        /// <summary>
        /// Health bar outline rectangle
        /// </summary>
        private Size healthBarOutlineSize;

        /// <summary>
        /// Health bar fill (hp indicator) rectangle
        /// </summary>
        private Size healthBarFillSize;

        /// <summary>
        /// Padding between health bar fill and health bar outline
        /// </summary>
        private int hpFillPadding;

        /// <summary>
        /// Padding between the scores and the outer edge of the scoreboard for all edges
        /// </summary>
        private int scorePadding;

        /// <summary>
        /// Font to draw the names in the scoreboard with.
        /// </summary>
        private Font nameFont;

        /// <summary>
        /// Size of the font to draw the names.
        /// 
        /// <remarks>
        /// Needed to compute offsets in the scoreboard
        /// </remarks>
        /// </summary>
        private int nameFontSize;

        /// <summary>
        /// The brush that the names in the scoreboard is drawn with.
        /// 
        /// <remarks>
        /// The brush here essentially just defines the color in which the names are drawn.
        /// </remarks>
        /// </summary>
        private Brush nameBrush;

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e 
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// Create a ScoreboardPanel
        /// </summary>
        /// <param name="world">
        /// the model to reference when drawing the scores. This class will not modify the model,
        /// it will only reference the ships to draw their names, scores, and health
        /// </param>
        public ScoreboardPanel(SpaceWarsWorld world)
        {
            // === Set the custom properties of this class ===
            this.world = world;
            // names are drawn in black, regular, 16-point Arial font
            this.nameFontSize = 12;
            this.nameFont = new Font(new FontFamily("Arial"), nameFontSize, FontStyle.Regular);
            this.nameBrush = new SolidBrush(Color.Black);
            this.hpFillPadding = 2;
            this.scorePadding = 5;
            // calculate the size of the hp bar
            this.healthBarOutlineSize = new Size(this.Size.Width - (2 * scorePadding), 10); // 10 high, fill entire width except padding
            this.healthBarFillSize = new Size(
                this.healthBarOutlineSize.Width - (2 * this.hpFillPadding) + 1,     // 1 must be added to counter the width of
                this.healthBarOutlineSize.Height - (2 * this.hpFillPadding) + 1     // the outline rectangle so the padding is even
                );
            // === set the inherited properties of this class ===
            this.DoubleBuffered = true; // done to stop screen flickering
            // set the background to white
            // note this property sets the background color of contained controls if background image is set
            //   since the image is not set now, this property will set the background color of the whole panel.
            this.BackColor = Color.White;

            // === register handlers ===
            // register our custom handler with the Paint event
            this.Paint += PaintEventHandler;
        }

        /// <summary>
        /// Repaint the scoreboard panel.
        /// 
        /// <remarks>
        /// his handler is registered with the 'Paint' event from the Control class in the constructor for this class.
        /// The Paint event is triggered by the OS every time this panel needs to be repainted.
        /// 
        /// This method is indirectly called by the 'UpdateViewHandler' method when the method calls
        /// the 'Invalidate' method. It 'invalidates' this form, causing a redraw to be triggered.
        /// So this method is triggered every time new information comes from the server and the window
        /// needs to be updated.
        /// 
        /// The invalidation mentioned earlier will erase the entire form and redraw it. This method
        /// will redraw only the scoreboard panel part of the form. Since it was entirely erased, this
        /// method will go through every player and redraw their status.
        /// </remarks>
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="pea">args from the paint event. Just passed to drawer delegates.</param>
        protected void PaintEventHandler(object sender, PaintEventArgs pea)
        {
            Graphics graphics = pea.Graphics;
            // Sort the ships in decending order
            IEnumerable <Ship> sortedShips = world.Ships.OrderByDescending(ship => ship.Score);

            int yOffset = 10;       // keeps track of how far down to start drawing
            foreach (Ship sortedShip in sortedShips)
            {
                // draw the score of the ship
                this.drawScore(graphics, sortedShip, yOffset);

                // increase the offset of the score
                yOffset += 50;
            }
        }

        /// <summary>
        /// Draw the score of a ship on the scoreboard.
        /// </summary>
        /// <param name="ship">The ship to draw the score of</param>
        /// <param name="yOffset">An offset from the top of the scoreboard to start drawing at</param>
        private void drawScore(Graphics graphics, Ship ship, int yOffset)
        {
            // draw player name label
            graphics.DrawString(ship.PlayerName + ": " + ship.Score, nameFont, nameBrush, scorePadding, yOffset);

            // draw health bar offsetted by the font size of the name
            // the score is drawn as two rectangles, a black one for the outline and a green one for the health
            // draw the outline
            Point outlineTopLeft = new Point(scorePadding, yOffset + this.nameFontSize + 5);    // add 5 to font size for extra padding, overlap otherwise
            Rectangle outlineRectangle = new Rectangle(outlineTopLeft, healthBarOutlineSize);
            graphics.DrawRectangle(new Pen(Color.Black), outlineRectangle);

            // draw the health bar fill
            Point fillTopLeft = new Point(outlineTopLeft.X + hpFillPadding, outlineTopLeft.Y + hpFillPadding);
            Rectangle fillRectangle = new Rectangle(fillTopLeft, healthBarFillSize);
            // scale to hp
            double hpScale = ((double) ship.HitPoints / (double) world.StartingHP);
            int newWidth = (int)(fillRectangle.Width * hpScale);
            fillRectangle.Width = newWidth;
            graphics.FillRectangle(new SolidBrush(Color.Green), fillRectangle);
        }
    }
}
