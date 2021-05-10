using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashback.Effects
{
    public class EffectReferenceCategory
    {
        public EffectReferenceCategory(string title, ObservableCollection<EffectReference> effectReferences)
        {
            Title = title;
            EffectReferences = effectReferences;
        }

        public string Title { get; }
        public ObservableCollection<EffectReference> EffectReferences { get; }
    }
}
