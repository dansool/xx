using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using CustomizedControl;
using CustomizedControl.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryAndroid))]
namespace CustomizedControl.Droid
{
    public class CustomEntryAndroid : EntryRenderer
    {
        public CustomEntryAndroid(Context context) : base(context)
        { }
        private bool HasUnderline;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var element = (CustomEntry)Element;
                HasUnderline = element.HasUnderline;
                var BorderClr = element.BorderColor.ToAndroid();

                if (HasUnderline == false)
                {
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetColor(Android.Graphics.Color.Transparent);
                    Control.SetBackgroundDrawable(gd);
                }

                if (BorderClr != Android.Graphics.Color.Transparent)
                {
                    int borderThickness = element.BorderThickness;
                    if (borderThickness == 0) { borderThickness = 1; } //in case border thickness was not set then default to 1
                    var brdr = new ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
                    brdr.Paint.Color = BorderClr;
                    brdr.Paint.SetStyle(Paint.Style.Stroke);
                    Control.Background = brdr;
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetColor(Android.Graphics.Color.Transparent);
                    gd.SetStroke(borderThickness, BorderClr);
                    Control.SetBackground(gd);
                }
            }

        }

    }
}