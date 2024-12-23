
using SkiaSharp;

using SkiaSharp.Views.Maui;

namespace Mde.Project.Mobile.Contents{
    public partial class SnowAnimationView : ContentView{
        private static readonly Random random = new Random();
        private List<Snowflake> snowflakes = new List<Snowflake>();

        public SnowAnimationView()
        {
            InitializeComponent();
            StartSnowfall();
        }

        private void StartSnowfall()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(30), () =>
            {
                canvasView.InvalidateSurface();
                return true;
            });

            for (int i = 0; i < 100; i++)
            {
                snowflakes.Add(new Snowflake
                {
                    Position = new SKPoint(random.Next(0, 1000), random.Next(0, 1000)),
                    Velocity = new SKPoint(0, random.Next(1, 5)),
                    Size = (float)(random.NextDouble() * 5f + 5f)
                });
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            var snowPaint = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true
            };

            foreach (var snowflake in snowflakes)
            {
                canvas.DrawCircle(snowflake.Position, snowflake.Size, snowPaint);
                snowflake.Position = new SKPoint(
                    snowflake.Position.X, 
                    (snowflake.Position.Y + snowflake.Velocity.Y) % e.Info.Height
                );
            }
        }
    }

    public class Snowflake
    {
        public SKPoint Position { get; set; }
        public SKPoint Velocity { get; set; }
        public float Size { get; set; }
    }
}
