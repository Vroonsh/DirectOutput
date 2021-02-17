using System.Collections.Generic;
using DirectOutput.Cab.Toys;
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
        /// Returns Toy impacted by this effect
        /// </summary>
        /// <returns>a toy name</returns>
        public virtual IToy GetAssignedToy() => null;

        /// <summary>
        /// Set the assigned toy to an effect
        /// </summary>
        /// <param name="toy">the assigned toy</param>
        public virtual void SetAssignedToy(IToy toy) { }

        /// <summary>
        /// Will populate a List with all effects from an effect hierarchy
        /// </summary>
        /// <returns>an array containing all effects</returns>
        public virtual IEffect[] GetAllEffects()
        {
            return new IEffect[] { this };
        }
    }
}
