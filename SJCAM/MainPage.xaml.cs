using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using SJCAM.Effects;
using SJCAM.MainGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SJCAM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		Color Dominant;
		public string Title { get; set; }
		public string Description { get; set; }
		public ObservableCollection<Clickable> Menu { get; set; }

		private CompositionEffectBrush _brush;
		private Compositor _compositor;

		public MainPage()
        {
			Init();
			this.InitializeComponent();
			Title = "SJCAM";	
			Description = "Welcome!";
			_compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
			CreateMenu();
		}

		private void CreateMenu()
		{
			Menu = new ObservableCollection<Clickable>();
			Menu.Add(new Clickable("Photo", "\uE114" ));
			Menu.Add(new Clickable("Video", "\uE116" ));
			Menu.Add(new Clickable("Settings", "\uE179" ));
			Menu.Add(new Clickable("Photo", "\uE114" ));
		}

		private async void Init()
		{
			Dominant = await Images.GetDominant();
			var view = ApplicationView.GetForCurrentView();
			view.TitleBar.BackgroundColor = Dominant;
			view.TitleBar.ButtonBackgroundColor = Dominant;
		}

		private void Blur()
		{
			var graphicsEffect = new GaussianBlurEffect()
			{
				Name = "Blur",
				Source = new CompositionEffectSourceParameter("Backdrop"),
				BlurAmount = 50,
				BorderMode = EffectBorderMode.Hard,
			};
			var blurEffectFactory = _compositor.CreateEffectFactory(graphicsEffect, new[] { "Blur.BlurAmount" });

			_brush = blurEffectFactory.CreateBrush();
			var destinationBrush = _compositor.CreateBackdropBrush();
			_brush.SetSourceParameter("Backdrop", destinationBrush);
			var blurSprite = _compositor.CreateSpriteVisual();
			blurSprite.Size = new Vector2((float)backgroundImage.ActualWidth, (float)backgroundImage.ActualHeight);
			blurSprite.Brush = _brush;
			ElementCompositionPreview.SetElementChildVisual(backgroundImage, blurSprite);
		}

		private void BackgroundImage_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SpriteVisual blurVisual = (SpriteVisual)ElementCompositionPreview.GetElementChildVisual(backgroundImage);
			if (blurVisual != null)
				blurVisual.Size = e.NewSize.ToVector2();
		}

		private void StartBlurAnimation()
		{
			ScalarKeyFrameAnimation blurAnimation = _compositor.CreateScalarKeyFrameAnimation();
			blurAnimation.InsertKeyFrame(0.0f, 0.0f);
			blurAnimation.InsertKeyFrame(1.0f, 10.0f);
			blurAnimation.Duration = TimeSpan.FromSeconds(2);
			blurAnimation.IterationBehavior = AnimationIterationBehavior.Count;
			blurAnimation.IterationCount = 1;
			_brush.StartAnimation("Blur.BlurAmount", blurAnimation);
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			
		}

		private async void BackgroundImage_Loaded(object sender, RoutedEventArgs e)
		{
			await Task.Delay(2500);
			Blur();
			_brush.Properties.InsertScalar("Blur.BlurAmount", 0);
			StartBlurAnimation();
		}
	}
}
