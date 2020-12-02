using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using DirectOutput.Cab.Toys;
using DirectOutput.General.Analog;

namespace DirectOutput.FX.AnalogToyFX
{
    /// <summary>
    /// Base class for effects controlling a single AnalogToy object.
    /// </summary>
    public abstract class AnalogToyEffectBase : EffectBase
    {
        /// <summary>
        /// The table which contains the effect.
        /// </summary>
        protected Table.Table Table;

        private string _ToyName;

        /// <summary>
        /// Name of the AnalogToy.
        /// </summary>
        /// <value>
        /// The name of the AnalogToy.
        /// </value>
        public string ToyName
        {
            get { return _ToyName; }
            set
            {
                if (_ToyName != value)
                {
                    _ToyName = value;
                    Toy = null;
                    Layer = null;
                }
            }
        }

        protected AnalogAlpha Layer { get; private set; }

        private bool _FixedLayerNr = false;
        /// <summary>
        /// Gets or sets if the LayerNr has been set by the TableConfigSetting
        /// </summary>
        /// <value>
        /// true is the LayerNr has been set by the TableConfigSetting
        /// </value>
        public bool FixedLayerNr
        {
            get { return _FixedLayerNr; }
            set { _FixedLayerNr = value; }
        }

        private int _LayerNr = 0;

        /// <summary>
        /// Gets or sets the number of the layer for the AnalogToy effect (Default=0).
        /// </summary>
        /// <value>
        /// The layer number.
        /// </value>
        public int LayerNr
        {
            get { return _LayerNr; }
            set { _LayerNr = value; }
        }




        /// <summary>
        /// Refrence to the AnalogToyy specified in the ToyName property.<br/>
        /// If the ToyName property is empty or contains a unknown name or the name of a toy which is not a AnalogToy this property will return null.
        /// </summary>
        [XmlIgnoreAttribute]
        protected IAnalogAlphaToy Toy{get;private set;}

        private void ResolveName(Table.Table Table)
        {


        }

        /// <summary>
        /// Initializes the effect.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public override void Init(Table.Table Table)
        {
            if (!ToyName.IsNullOrWhiteSpace() && Table.Pinball.Cabinet.Toys.Contains(ToyName))
            {
                if (Table.Pinball.Cabinet.Toys[ToyName] is IAnalogAlphaToy)
                {
                    Toy = (IAnalogAlphaToy)Table.Pinball.Cabinet.Toys[ToyName];
                    Layer = Toy.Layers[LayerNr];
                }

            }
            this.Table = Table;
            ResolveName(Table);
        }

        /// <summary>
        /// Finish does all necessary cleanupwork before the effect is discarded.
        /// </summary>
        public override void Finish()
        {
           
            Toy = null;
            this.Table = null;
            base.Finish();
        }

        /// <summary>
        /// Returns Toy impacted by this effect
        /// </summary>
        /// <returns>the assigned toy</returns>
        public override IToy GetAssignedToy() => Toy;

        /// <summary>
        /// Set the assigned toy to an effect
        /// </summary>
        /// <param name="toy">the assigned toy</param>
        public override void SetAssignedToy(IToy toy)
        {
            if (toy is IAnalogAlphaToy) {
                ToyName = toy.Name;
                Toy = (IAnalogAlphaToy)toy;
            }
        }

        /// <summary>
        /// Will tell if this effect or any targeted effects have an action on the provided toys list
        /// </summary>
        /// <param name="ToyNames">a list of toy names</param>
        /// <returns>true if any effect in the chain is acting on at least one of the provided toys</returns>
        public override bool ActOnAnyToys(IEnumerable<string> ToyNames)
        {
            return ToyNames.Any(N => N.Equals(_ToyName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
