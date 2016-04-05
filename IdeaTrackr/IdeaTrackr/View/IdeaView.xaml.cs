using IdeaTrackr.Model;
using IdeaTrackr.ViewModel;

using Xamarin.Forms;

namespace IdeaTrackr.View
{
    public partial class IdeaView : ContentPage
    {
        public IdeaView() : this(new Idea())
        {
        }

        public IdeaView(Idea idea)
        {
            InitializeComponent();

            var _viewModel = new IdeaViewModel(Navigation, idea);
            BindingContext = _viewModel;


            ToolbarItems.Add(new ToolbarItem("Save", null,
                async () => await _viewModel.SaveIdeaAsync(), ToolbarItemOrder.Primary));
        }
    }
}
