using DirectOutput.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DofConfigToolWrapper
{
    /// <summary>
    /// The DofConfigToolSetup is used by the <see cref="DofConfigToolWrapper"/> to retrieve the generated config files using the APIKey.
    /// It's also used by other classes to retrieve the output mappings for proper redirection.
    /// This setup is not generated from DofConfigTool site, you'll have to create it to match the one you're using with your generated dof files.
    /// </summary>
    public class DofConfigToolSetup : XmlSerializable<DofConfigToolSetup>
    {
        /// <summary>
        /// This mapping represent one entry in your Port assignation page on DofConfigTool site.
        /// </summary>
        public class OutputMapping
        {
            /// <summary>
            /// Unique Name for this mapping
            /// </summary>
            public string Name { get; set; } = string.Empty;
            /// <summary>
            /// The port number, has to be at least 1
            /// </summary>
            public int PortNumber { get; set; } = 1;

            public int PortRange => Output == DofConfigToolOutputEnum.Invalid ? 0 :
                                    (Output > DofConfigToolOutputEnum.AnalogOutputs_Start && Output < DofConfigToolOutputEnum.AnalogOutputs_End) ? 1 : 3;

            /// <summary>
            /// The output assigned to this port from all available Output types declared in DofConfigTool
            /// </summary>
            public DofConfigToolOutputEnum Output { get; set; } = DofConfigToolOutputEnum.Invalid;
        }

        /// <summary>
        /// A ControllerSetup represent one of the card you've declared in your DofConfigTool account
        /// </summary>
        public class ControllerSetup
        {
            /// <summary>
            /// This name is just there to ease your setup
            /// </summary>
            public string Name { get; set; } = string.Empty;
            /// <summary>
            /// This number has to match the one assigned to your DofConfigTool declared cards (e.g. 51 for first Pinscape usually)
            /// </summary>
            public int Number { get; set; } = 0;
            /// <summary>
            /// List of the ports you declared on this controller.
            /// Combos are supported by declaring several OutputMapping with the same PortNumber but different Output
            /// </summary>
            public List<OutputMapping> OutputMappings { get; set; } = new List<OutputMapping>();

            public override string ToString() => $"{Name} [{Number}]";
        }

        /// <summary>
        /// The user name you used for this setup on the site, doesn't need to match it's pure information
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// The Api key will allow you to synchronize with your latest generated dof files on some tools using DofConfigToolSetup
        /// </summary>
        public string APIKey { get; set; } = string.Empty;

        /// <summary>
        /// List of the controllers used by this setup.
        /// </summary>
        public List<ControllerSetup> ControllerSetups { get; private set; } = new List<ControllerSetup>();

        /// <summary>
        /// Will validate the whole setup and eventually shows a messagebox listing errors & warnings
        /// </summary>
        /// <param name="silent">if true, no messagebox is shown</param>
        /// <returns>false if there are errors, even in silent mode</returns>
        public bool Validate(bool silent = false)
        {
            string Errors = string.Empty;
            string Warnings = string.Empty;

            if (UserName.IsNullOrEmpty()) {
                Warnings += "UserName has not been set. It helps to identify your setup\n";
            }

            if (APIKey.IsNullOrEmpty()) {
                Warnings += "APIKey has not been set. You won't be able to retrieve your DofConfigTool settings from the online tool\n";
            }

            List<int> duplicateControllerNum = new List<int>();
            List<string> duplicateControllerName = new List<string>();
            foreach (var controller in ControllerSetups) {

                if (!duplicateControllerNum.Contains(controller.Number) && ControllerSetups.Any(C=> C != controller && C.Number == controller.Number)) {
                    duplicateControllerNum.Add(controller.Number);
                    Errors += $"Duplicate controller number {controller.Number}.\n";
                }

                if (!duplicateControllerName.Contains(controller.Name) && ControllerSetups.Any(C => C != controller && C.Name.Equals(controller.Name, StringComparison.InvariantCultureIgnoreCase))) {
                    duplicateControllerName.Add(controller.Name);
                    Warnings += $"Duplicate controller name {controller.Name}.\n";
                }

                for(var num = 0; num < controller.OutputMappings.Count; ++num) {
                    var mapping = controller.OutputMappings[num];

                    if (mapping.Output == DofConfigToolOutputEnum.Invalid) {
                        Errors += $"controller [{controller}] mapping {num} is Invalid type.\n";
                    }else if (controller.OutputMappings.Any(M=>M != mapping && M.Output == mapping.Output)) {
                        Warnings += $"controller [{controller}] mapping {num} has duplicated Output {mapping.Output}, it's supported but could be a setup error.\n";
                    }

                    if (mapping.PortNumber <= 0) {
                        Errors += $"controller [{controller}] mapping {num} has invalid PortNumber {mapping.PortNumber} (need to be at least 1).\n";
                    }
                    var dupPortNum = controller.OutputMappings.FirstOrDefault(M => M != mapping && M.PortNumber == mapping.PortNumber);
                    if (dupPortNum != null && dupPortNum.Output == mapping.Output) {
                        Errors += $"controller [{controller}] mapping {num} has duplicated PortNumber {mapping.PortNumber} with same output {mapping.Output}.\n";
                    }
                }
            }

            if (!silent) {
                if (!Warnings.IsNullOrEmpty() || !Errors.IsNullOrEmpty()) {
                    string message = "DofConfigToolSetup validation :\n\n";
                    if (!Errors.IsNullOrEmpty()) {
                        message += "Errors:\n" + Errors;
                    }
                    if (!Warnings.IsNullOrEmpty()) {
                        message += "Warnings:\n" + Warnings;
                    }
                    MessageBox.Show(message, "DofConfigToolSetup validation", MessageBoxButtons.OK, Errors.IsNullOrEmpty() ? MessageBoxIcon.Warning : MessageBoxIcon.Error);
                }
            }

            return Errors.IsNullOrEmpty();
        }
    }
}
