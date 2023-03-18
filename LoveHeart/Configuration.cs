
namespace LoveHeart
{
    public class Configuration
    {
        public Configuration() : this("WYR, I Iove You!")
        {
        }
        public Configuration(string title) : this(title, 1200, 800)
        {
        }

        public Configuration(string title, int width, int height)
        {
            Title = title;
            Width = width;
            Height = height;
            WaitVerticalBlanking = false;
        }

        public string Title
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public bool WaitVerticalBlanking
        {
            get; set;
        }
    }
}
