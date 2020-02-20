using Newtonsoft.Json;
using SpaceWars;

namespace Model
{
    /// <summary>
    /// An object that represents a start in the game. It has the following properties:
    /// 
    /// "star" - an int representing the star's unique ID.
    ///"loc" - a Vector2D representing the star's location.
    ///"mass" - a double representing the star's mass. Note that the sample client does not use this information, but you may choose to display stars differently based on their mass.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Star
    {
        /*
        * The info about these properties can be found in the method description
        */

        [JsonProperty(PropertyName = "star")]
        public int StarID { set; get; }

        [JsonProperty(PropertyName = "loc")]
        public Vector2D Location { set; get; }

        [JsonProperty(PropertyName = "mass")]
        public double Mass { set; get; }

        /// <summary>
        /// The number of frames before the ship is considered "timed out" and no longer rendered.
        /// </summary>
        private int frameCount;

        /// <summary>
        /// Create a new star.
        /// </summary>
        /// <param name="idnum">The id of the star</param>
        /// <param name="locXCoord">The X component of the location of the star</param>
        /// <param name="locYCoord">The Y component of the location of the star</param>
        /// <param name="mass">The mass of the star</param>
        public Star(int idnum, double locXCoord, double locYCoord, double mass)
        {
            this.StarID = idnum;
            this.Location = new Vector2D(locXCoord, locYCoord);
            this.Mass = mass;
        }

        /// <summary>
        /// Default star constructor
        /// </summary>
        public Star()
        {
            frameCount = 30;
        }

        /// <summary>
        /// Since each ID is unique between stars, it is an ideal use for a hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return StarID;
        }

        /// <summary>
        /// Stars are considered equal if they have the same ID.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        // This in combination with GetHashCode means we can use hashsets effectively in the model.
        public override bool Equals(object obj)
        {
            if (obj is Star)
            {
                Star otherStar = (Star)obj;
                return StarID == otherStar.StarID;
            }
            return false;
        }

        /// <summary>
        /// Increases the timeout counter for this object
        /// </summary>
        public void CountDown()
        {
            --frameCount;
        }

        /// <summary>
        /// Indicates if the object timed out or not
        /// </summary>
        /// <returns></returns>
        public bool TimedOut()
        {
            // When the frame count is zero, the object is considered timed out and no longer rendered.
            return frameCount == 0;
        }
    }
}
