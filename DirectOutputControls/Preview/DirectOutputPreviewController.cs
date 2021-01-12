using DirectOutput;
using DirectOutput.Cab;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Toys.Hardware;
using DirectOutput.Cab.Toys.LWEquivalent;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    public class DirectOutputPreviewController : OutputControllerCompleteBase
    {
        public DofConfigToolSetup DofSetup { get; set; }
        public DirectOutputViewSetup DofViewSetup { get; set; }

        class OutputRemap
        {
            public int OutputIndex { get; set; } = -1;
            public int OutputSize { get; set; } = -1;
            public int LedWizNum { get; set; } = -1;
            public DirectOutputViewArea Area { get; set; } = null;
        }

        List<OutputRemap> OutputMappings = new List<OutputRemap>();

        public Action Refresh = null;

        private int NbOutputs = 0;

        public void Setup(Pinball p)
        {
            if (DofSetup == null) {
                throw new Exception("Cannot setup DirectOutputPreviewController, no valid DofConfigToolSetup set.");
            }

            if (DofViewSetup == null) {
                throw new Exception("Cannot setup DirectOutputPreviewController, no valid DirectOutputPreviewControl set.");
            }

            NbOutputs = 0;
            Name = "DirectOutputPreviewController";

            //Reset Cabinet
            p.Finish();
            p.Cabinet.Clear(true);

            Outputs = new OutputList();
            OutputMappings.Clear();

            int firstAdressableLedNumber = 1;

            //First pass create Ledwiz Equivalent & adressable outputs, they've to be first
            foreach (var controller in DofSetup.ControllerSetups) {
                var LWE = new LedWizEquivalent() { Name = $"{controller.Name} Equivalent", LedWizNumber = controller.Number };
                for (var i = 0; i < controller.OutputMappings.Count; ++i) {
                    var o = controller.OutputMappings[i];

                    //It's an MX output, find MX Area attached and create a ledstrip
                    if (o.Output > DofConfigToolOutputEnum.RGBMXOutputs_Start && o.Output < DofConfigToolOutputEnum.RGBMXOutputs_End) {
                        var areas = DofViewSetup.GetViewAreas<DirectOutputViewAreaRGB>(o.Output);
                        if (areas != null && areas.Length > 0) {
                            var area = areas[0];
                            if (area.ValueType == DirectOutputViewAreaRGB.ValueTypeEnum.Adressable) {
                                var ledstrip = new LedStrip() {
                                    ColorOrder = DirectOutput.Cab.Toys.Layer.RGBOrderEnum.RGB,
                                    FadingCurveName = "SwissLizardsLedCurve",
                                    Name = $"Ledstrip {p.Cabinet.Toys.Where(T => T is LedStrip).Count()}",
                                    OutputControllerName = this.Name,
                                    LedStripArrangement = DirectOutput.Cab.Toys.Layer.LedStripArrangementEnum.LeftRightTopDown,
                                    Width = area.MxWidth,
                                    Height = area.MxHeight,
                                    FirstLedNumber = firstAdressableLedNumber
                                };

                                firstAdressableLedNumber += area.MxWidth * area.MxHeight;
                                p.Cabinet.Toys.Add(ledstrip);

                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = ledstrip.Name, LedWizEquivalentOutputNumber = o.PortNumber };
                                LWE.Outputs.Add(LWEO);

                                Outputs.Add(new Output() { Name = LWEO.OutputName, Number = NbOutputs+1, Value = 0 });
                                var remap = new OutputRemap() { LedWizNum = LWE.LedWizNumber, Area = area, OutputIndex = NbOutputs, OutputSize = area.MxWidth * area.MxHeight * o.PortRange };
                                OutputMappings.Add(remap);
                                NbOutputs += remap.OutputSize;
                            }
                        }
                    }
                }
                p.Cabinet.Toys.Add(LWE);
            }

            //Second pass non Adressable outputs.
            foreach (var controller in DofSetup.ControllerSetups) {
                var LWE = p.Cabinet.Toys.OfType<LedWizEquivalent>().FirstOrDefault(T => T.LedWizNumber == controller.Number);
                if (LWE != null) {
                    for (var i = 0; i < controller.OutputMappings.Count; ++i) {
                        var o = controller.OutputMappings[i];

                        //It's an MX output, find MX Area attached and create a ledstrip
                        if (o.Output < DofConfigToolOutputEnum.RGBMXOutputs_Start || o.Output > DofConfigToolOutputEnum.RGBMXOutputs_End) {
                            for (var num = o.PortNumber; num < o.PortNumber + o.PortRange; ++num) {
                                var outputName = $"{LWE.Name}.{num:00}";
                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = $"{Name}\\{outputName}", LedWizEquivalentOutputNumber = num };
                                LWE.Outputs.Add(LWEO);
                                Outputs.Add(new Output() { Name = outputName, Number = NbOutputs + num + 1 - o.PortNumber, Value = 0 });
                            }

                            var matchingAreas = DofViewSetup.GetViewAreas<DirectOutputViewArea>(o.Output);
                            if (matchingAreas != null && matchingAreas.Length > 0) {
                                foreach (var area in matchingAreas) {
                                    var remap = new OutputRemap() { LedWizNum = LWE.LedWizNumber, Area = area, OutputIndex = NbOutputs, OutputSize = o.PortRange };
                                    OutputMappings.Add(remap);
                                }
                            }

                            NbOutputs += o.PortRange;
                        }
                    }
                }
            }

            p.Cabinet.OutputControllers.Add(this);
        }

        protected override void ConnectToController()
        {
        }

        protected override void DisconnectFromController()
        {
        }

        protected override int GetNumberOfConfiguredOutputs()
        {
            return NbOutputs;
        }

        protected override void UpdateOutputs(byte[] OutputValues)
        {
            bool needRefresh = false;
            foreach(var mapping in OutputMappings) {
                var pValues = OutputValues.Skip(mapping.OutputIndex).Take(mapping.OutputSize).ToArray();
                needRefresh |= mapping.Area.SetValues(pValues);
            }
            if (needRefresh && Refresh != null) {
                Refresh.Invoke();
            }
        }

        protected override bool VerifySettings()
        {
            return true;
        }

    }
}
