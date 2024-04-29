using System.Text;
using System.Windows;
using System.Drawing;
using Microsoft.Win32;

namespace ImageToAscii
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private Bitmap ResizeImage(Bitmap image, int width)
        {
            // <summary>
            //  Since the bitmap class only takes whole numbers in the constructor I made a workaround for the aspect ratio
            //  The size options will be multiples of 16 and since the difference between 16 and 9 is 7
            //  The difference for higher multiples of 16 and 9 will be multiples of 7 as well
            //  So this divides the width by 16 to find the multiple count then subtracts the product of 7 times that value from the width
            // </summary>

            int multiple = width / 16;
            int height = width - Math.Abs(multiple * -7);

            Bitmap resizedImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height);
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
            string? selectedSize = sizeOptions.SelectionBoxItem.ToString();

            if (!string.IsNullOrWhiteSpace(filePath.Text))
            {
                if (selectedSize != null && selectedSize != "")
                {
                    int imageWidth = int.Parse(selectedSize);

                    Bitmap image = new Bitmap(filePath.Text);
                    Bitmap imageToResize = ResizeImage(image, imageWidth);

                    string asciiImage = ConvertToAscii(imageToResize);
                    asciiBox.Text = asciiImage;
                }

                else
                {
                    MessageBox.Show("You must select a size before THE CONVERSION!");
                }
            }

            else
            {
                MessageBox.Show("Must select an image to convert first!");
            }
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