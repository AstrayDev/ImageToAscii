using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Microsoft.Windows;
using Microsoft.Win32;
using System.IO;

namespace ImageToAscii
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private Bitmap ResizeImage(Bitmap image, int width)
        {
            float aspectRatio = 16 / 9;

            int height = (int)(width * aspectRatio);

            Bitmap resizedImage = new Bitmap(width, height / 2);

            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height / 2);
            }

            return resizedImage;
        }

        private string ConvertToAscii(Bitmap image)
        {
            StringBuilder sb = new StringBuilder();
            string[] asciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", "," };

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);

                    int r = pixelColor.R;
                    int g = pixelColor.G;
                    int b = pixelColor.B;

                    Color greyScale = Color.FromArgb(r, g, b);

                    int index = (greyScale.R * 10) / 255;

                    sb.Append(asciiChars[index]);
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

        private void AsciiButton_Click(object sender, RoutedEventArgs e)
        {
            Bitmap image = new Bitmap(filePath.Text);
            //Bitmap bmp = new Bitmap(image, new System.Drawing.Size(image.Width / 20, image.Height / 20));
            Bitmap imageToResize = ResizeImage(image, 80);


            string asciiImage = ConvertToAscii(imageToResize);

            asciiBox.Text = asciiImage;
        }

        private void FileBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.ShowDialog();

            dlg.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg";

            filePath.Text = dlg.FileName;
        }
    }
}