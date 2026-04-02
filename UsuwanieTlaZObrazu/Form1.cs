using System.Drawing;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
namespace UsuwanieTlaZObrazu
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();

            int logicalProcessors = Environment.ProcessorCount;
            if (trackThreads.Maximum < logicalProcessors)
            {
                trackThreads.Maximum = 64;
            }

            trackThreads.Value = logicalProcessors;
            labelThreadsValue.Text = $"{trackThreads.Value}";
        }

        [DllImport("BibliotekaCpp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessImageCpp(
            IntPtr pData,
            int width,
            int height,
            int stride,
            byte keyR,
            byte keyG,
            byte keyB,
            int tolerance
            );

        [DllImport("BibliotekaAsm.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessImageAsm(
            IntPtr pData,
            int width,
            int height,
            int stride,
            int keyR,
            int keyG,
            int keyB,
            int tolerance
            );

        private void pictureBoxOutput_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void trackTolerance_Scroll(object sender, EventArgs e)
        {
            labelToleranceValue.Text = $"{trackTolerance.Value}";
        }

        private void labelColorInfo_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkCpp.Checked)
            {
                checkAsm.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkAsm.Checked)
            {
                checkCpp.Checked = false;
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFIleDialog = new OpenFileDialog();

            openFIleDialog.Filter = "Pliki obrazów|*.bmp;*.jpg;*.jpeg;*.png";
            openFIleDialog.Title = "Wybierz obraz";

            if (openFIleDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap originalImage = new Bitmap(openFIleDialog.FileName);

                pictureBoxInput.Image = originalImage;

                pictureBoxOutput.Image = null;
            }
        }

        private void buttonProcess_Click(object sender, EventArgs e)
        {
            if (pictureBoxInput.Image == null)
            {
                MessageBox.Show("Najpierw wczytaj obraz!");
                return;
            }

            if (!checkCpp.Checked && !checkAsm.Checked)
            {
                MessageBox.Show("Zaznacz C++ lub ASM x64, aby przetestowaæ!");
                return;
            }

            Bitmap bitmap = new Bitmap(pictureBoxInput.Image);

            if (bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                Bitmap temp = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(temp))
                {
                    g.DrawImage(bitmap, 0, 0);
                }
                bitmap = temp;
            }

            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );

            int tolerance = trackTolerance.Value;
            int toleranceScaled = (int)(tolerance * 2.5);
            int threads = trackThreads.Value;
            bool useAsm = checkAsm.Checked;
            Color colorToDelete = panelSelectedColor.BackColor;
            int stride = Math.Abs(bmpData.Stride);
            int height = bitmap.Height;
            int width = bitmap.Width;
            IntPtr scan0 = bmpData.Scan0;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            ExecuteParallelProcessing(scan0, stride, width, height, threads, useAsm, colorToDelete, tolerance);

            stopwatch.Stop();

            bitmap.UnlockBits(bmpData);

            pictureBoxOutput.Image = bitmap;
            double microseconds = (double)stopwatch.Elapsed.Ticks / System.Diagnostics.Stopwatch.Frequency * 1_000_000;
            labelTime.Text = $"Czas: {microseconds:F2} µs";
        }

        private void pictureBoxInput_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void trackThreads_Scroll(object sender, EventArgs e)
        {
            labelThreadsValue.Text = $"{trackThreads.Value}";
        }

        private void panelSelectedColor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBoxInput_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBoxInput.Image == null) return;

            Bitmap original = (Bitmap)pictureBoxInput.Image;
            PictureBox pictureBox = pictureBoxInput;

            float imageAspect = (float)original.Width / original.Height;
            float controlAspect = (float)pictureBox.Width / pictureBox.Height;

            float scaleFactor;
            float xOffset = 0;
            float yOffset = 0;

            if (imageAspect > controlAspect)
            {
                scaleFactor = (float)pictureBox.Width / original.Width;
                float scaledHeight = original.Height * scaleFactor;
                yOffset = (pictureBox.Height - scaledHeight) / 2;
            }
            else
            {
                scaleFactor = (float)pictureBox.Height / original.Height;
                float scaledWidth = original.Width * scaleFactor;
                xOffset = (pictureBox.Width - scaledWidth) / 2;
            }

            int realX = (int)((e.X - xOffset) / scaleFactor);
            int realY = (int)((e.Y - yOffset) / scaleFactor);

            if (realX < 0 || realX >= original.Width || realY < 0 || realY >= original.Height)
            {
                return; // Klikniêto poza obrazek
            }

            Color pickedColor = original.GetPixel(realX, realY);

            panelSelectedColor.BackColor = pickedColor;
        }

        private void labelThreads_Click(object sender, EventArgs e)
        {

        }

        private void buttonBenchmark_Click(object sender, EventArgs e)
        {
            string[] fileNames = { "maly.jpg", "sredni.png", "duzy.jpg" };

            foreach (var f in fileNames)
            {
                if (!System.IO.File.Exists(f))
                {
                    MessageBox.Show($"Nie znaleziono pliku: {f}\nWrzuc go do folderu z plikiem .exe!");
                    return;
                }
            }

            int[] threadCounts = { 1, 2, 4, 8, 16, 32, 64 };
            int runsPerSetting = 5;

            Color colorToDelete = panelSelectedColor.BackColor;
            int toleranceScaled = (int)(trackTolerance.Value * 2.5);

            System.Text.StringBuilder results = new System.Text.StringBuilder();
            results.AppendLine("Obraz;Biblioteka;Watki;Czas[µs]");

            this.Text = "Benchmark w toku...";
            this.Enabled = false;

            try
            {
                foreach (string fileName in fileNames)
                {
                    using (Bitmap loadedBmp = new Bitmap(fileName))
                    {
                        Bitmap bmp32;
                        if (loadedBmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                        {
                            bmp32 = new Bitmap(loadedBmp.Width, loadedBmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            using (Graphics g = Graphics.FromImage(bmp32))
                            {
                                g.DrawImage(loadedBmp, 0, 0);
                            }
                        }
                        else
                        {
                            bmp32 = new Bitmap(loadedBmp);
                        }
                        foreach (int threads in threadCounts)
                        {
                            double averageCpp = RunBenchmarkLoop(bmp32, false, threads, runsPerSetting, colorToDelete, toleranceScaled);
                            results.AppendLine($"{fileName};C++;{threads};{averageCpp:F2}");
                            double averageAsm = RunBenchmarkLoop(bmp32, true, threads, runsPerSetting, colorToDelete, toleranceScaled);
                            results.AppendLine($"{fileName};ASM x64;{threads};{averageAsm:F2}");
                        }
                        bmp32.Dispose();
                    }
                }
                string path = "wyniki_testoww.csv";
                System.IO.File.WriteAllText(path, results.ToString());
                MessageBox.Show($"Testy zakonczone! Wyniki zapisano w: {System.IO.Path.GetFullPath(path)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wyst¹pi³ b³¹d: {ex.Message}");
            }
            finally
            {
                this.Text = "Gotowe";
                this.Enabled = true;
            }
        }

        private double RunBenchmarkLoop (Bitmap bitmap, bool useAsm, int threads, int runs, Color color, int tolerance)
        {
            MeasureSingleRun(bitmap, useAsm, threads, color, tolerance);

            double totalTime = 0.0;
            for (int i = 0; i < runs; i++)
            {
                totalTime += MeasureSingleRun(bitmap, useAsm, threads, color, tolerance);
            }

            return totalTime / runs;
        }

        private double MeasureSingleRun (Bitmap bitmap, bool useAsm, int threads, Color color, int tolerance)
        {
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );

            int width = bitmap.Width;
            int height = bitmap.Height;
            int stride = bmpData.Stride;
            IntPtr scan0 = bmpData.Scan0;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            ExecuteParallelProcessing(scan0, stride, width, height, threads, useAsm, color, tolerance);

            stopwatch.Stop();
            bitmap.UnlockBits(bmpData);
            return (double)stopwatch.Elapsed.Ticks / System.Diagnostics.Stopwatch.Frequency * 1_000_000;
        }

        private void ExecuteParallelProcessing(IntPtr scan0, int stride, int width, int height, int threads, bool useAsm, Color color, int tolerance)
        {
            Parallel.For(0, threads, i =>
            {
                int startY = (height * i) / threads;
                int endY = (height * (i + 1)) / threads;
                int localHeight = endY - startY;

                if (localHeight > 0)
                {
                    int offset = startY * stride;
                    IntPtr localPtr = IntPtr.Add(scan0, offset);

                    if (useAsm)
                    {
                        ProcessImageAsm(localPtr, width, localHeight, stride, color.R, color.G, color.B, tolerance);
                    }
                    else
                    {
                        ProcessImageCpp(localPtr, width, localHeight, stride, color.R, color.G, color.B, tolerance);
                    }
                }
            });
        }
    }
}

