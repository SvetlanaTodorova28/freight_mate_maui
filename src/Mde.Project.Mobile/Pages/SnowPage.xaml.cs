using SkiaSharp;


namespace Mde.Project.Mobile.Pages;

public partial class SnowPage : ContentPage
{
    private static readonly Random random = new Random();
    private List<Snowflake> snowflakes = new List<Snowflake>();
    private int snowflakeCount = 100; 

    public SnowPage()
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
    }

    private void OnCanvasViewPaintSurface(object sender, SkiaSharp.Views.Maui.SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        var width = e.Info.Width;
        var height = e.Info.Height;
        Console.WriteLine($"Canvas Width: {width}, Height: {height}");

        
        if (snowflakes.Count == 0)
        {
            for (int i = 0; i < snowflakeCount; i++)
            {
                snowflakes.Add(CreateRandomSnowflake(width, height));
            }
        }

        canvas.Clear(SKColors.Transparent);

        var snowPaint = new SKPaint
        {
            Color = SKColors.White,
            IsAntialias = true
        };

        // Update en teken sneeuwvlokken
        foreach (var snowflake in snowflakes)
        {
            canvas.DrawCircle(snowflake.Position, snowflake.Size, snowPaint);

            // Update positie
            snowflake.Position = new SKPoint(
                (snowflake.Position.X + snowflake.Velocity.X) % width, // Horizontale beweging
                (snowflake.Position.Y + snowflake.Velocity.Y) % height // Verticale beweging
            );

            // Plaats sneeuwvlokken die buiten het scherm vallen terug bovenaan
            if (snowflake.Position.Y > height || snowflake.Position.X > width)
            {
                snowflake.Position = new SKPoint(random.Next(0, width), -10); // Net boven het canvas
            }
        }
    }

    private Snowflake CreateRandomSnowflake(int width, int height)
    {
        return new Snowflake
        {
            Position = new SKPoint(random.Next(0, width), random.Next(0, height)),
            Velocity = new SKPoint((float)(random.NextDouble() * 2 - 1), random.Next(2, 6)), 
            Size = (float)(random.NextDouble() * 4f + 2f) 
        };
    }
}

public class Snowflake
{
    public SKPoint Position { get; set; }
    public SKPoint Velocity { get; set; }
    public float Size { get; set; }
}
