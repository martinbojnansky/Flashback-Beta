using Windows.UI.Xaml.Controls;

namespace Helpers.Navigation
{
    public class NavigationLink
    {
        public NavigationLink() { }

        public NavigationLink(string label, Symbol symbol, Control control)
        {
            Label = label;
            Symbol = symbol;
            Control = control;
        }

        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public Control Control { get; set; }
    }
}
