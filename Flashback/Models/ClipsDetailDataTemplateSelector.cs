using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Flashback.Models
{
    public class ClipsDetailDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is Clip)
            {
                var type = item.GetType();
                if (type == typeof(VideoClip))
                    return element.Resources["VideoClipDataTemplate"] as DataTemplate;
                else if (type == typeof(SlideshowClip))
                    return element.Resources["SlideshowClipDataTemplate"] as DataTemplate;
            }

            return null;

        }
    }
}
