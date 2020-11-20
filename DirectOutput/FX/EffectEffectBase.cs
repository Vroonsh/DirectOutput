using System.Xml.Serialization;
using DirectOutput.Table;
using System;
using System.Collections.Generic;

namespace DirectOutput.FX
{
    /// <summary>
    /// Base class for effects targeting another effect.
    /// </summary>
    public abstract class EffectEffectBase : EffectBase
    {
        #region EffectName
        private string _TargetEffectName;
        /// <summary>
        /// Name of the target effect.<br/>
        /// Triggers EffectNameChanged if value is changed.
        /// </summary>    
        public string TargetEffectName
        {
            get { return _TargetEffectName; }
            set
            {
                if (_TargetEffectName != value)
                {
                    _TargetEffectName = value;
                    TargetEffect = null;

                }
            }
        }




        #endregion



        #region Effect
        private IEffect _TargetEffect;
        /// <summary>
        /// TargetEffect for the effect (ReadOnly).<br/>
        /// The property is resolved from the TargetEffectName. If TargetEffectName is empty or unknown this property will return null.
        /// </summary>
        [XmlIgnoreAttribute]
        public IEffect TargetEffect
        {
            get
            {
                return _TargetEffect;
            }
            private set
            {
                _TargetEffect = value;
            }
        }

        /// <summary>
        /// Triggers the target effect.<br/>
        /// The method will deactivate the target effect if it throws a exception.
        /// </summary>
        /// <param name="TriggerData">The trigger data for the target effect.</param>
        protected void TriggerTargetEffect(TableElementData TriggerData)
        {
            if (TargetEffect != null)
            {
                try
                {
                    TargetEffect.Trigger(TriggerData);
                }
                catch (Exception E)
                {
                    Log.Exception("The target effect {0} of the {1} {2} has thrown a exception. Disabling further calls of the target effect.".Build(TargetEffectName, GetType().Name, Name), E);
                    TargetEffect = null;
                }
            }

        }


        private void ResolveEffectName(Table.Table Table)
        {
            if (!TargetEffectName.IsNullOrWhiteSpace() && Table.Effects.Contains(TargetEffectName))
            {
                TargetEffect = Table.Effects[TargetEffectName];
            };

        }

        #endregion

        /// <summary>
        /// Gets the table object which is hosting the effect.
        /// </summary>
        /// <value>
        /// The table object which is hosting the effect.
        /// </value>
        protected Table.Table Table { get; private set; }

        /// <summary>
        /// Initializes the EffectEffect. <br/>
        /// Resolves the name of the TargetEffect.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            this.Table = Table;
            ResolveEffectName(Table);
        }

        /// <summary>
        /// Finishes the EffectEffect.<br/>
        /// Releases the references to the target effect and to the table object.
        /// </summary>
        public override void Finish()
        {
            TargetEffect = null;
            Table = null;
            base.Finish();
        }

        /// <summary>
        /// Will tell if this effect or any targeted effects have an action on the provided toys list
        /// </summary>
        /// <param name="ToyNames">a list of toy names</param>
        /// <returns>true if any effect in the chain is acting on at least one of the provided toys</returns>
        /// <remarks>only pass through to the targeted effect</remarks>
        public override bool ActOnAnyToys(IEnumerable<string> ToyNames)
        {
            if (TargetEffect != null) {
                return TargetEffect.ActOnAnyToys(ToyNames);
            }
            return base.ActOnAnyToys(ToyNames);
        }
    }
}
