using Microsoft.Xna.Framework.Graphics;

namespace GDLibrary
{
    public class ScreenUtility
    {
        //used to define screen layout in ScreenManager
        public enum ScreenType : sbyte
        {
            SingleScreen, //simple
            MultiScreen, // multiple (non-overlapping) cameras
            MultiPictureInPicture //multiple cameras with PIP e.g. rear-view mirror in a rally game
        }

        public static Integer2 RSGA = new Integer2(160, 100); //a made-up resolution!
        public static Integer2 CGA = new Integer2(320, 200);
        public static Integer2 QVGA = new Integer2(320, 240);
        public static Integer2 VGA = new Integer2(640, 480);
        public static Integer2 WVGA = new Integer2(800, 480);
        public static Integer2 SVGA = new Integer2(800, 600);
        public static Integer2 XVGA = new Integer2(1024, 768);
        public static Integer2 HD720 = new Integer2(1280, 720);
        public static Integer2 WXGA = new Integer2(1280, 768);
        public static Integer2 SXGA = new Integer2(1280, 1024);
        public static Integer2 HD1080 = new Integer2(1920, 1080);
        public static Integer2 WUXGA = new Integer2(1920, 1200);
        public static Integer2 TWOK = new Integer2(2048, 1080);


        //returns a viewport with padding on horizontal and vertical edges - used by cameras typically to make room for game state
        public static Viewport Pad(Viewport viewport, int leftPadding, int topPadding, int rightPadding, int bottomPadding)
        {
            //reduce by whatever padding has been specified
            return new Viewport(
                viewport.X + leftPadding,
                viewport.Y + topPadding,
                viewport.Width - leftPadding - rightPadding,
                viewport.Height - topPadding - bottomPadding);
        }

        public static Viewport Deflate(Viewport viewport, int deflateVertical, int deflateHorizontal)
        {
            //reduce by whatever padding has been specified
            return new Viewport(
                viewport.X + deflateHorizontal,
                viewport.Y + deflateVertical,
                viewport.Width - 2*deflateHorizontal,
                viewport.Height - 2 * deflateVertical);
        }
    }
}
