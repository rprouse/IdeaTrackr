using IdeaTrackr.Helpers;
using IdeaTrackr.Model;
using IdeaTrackr.ViewModel;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace IdeaTrackr.View
{
    public partial class IdeaListView : ContentPage
    {
        IdeaListViewModel _viewModel;

        public IdeaListView()
        {
            InitializeComponent();

            BindingContext = _viewModel = new IdeaListViewModel(Navigation);

            string addIcon = null;
            switch (Device.OS)
            {
                case TargetPlatform.Android:
                    addIcon = "ic_menu_add";
                    break;
                case TargetPlatform.WinPhone:
                    addIcon = "add.png";
                    break;
            }
            ToolbarItems.Add(new ToolbarItem("Add", addIcon,
                async () => await _viewModel.AddIdea(), ToolbarItemOrder.Primary));


            ListViewIdeas.ItemTapped += async (sender, e) =>
            {
                await _viewModel.ShowIdeaView(e.Item as Idea);
            };

            if (Device.OS != TargetPlatform.iOS && Device.OS != TargetPlatform.Android)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Command = _viewModel.RefreshIdeas
                });
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;
            OfflineStack.IsVisible = !CrossConnectivity.Current.IsConnected;
            if (Settings.IsLoggedIn)
            {
                if (_viewModel.Ideas.Count == 0)
                    await _viewModel.LoadIdeasAsync();
                await _viewModel.RefreshIdeasAsync();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CrossConnectivity.Current.ConnectivityChanged -= ConnectivityChanged;
        }

        void ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                OfflineStack.IsVisible = !e.IsConnected;
            });
        }
    }
}
