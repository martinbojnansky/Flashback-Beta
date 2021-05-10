using Flashback.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Flashback.Views
{
    public sealed partial class TitlesView : UserControl
    {
        public ProjectViewModel ProjectViewModel = ProjectViewModel.Instance;

        public TitlesView()
        {
            this.InitializeComponent();
        }

        //public ObservableCollection<Color> Colors = new ObservableCollection<Color>()
        //{
        //    Color.FromArgb(255, 255, 255, 255),
        //    //Color.FromArgb(255, 236, 240, 241),
        //    //Color.FromArgb(255, 236, 240, 241),
        //    //Color.FromArgb(255, 149, 165, 166),
        //    //Color.FromArgb(255, 127, 140, 141),
        //    //Color.FromArgb(255, 52, 73, 94),
        //    //Color.FromArgb(255, 44, 62, 80),
        //    Color.FromArgb(255, 0, 0, 0),
        //    Color.FromArgb(255, 241, 196, 15),
        //    Color.FromArgb(255, 243, 156, 18),
        //    Color.FromArgb(255, 230, 126, 34),
        //    Color.FromArgb(255, 211, 84, 0),
        //    Color.FromArgb(255, 231, 76, 60),
        //    Color.FromArgb(255, 192, 57, 43),
        //    Color.FromArgb(255, 155, 89, 182),
        //    Color.FromArgb(255, 155, 89, 182),
        //    Color.FromArgb(255, 52, 152, 219),
        //    Color.FromArgb(255, 41, 128, 185),
        //    Color.FromArgb(255, 26, 188, 156),
        //    Color.FromArgb(255, 22, 160, 133),
        //    Color.FromArgb(255, 46, 204, 113),
        //    Color.FromArgb(255, 39, 174, 96),
        //};
    }
}
