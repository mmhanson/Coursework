using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    /// <summary>
    /// This class is what the world is drawn onto. In the GUI, this corresponds to the black square
    /// with the stars, ships, and projectiles on it. It includes several helper methods to assist
    /// with drawing.
    /// </summary>
    public class DrawingPanel : Panel
    {
        /// <summary>
        /// The model that this drawing panel will represent.
        /// </summary>
        private SpaceWarsWorld world;

        /// <summary>
        /// The name of the client player. Used for custom ship images.
        /// </summary>
        private string username;

        /// <summary>
        /// The ID of the client player. Used for custom ship images.
        /// </summary>
        private int userid;

        /// <summary>
        /// Bitmap of a ship created from a png file in the contructor.
        /// 
        /// <remarks>
        /// The reason this is a property is so the DrawShip method can reference it
        /// every time a ship is drawn and not have to read a file and construct a new
        /// bitmap every call.
        /// </remarks>
        /// </summary>
        private Bitmap shipGreenImage;
        private Bitmap shipRedImage;
        private Bitmap shipBlueImage;
        private Bitmap shipYellowImage;
        private Bitmap shipVioletImage;
        private Bitmap shipBrownImage;
        private Bitmap shipWhiteImage;
        private Bitmap shipGreyImage;
        private Bitmap customShipImage;

        /// <summary>
        /// Images for thrusting
        /// </summary>
        private Bitmap shipGreenThrustingImage;
        private Bitmap shipRedThrustingImage;
        private Bitmap shipBlueThrustingImage;
        private Bitmap shipYellowThrustingImage;
        private Bitmap shipVioletThrustingImage;
        private Bitmap shipBrownThrustingImage;
        private Bitmap shipWhiteThrustingImage;
        private Bitmap shipGreyThrustingImage;

        /// <summary>
        /// Bitmap of a projectile created from a png file in the contructor.
        /// 
        /// <remarks>
        /// The reason this is a property is so the DrawProj method can reference it
        /// every time a proj is drawn and not have to read a file and construct a new
        /// bitmap every call.
        /// </remarks>
        /// </summary>
        private Bitmap projGreenImage;
        private Bitmap projRedImage;
        private Bitmap projYellowImage;
        private Bitmap projBlueImage;
        private Bitmap projVioletImage;
        private Bitmap projBrownImage;
        private Bitmap projWhiteImage;
        private Bitmap projGreyImage;

        /// <summary>
        /// Bitmap of a star created from a png file in the contructor.
        /// 
        /// <remarks>
        /// The reason this is a property is so the DrawStar method can reference it
        /// every time a star is drawn and not have to read a file and construct a new
        /// bitmap every call.
        /// </remarks>
        /// </summary>
        private Bitmap starImage;

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// Create a drawing panel.
        /// </summary>
        /// <param name="world">The model to draw</param>
        public DrawingPanel(SpaceWarsWorld world)
        {
            // this property stops screen flickering as drawing is not done as the panel is displaying
            this.DoubleBuffered = true;

            // set the background to black
            // note this property sets the background color of contained controls if background image is set
            //   since the image is not set now, this property will set the background color of the whole panel.
            this.BackColor = Color.Black;

            // register our custom handler with the Paint event
            this.Paint += PaintEventHandler;

            // set properties
            this.world = world;
            // Initialize the native images
            this.shipGreenImage = new Bitmap(@"..\..\..\Resources\Images\ship-coast-green.png");
            this.projGreenImage = new Bitmap(@"..\..\..\Resources\Images\shot-green.png");
            this.shipGreenThrustingImage = new Bitmap(@"..\..\..\Resources\Images\ship-thrust-green.png");
            this.shipRedImage = new Bitmap(@"..\..\..\Resources\Images\ship-coast-red.png");
            this.projRedImage = new Bitmap(@"..\..\..\Resources\Images\shot-red.png");
            this.shipRedThrustingImage = new Bitmap(@"..\..\..\Resources\Images\ship-thrust-red.png");
            this.shipBlueImage = new Bitmap(@"..\..\..\Resources\Images\ship-coast-blue.png");
            this.shipBlueThrustingImage = new Bitmap(@"..\..\..\Resources\Images\ship-thrust-blue.png");
            this.shipYellowImage = new Bitmap(@"..\..\..\Resources\Images\ship-coast-yellow.png");
            this.shipYellowThrustingImage = new Bitmap(@"..\..\..\Resources\Images\ship-thrust-yellow.png");
            this.shipVioletImage = new Bitmap(@"..\..\..\Resources\Images\ship-coast-violet.png");
            this.shipVioletThrustingImage = new Bitmap(@"..\..\..\Resources\Images\ship-thrust-violet.png");
            this.shipBrownImage = new Bitmap(@"..\..\..\Resources\Images\ship-coast-brown.png");
            this.shipBrownThrustingImage = new Bitmap(@"..\..\..\Resources\Images\ship-thrust-brown.png");
            this.shipWhiteImage = new Bitmap(@"..\..\..\Resources\Images\ship-coast-white.png");
            this.shipWhiteThrustingImage = new Bitmap(@"..\..\..\Resources\Images\ship-thrust-white.png");
            this.shipGreyImage = new Bitmap(@"..\..\..\Resources\Images\ship-coast-grey.png");
            this.shipGreyThrustingImage = new Bitmap(@"..\..\..\Resources\Images\ship-thrust-grey.png");
            this.projYellowImage = new Bitmap(@"..\..\..\Resources\Images\shot-yellow.png");
            this.projBlueImage = new Bitmap(@"..\..\..\Resources\Images\shot-blue.png");
            this.projVioletImage = new Bitmap(@"..\..\..\Resources\Images\shot-violet.png");
            this.projBrownImage = new Bitmap(@"..\..\..\Resources\Images\shot-brown.png");
            this.projWhiteImage = new Bitmap(@"..\..\..\Resources\Images\shot-white.png");
            this.projGreyImage = new Bitmap(@"..\..\..\Resources\Images\shot-grey.png");
            this.starImage = new Bitmap(@"..\..\..\Resources\Images\star.jpg");
        }

        /// <summary>
        /// Adds the custom ship to the drawing tool.
        /// </summary>
        /// <param name="ship"></param>
        public void RegisterCustomShip(Bitmap ship)
        {
            customShipImage = ship;
        }

        /// <summary>
        /// Adds the players name to the drawer
        /// </summary>
        /// <param name="name"></param>
        public void RegisterUsername(string name)
        {
            username = name;
        }

        /// <summary>
        /// Adds the players ID to the drawer
        /// </summary>
        /// <param name="id"></param>
        public void RegisterId(int id)
        {
            userid = id;
        }

        /// <summary>
        /// Repaint the drawing panel.
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
        /// will redraw only the drawing panel part of the form. Since it was entirely erased, this
        /// method will go through every entity in the model and draw it onto the drawing panel.
        /// </remarks>
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="pea">args from the paint event. Just passed to drawer delegates.</param>
        protected void PaintEventHandler(object sender, PaintEventArgs pea)
        {
            // the world is locked so the model cannot be changed by other threads (e.g. the controller)
            //   while this method is iterating on them with a foreach loop
            lock (this.world)
            {
                // Draw the ships
                foreach (Ship ship in world.Ships)
                {
                    if (ship.HitPoints > 0)
                    {
                        DrawObjectWithTransform(pea, ship, this.Size.Width, ship.Location.GetX(), ship.Location.GetY(),
                            ship.Direction.ToAngle(), ShipDrawer);
                    }
                }

                // Draw the projectiles
                foreach (Projectile proj in world.Projectiles)
                {
                    DrawObjectWithTransform(pea, proj, this.Size.Width, proj.Location.GetX(), proj.Location.GetY(),
                        proj.Direction.ToAngle(), ProjectileDrawer);
                }

                // Draw the stars
                foreach (Star star in world.Stars)
                {
                    DrawObjectWithTransform(pea, star, this.Size.Width, star.Location.GetX(), star.Location.GetY(),
                        0, StarDrawer);   // angle zero since star angle not relevant
                }
            }
        }

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// 
        /// <remarks>
        /// Taken from lab 10. Some comments and documentation was added, but the functionality was not altered.
        /// </remarks>
        /// </summary>
        /// <param name="pea">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="obj">The object to draw</param>
        /// <param name="worldEdgeSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldXCoord">The X coordinate of the object in world space</param>
        /// <param name="worldYCoord">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs pea, object obj, int worldEdgeSize, double worldXCoord, double worldYCoord, double angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int pixelXCoord = WorldSpaceToImageSpace(worldEdgeSize, worldXCoord);
            int pixelYCoord = WorldSpaceToImageSpace(worldEdgeSize, worldYCoord);
            pea.Graphics.TranslateTransform(pixelXCoord, pixelYCoord);
            pea.Graphics.RotateTransform((float)angle);

            // Draw the object 
            drawer(obj, pea);

            // Then undo the transformation
            pea.Graphics.ResetTransform();
        }

        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        // === DRAWER DELEGATES ===
        // the methods below are helper methods (indirectly) used by PaintEventHandler
        //   to draw the entities from the model onto the panel.

        /// <summary>
        /// Draw a ship centered at 0, 0
        /// 
        /// <remarks>
        /// Taken from lab 10. Some comments and documentation was added, but the functionality was not altered.
        /// 
        /// This method is used en tandem with DrawObjectWithTransform. It is
        /// passed to the method as a delegate and used to draw ships at any
        /// location on the drawing panel
        /// </remarks>
        /// </summary>
        /// <param name="obj">The object to draw</param>
        /// <param name="pea">PaintEventArgs for accessing graphics</param>
        private void ShipDrawer(Object obj, PaintEventArgs pea)
        {
            int shipImageSideLen = 35;  // ships are scaled to 30x30 pixels on the panel
            Bitmap shipImage = shipGreenImage;
            if (obj is Ship)
            {
                Ship ship = (Ship)obj;
                // Select ship color
                if (!ReferenceEquals(customShipImage, null) && ship.PlayerName == username && ship.ShipID == userid)
                {
                    shipImage = customShipImage;
                }
                else
                {
                    if (ship.IsThrusting)
                    {
                        switch (ship.Color)
                        {
                            case "red":
                                shipImage = shipRedThrustingImage;
                                break;
                            case "green":
                                break;
                            case "blue":
                                shipImage = shipBlueThrustingImage;
                                break;
                            case "yellow":
                                shipImage = shipYellowThrustingImage;
                                break;
                            case "violet":
                                shipImage = shipVioletThrustingImage;
                                break;
                            case "brown":
                                shipImage = shipBrownThrustingImage;
                                break;
                            case "grey":
                                shipImage = shipGreyThrustingImage;
                                break;
                            case "white":
                                shipImage = shipWhiteThrustingImage;
                                break;
                        }
                    }
                    else
                    {
                        switch (ship.Color)
                        {
                            case "red":
                                shipImage = shipRedImage;
                                break;
                            case "green":
                                break;
                            case "blue":
                                shipImage = shipBlueImage;
                                break;
                            case "yellow":
                                shipImage = shipYellowImage;
                                break;
                            case "violet":
                                shipImage = shipVioletImage;
                                break;
                            case "brown":
                                shipImage = shipBrownImage;
                                break;
                            case "grey":
                                shipImage = shipGreyImage;
                                break;
                            case "white":
                                shipImage = shipWhiteImage;
                                break;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Attempted drawing non-ship object as ship");
            }
            this.DrawImage(pea, shipImage, shipImageSideLen);
    }

    /// <summary>
    /// Draw a projectile centered at 0, 0
    /// 
    /// <remarks>
    /// This method is used en tandem with DrawObjectWithTransform. It is
    /// passed to the method as a delegate and used to draw projectiles at
    /// any location on the drawing panel
    /// </remarks>
    /// </summary>
    /// <param name="obj">The object to draw</param>
    /// <param name="pea">PaintEventArgs for accessing graphics</param>
    private void ProjectileDrawer(Object obj, PaintEventArgs pea)
    {
        int projImageSideLen = 20;  // projectiles are scaled to 30x30 pixels on the panel
        Bitmap projImage = projGreenImage;
        Projectile proj = (Projectile)obj;
        switch (proj.Owner % 8)
        {
            case 0:
                projImage = projRedImage;
                break;
            case 1:
                break;
            case 2:
                projImage = projBlueImage;
                break;
            case 3:
                projImage = projYellowImage;
                break;
            case 4:
                projImage = projVioletImage;
                break;
            case 5:
                projImage = projBrownImage;
                break;
            case 6:
                projImage = projGreyImage;
                break;
            case 7:
                projImage = projWhiteImage;
                break;
        }
        this.DrawImage(pea, projImage, projImageSideLen);
    }

    /// <summary>
    /// Draw a star centered at 0, 0
    /// 
    /// <remarks>
    /// This method is used en tandem with DrawObjectWithTransform. It is
    /// passed to the method as a delegate and used to draw stars at any
    /// location on the drawing panel
    /// </remarks>
    /// </summary>
    /// <param name="obj">The object to draw</param>
    /// <param name="pea">PaintEventArgs for accessing graphics</param>
    private void StarDrawer(Object obj, PaintEventArgs pea)
    {
        int starImageSideLen = 50;  // stars are scaled to 30x30 pixels on the panel
        this.DrawImage(pea, this.starImage, starImageSideLen);
    }

    /// <summary>
    /// Draw a bitmap on the panel centered at 0,0 and scaled to a square with a specified side length.
    /// </summary>
    /// <param name="pea">paint event args where this method gets the graphics object to draw with</param>
    /// <param name="image">Image to draw</param>
    /// <param name="sideLenPixels">side length (after transformation) of the image to draw in pixels</param>
    private void DrawImage(PaintEventArgs pea, Bitmap image, int sideLenPixels)
    {
        Size rectSize = new Size(sideLenPixels, sideLenPixels);
        // moves to top left left by half the image side length and up by the same length
        // done to center the image at (0, 0) and not have the top left corner at (0, 0)
        Point rectTopLeft = new Point(-(sideLenPixels / 2), -(sideLenPixels / 2));

        Rectangle rect = new Rectangle(rectTopLeft, rectSize);
        pea.Graphics.DrawImage(image, rect);
    }
}
}
