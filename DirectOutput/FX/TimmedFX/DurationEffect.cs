﻿using System.Xml.Serialization;

namespace DirectOutput.FX.TimmedFX
{
    /// <summary>
    /// Duration effect which triggers a specified target effect for a specified duration.<br/>
    /// When this effect is triggered it triggers the target effect immediately with the same data it has received. After the specified duration it calls trigger on the target effect again with data for the same table elmenet, but with the value changed to 0.<br/>
    /// \image html FX_Duration.png "Duration effect"
    /// </summary>
    public class DurationEffect : EffectEffectBase
    {
        private RetriggerBehaviourEnum _RetriggerBehaviour = RetriggerBehaviourEnum.Restart;

        /// <summary>
        /// Gets or sets the retrigger behaviour.<br/>
        /// The setting defines the behaviour of the effect if it is retriggered while it is still active. <br/>
        /// This settings is only relevant, if the effect can be called from more than one table element.
        /// </summary>
        /// <value>
        /// Valid values are Restart (Restarts the duration) or Ignore (keeps the org duration).
        /// </value>
        public RetriggerBehaviourEnum RetriggerBehaviour
        {
            get { return _RetriggerBehaviour; }
            set { _RetriggerBehaviour = value; }
        }

        private int _DurationMs = 500;

        /// <summary>
        /// Gets or sets the duration for the effect in milliseconds.
        /// </summary>
        /// <value>
        /// The effect duration in milliseconds.
        /// </value>
        public int DurationMs
        {
            get { return _DurationMs; }
            set { _DurationMs = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="DurationEffect"/> is currently active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise <c>false</c>.
        /// </value>
        [XmlIgnoreAttribute]
        public bool Active { get; private set; }

        /// <summary>
        /// Triggers the DurationEffect with the given TableElementData.<br/>
        /// The duration is started, if the value portion of the TableElementData parameter is !=0. 
        /// Trigger calls with a TableElement value=0 have no effect.
        /// </summary>
        /// <param name="TableElementData">TableElementData for the TableElement which has triggered the effect.</param>
        public override void Trigger(Table.TableElementData TableElementData)
        {
            if (TargetEffect != null && TableElementData.Value != 0)
            {
                if (!Active)
                {
                   TriggerTargetEffect(TableElementData);
                    Table.Pinball.Alarms.RegisterAlarm(DurationMs, DurationEnd, TableElementData);
                    Active = true;
                } else if(RetriggerBehaviour==RetriggerBehaviourEnum.Restart) {
                    Table.Pinball.Alarms.RegisterAlarm(DurationMs, DurationEnd, TableElementData);
                }
            }
        }


        private void DurationEnd(object TableElementData)
        {

            Table.TableElementData TED = (Table.TableElementData)TableElementData;
            TED.Value = 0;
            TriggerTargetEffect(TED);
            Active = false;
        }

        /// <summary>
        /// Finishes the DurationEffect.
        /// </summary>
        public override void Finish()
        {
            try
            {
                Table.Pinball.Alarms.UnregisterAlarm(DurationEnd);

            }
            catch { }
            Active = false;
            base.Finish();
        }

    }

    /// <summary>
    /// BlinkDurationEffect are basically DurationEffect which was setup from blink TableConfigSetting data.<br/>
    /// It is used to recreate TableConfigSetting from effect (used in DirectOutput Toolkit)
    /// </summary>
    public class BlinkDurationEffect : DurationEffect
    {
        [XmlIgnoreAttribute]
        public int Blink = 0;
        [XmlIgnoreAttribute]
        public int BlinkInterval = 0;
    }
}
