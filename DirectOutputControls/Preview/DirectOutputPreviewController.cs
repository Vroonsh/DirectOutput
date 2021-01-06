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
        public DirectOutputPreviewControl PreviewControl { get; set; }

        private int NbOutputs = 0;

        public void Setup(Pinball p)
        {
            if (DofSetup == null) {
                throw new Exception("Cannot setup DirectOutputPreviewController, no valid DofConfigToolSetup set.");
            }

            if (PreviewControl == null || PreviewControl.DirectOutputViewSetup == null) {
                throw new Exception("Cannot setup DirectOutputPreviewController, no valid DirectOutputPreviewControl set.");
            }

            NbOutputs = 0;

            //Reset Cabinet
            p.Finish();
            p.Cabinet.Clear(true);

            int firstAdressableLedNumber = 1;

            //LedWizEquivalentOutputNumber is PortNum

            //Create Ledwiz Equivalent & outputs
            foreach (var controller in DofSetup.ControllerSetups) {
                var LWE = new LedWizEquivalent() { Name = $"{controller.Name} Equivalent", LedWizNumber = controller.Number };
                for (var i = 0; i < controller.OutputMappings.Count; ++i) {
                    var o = controller.OutputMappings[i];

                    for (var num = o.PortNumber; num < o.PortNumber + o.PortRange; ++num) {
                        LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(LWE.Name, num), LedWizEquivalentOutputNumber = num };
                        LWE.Outputs.Add(LWEO);
                        NbOutputs++;
                    }

                    //It's an MX output, find MX Area attached and create a ledstrip
                    if (o.Output > DofConfigToolOutputEnum.RGBMXOutputs_Start && o.Output < DofConfigToolOutputEnum.RGBMXOutputs_End) {
                        var areas = PreviewControl.DirectOutputViewSetup.GetViewAreas<DirectOutputViewAreaRGB>(o.Output);
                        if (areas != null && areas.Length == 1) {
                            var area = areas[0];
                            if (area.ValueType == DirectOutputViewAreaRGB.ValueTypeEnum.Adressable) {
                                var ledstrip = new LedStrip() {
                                    ColorOrder = DirectOutput.Cab.Toys.Layer.RGBOrderEnum.RBG,
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

                                //Replace nb outputs for this toy by the real matrix size * 3
                                NbOutputs -= o.PortRange;
                                NbOutputs += area.MxWidth * area.MxHeight * o.PortRange;
                            }
                        }
                    } 
                }

                p.Cabinet.Toys.Add(LWE);
            }

            p.Cabinet.OutputControllers.Add(this);

            p.Init();
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
        }

        protected override bool VerifySettings()
        {
            return true;
        }

    }
}
