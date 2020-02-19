using StarMap.Cll.Abstractions.Urho;
using Xamarin.Forms;

namespace StarMap.Views
{
	public partial class StarDetailPage : ContentPage
	{
		public StarDetailPage()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await ((IUrhoHandler)BindingContext).GenerateUrho(surface);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			surface = null;
		}
	}
}
