// MoonDisk.cs by Charles Petzold, March 2009
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace MoonPhaseCalendar
{
    public partial class MoonDisk : Viewport3D
    {
        // Create shareable model for moon.
        static string strMoonImage = "pack://application:,,,/MoonPhaseCalendar;Component/moonmap.png";
        // (Original moonmap4k.jpg from from http://planetpixelemporium.com/planets.html, 
        //      resized to 25% and converted to PNG.) 
        static Uri uriMoonImage = new Uri(strMoonImage);
        static BitmapImage imgMoon = new BitmapImage(uriMoonImage);
        static ImageBrush brush = new ImageBrush(imgMoon);
        static DiffuseMaterial matMoon = new DiffuseMaterial(brush);
        static MeshGeometry3D sphere = GenerateSphere(new Point3D(0, 0, 0), 1, 360, 180);
        static GeometryModel3D geomod = new GeometryModel3D(sphere, matMoon);

        // Create shareable model for lighting.
        static AmbientLight ambiLight = new AmbientLight(Color.FromRgb(16, 16, 16));
        static DirectionalLight dirLight = new DirectionalLight(Colors.White, new Vector3D(0, 0, -1));
        static Model3DGroup modgrp = new Model3DGroup();

        // Create shareable camera.
        static OrthographicCamera cam = new OrthographicCamera(new Point3D(0, 0, 2), new Vector3D(0, 0, -1),
                                                               new Vector3D(0, 1, 0), 2.1);

        // Put together Model3DGroup for lighting & freeze the two models.
        static MoonDisk()
        {
            geomod.Freeze();
            modgrp.Children.Add(ambiLight);

            for (int i = 0; i < 4; i++)
                modgrp.Children.Add(dirLight);

            modgrp.Freeze();
        }

        public MoonDisk()
        {
            InitializeComponent();

            // Put together the Viewport3D.
            viewport3d.Camera = cam;
            modvisGeometry.Content = geomod;
            modvisLight.Content = modgrp;

            // Set handler for DataContextChanged
            DataContextChanged += OnDataContextChanged;
        }

        void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            rotate.Angle = PhaseAngle((DateTime)args.NewValue);
        }

        static MeshGeometry3D GenerateSphere(Point3D center, double radius,
                              int slices, int stacks)
        {
            // Create the MeshGeometry3D.
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Fill the Position, Normals, and TextureCoordinates collections.
            for (int stack = 0; stack <= stacks; stack++)
            {
                double phi = Math.PI / 2 - stack * Math.PI / stacks;
                double y = radius * Math.Sin(phi);
                double scale = -radius * Math.Cos(phi);

                for (int slice = 0; slice <= slices; slice++)
                {
                    double theta = slice * 2 * Math.PI / slices;
                    double x = scale * Math.Sin(theta);
                    double z = scale * Math.Cos(theta);

                    Vector3D normal = new Vector3D(x, y, z);
                    mesh.Normals.Add(normal);
                    mesh.Positions.Add(normal + center);
                    mesh.TextureCoordinates.Add(
                            new Point((double)slice / slices,
                                      (double)stack / stacks));
                }
            }

            // Fill the TriangleIndices collection.
            for (int stack = 0; stack < stacks; stack++)
                for (int slice = 0; slice < slices; slice++)
                {
                    int n = slices + 1; // Keep the line length down.

                    if (stack != 0)
                    {
                        mesh.TriangleIndices.Add((stack + 0) * n + slice);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice);
                        mesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                    }
                    if (stack != stacks - 1)
                    {
                        mesh.TriangleIndices.Add((stack + 0) * n + slice + 1);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice);
                        mesh.TriangleIndices.Add((stack + 1) * n + slice + 1);
                    }
                }
            return mesh;
        }

        // All formula references are from 
        //  Jean Meeus, Astronomical Algorithms, first edition (Willmann-Bell, 1991)
        public double PhaseAngle(DateTime dtInput)
        {
            DateTime dt = dtInput;

            if (dtInput.Kind == DateTimeKind.Local)
                dt = dtInput.ToUniversalTime();

            // 7.1: Julian Day
            // ---------------
            int yr = dt.Year;
            int mon = dt.Month;
            double day = dt.Day + (dt.Hour + (dt.Minute +
                                           (dt.Second +
                                            dt.Millisecond / 1000.0) / 60.0) / 60.0) / 24.0;

            if (mon == 1 || mon == 2)
            {
                yr -= 1;
                mon += 12;
            }

            int A = yr / 100;
            int B = 2 - A + A / 4;

            double JD = Math.Floor(365.25 * (yr + 4716)) +
                        Math.Floor(30.6001 * (mon + 1)) + day + B - 1524.5;

            // Julian Emphemeris Day (approximate -- see chapter 9)
            // ----------------------------------------------------
            double JDE = JD;

            // 21.1: Time measured in Julian centures from Epoch J2000.0
            // ---------------------------------------------------------
            double tau = (JDE - 2451545) / 36525;

            // 45.2: Mean elongation of moon
            // -----------------------------
            double D = 297.8501921 +
                       445267.1114034 * tau -
                       0.0018819 * tau * tau +
                       tau * tau * tau / 545868 -
                       tau * tau * tau * tau / 113065000;

            D = Normalize(D);

            // 45.3: Sun's mean anomaly
            // ------------------------
            double M = 357.5291092 +
                       35999.0502909 * tau -
                       0.0001536 * tau * tau +
                       tau * tau * tau / 24490000;

            M = Normalize(M);

            // 45.4: Moon's mean anomaly
            // -------------------------
            double Mprime = 134.9633964 +
                            477198.8675055 * tau +
                            0.0087414 * tau * tau +
                            tau * tau * tau / 69699 -
                            tau * tau * tau * tau / 14712000;

            Mprime = Normalize(Mprime);

            // 46.4: Phase angle (simplified calculation)
            // ------------------------------------------
            double i = 180 - D - 6.289 * Sine(Mprime)
                               + 2.100 * Sine(M)
                               - 1.274 * Sine(2 * D - Mprime)
                               - 0.658 * Sine(2 * D)
                               - 0.214 * Sine(2 * Mprime)
                               - 0.110 * Sine(D);

            // 46.1: Illuminated fraction (not used)
            // --------------------------
            double k = (1 + Cosine(i)) / 2;

            return i;

        }

        double Normalize(double angle)
        {
            angle = angle - 360 * (int)(angle / 360);

            if (angle < 0)
                angle += 360;

            return angle;
        }
        double Sine(double angle)
        {
            return Math.Sin(Math.PI * angle / 180);
        }
        double Cosine(double angle)
        {
            return Math.Cos(Math.PI * angle / 180);
        }
    }
}
