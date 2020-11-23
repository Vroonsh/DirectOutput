using System.Collections.Generic;
using DirectOutput.General.Generic;
using DirectOutput.LedControl.Loader;
using DirectOutput.Table;

namespace DirectOutput.FX
{

    /// <summary>
    /// Abstract base class for IEffect objects.<br/>
    /// This class inherits NamedItemBase and implements IEffect.
    /// </summary>
    public abstract class EffectBase : NamedItemBase,IEffect
    {


        /// <summary>
        /// Triggers the effect with the given TableElementData.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public abstract void Trigger(TableElementData TableElementData);


        /// <summary>
        /// Init does all necessary initialization work after the effect object has been instanciated.
        /// </summary>
        /// <param name="Table">Table object containing the effect.</param>
        public abstract void Init(Table.Table Table);


        /// <summary>
        /// Finish does all necessary cleanupwork before the object is discarded.
        /// </summary>
        public virtual void Finish() {}

        /// <summary>
        /// Will tell if this effect or any targeted effects have an action on the provided toys list
        /// </summary>
        /// <param name="ToyNames">a list of toy names</param>
        /// <returns>true if any effect in the chain is acting on at least one of the provided toys</returns>
        public virtual bool ActOnAnyToys(IEnumerable<string> ToyNames) => false;

        /// <summary>
        /// Will populate a List with all effects from an effect hierarchy
        /// </summary>
        /// <param name="Effects">a List of effets to fill</param>
        public virtual void GetAllEffects(List<IEffect> Effects)
        {
            Effects.Add(this);
        }
    }
}
