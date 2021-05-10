using Flashback.Effects;
using Flashback.Effects.Adjustments;
using Flashback.Effects.Borders;
using Flashback.Effects.Filters;
using Helpers.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashback.ViewModels
{
    public partial class ProjectViewModel : ViewModelBase
    {
        public ObservableCollection<EffectReferenceCategory> EffectReferenceCategories;

        /// <summary>
        /// Create effect categories and references.
        /// </summary>
        /// <returns></returns>
        public async Task CreateEffectReferenceCategories()
        {
            await Task.Run(() =>
            {
                EffectReferenceCategories = new ObservableCollection<EffectReferenceCategory>()
                {
                    // Filters
                    new EffectReferenceCategory("Filters", new ObservableCollection<EffectReference>()
                    {
                        new EffectReference(typeof(NormalEffect)),
                        new EffectReference(typeof(BoostEffect)),
                        new EffectReference(typeof(XproEffect)),
                        new EffectReference(typeof(HipsterEffect)),
                        new EffectReference(typeof(InkwellEffect)),
                        new EffectReference(typeof(MonoEffect)),
                        new EffectReference(typeof(DreamEffect)),
                        //new EffectReference(typeof(AutoFixEffect)),
                    }),
                    // Adjustments
                    new EffectReferenceCategory("Adjustments", new ObservableCollection<EffectReference>()
                    {
                        new EffectReference(typeof(BrightnessEffect)),
                        new EffectReference(typeof(ContrastEffect)),
                        new EffectReference(typeof(SaturationEffect)),
                        new EffectReference(typeof(TemperatureAndTintEffect)),
                        new EffectReference(typeof(HighlightsAndShadowsEffect)),
                        new EffectReference(typeof(SharpnessEffect)),
                        //new EffectReference(typeof(FlareEffect))
                        //new EffectReference(typeof(StabilizationEffect))
                    }),
                    // Borders
                    new EffectReferenceCategory("Borders", new ObservableCollection<EffectReference>()
                    {
                        new EffectReference(typeof(VignetteEffect))
                    })
                };
            });
        }
    }
}
