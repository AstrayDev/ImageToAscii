using System.Text;
using System.Windows;
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration.Internal;

namespace ImageToAscii
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private string ConvertToAscii(Bitmap image)
        {
            StringBuilder sb = new StringBuilder();
            char[] asciiChars = new char[]
            {
                '@', '#', '8', '&', '%', 'B', 'M', 'W', 'Q', 'R', 'E', 'N', 'H', 'K', '$', 'G', 'O', 'A', 'U', 'D', 'I', 'V', 'S', 'P', 'Z', 'Y', 'J', 'L', 'C', 'X', 'T', 'F',
                'm', 'w', 'a', 'h', 'k', 'b', 'd', 'u', 'n', 'e', 'p', 'o', 'q', 'g', 'f', 'l', 't', 'c', 'y', 'v', 'i', 'x', 'z', 'r', 'j', 's', '1', '2', '3', '4', '5', '6', '7', '0',
                '!', '?', '|', '{', '}', '[', ']', '(', ')', '+', '=', '<', '>', ':', ';', ',', '~', '-', '^', '_', '"', '\'', '\\', '/', '.', ' '
            };


            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);

                    int r = pixelColor.R;
                    int g = pixelColor.G;
                    int b = pixelColor.B;

                    Color grayScale = Color.FromArgb(r, g, b);

                    int index = (grayScale.R * asciiChars.Length - 1) / 255;

                    sb.Append(asciiChars[index]);
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

        private void AsciiButton_Click(object sender, RoutedEventArgs e)
        {
            string? textSize = sizeOptions.SelectionBoxItem.ToString();

            if (!string.IsNullOrWhiteSpace(filePath.Text) && File.Exists(filePath.Text))
            {
                if (!string.IsNullOrWhiteSpace(textSize))
                {
                    int selectedSize = int.Parse(textSize);
                    Bitmap image = new Bitmap(filePath.Text);

                    int width = image.Width / selectedSize;
                    int height = image.Height / selectedSize;

                    Bitmap imageToResize = ResizeImage(image, width, height);
                    string asciiImage = ConvertToAscii(imageToResize);
                    asciiBox.Text = asciiImage;
                }

                else
                {
                    MessageBox.Show("Must select a size before the CONVERSION");
                }
            }

            else if (string.IsNullOrWhiteSpace(filePath.Text))
            {
                MessageBox.Show("Must select an image to convert first!");
            }

            else
            {
                MessageBox.Show("File not found. Make sure file path is correct.");
            }
        }

        private void FileBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Image Files|*.jpg;*.jpg;*.png;*.jpeg";

            dlg.ShowDialog();

            filePath.Text = dlg.FileName;
        }
    }
}