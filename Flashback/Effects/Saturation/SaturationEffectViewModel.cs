using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace Flashback.Effects.Saturation
{
    [DataContract]
    public class SaturationEffectViewModel : EffectViewModel
    {
        public SaturationEffectViewModel(EffectReference effect): base(effect) { }

        [IgnoreDataMember]
        public double Saturation
        {
            get
            {
                return Convert.ToDouble(EffectReference.Properties[nameof(Saturation)]);
            }
            set
            {
                EffectReference.Properties[nameof(Saturation)] = value;
                RaisePropertyChanged(nameof(Saturation));
            }
        }
    }
}
