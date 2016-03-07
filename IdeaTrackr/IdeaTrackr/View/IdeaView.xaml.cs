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

            BindingContext = new IdeaViewModel(Navigation, idea);
        }
    }
}
