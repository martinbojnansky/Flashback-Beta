using Flashback.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Flashback.Effects
{
    public class EffectReference
    {
        public EffectReference(Type effectType)
        {
            _effectType = effectType;

            // Set saved effect settings or create new one if not available
            try
            {
                Effect = ProjectViewModel.Instance.Project.Effects[_effectType.FullName];
            }
            catch
            {
                Effect = (Effect)Activator.CreateInstance(_effectType);
                ProjectViewModel.Instance.Project.Effects.Add(_effectType.FullName, Effect);
            }
        }

        private Type _effectType;

        public Effect Effect { get; }
    }
}
