﻿using DirectOutput;
using DirectOutput.Cab;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Toys;
using DirectOutput.Cab.Toys.Hardware;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.Cab.Toys.LWEquivalent;
using DirectOutput.LedControl.Loader;
using DofConfigToolWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    public class RGBSplitAnalogAlphaToy : AnalogAlphaToy
    {
        public override void UpdateOutputs()
        {
            if (Layers.Count == 0) return;

            base.UpdateOutputs();
        }
    }

    public class DirectOutputPreviewController : OutputControllerCompleteBase
    {
        public DofConfigToolSetup DofSetup { get; set; }
        public DirectOutputViewSetup DofViewSetup { get; set; }

        public class OutputRemap
        {
            public int OutputIndex { get; set; } = -1;
            public int OutputSize { get; set; } = -1;
            public int LedWizNum { get; set; } = -1;
            public DofConfigToolOutputEnum DofOutput { get; set; } = DofConfigToolOutputEnum.Invalid;
            public IToy Toy { get; set; } = null;
            public List<IToy> SplitToys { get; set; } = new List<IToy>();
        }

        public class AreaMapping
        {
            public DirectOutputViewAreaUpdatable Area { get; set; } = null;
            public List<OutputRemap> OutputMappings = new List<OutputRemap>();
        }

        private List<AreaMapping> AreaMappings = new List<AreaMapping>();
        private List<OutputRemap> OutputMappings = new List<OutputRemap>();

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
            p.Cabinet = new Cabinet();

            Outputs = new OutputList();
            AreaMappings.Clear();

            int firstAdressableLedNumber = 1;

            //First pass create Ledwiz Equivalent & adressable outputs, they've to be first
            foreach (var controller in DofSetup.ControllerSetups) {
                var LWE = new LedWizEquivalent() { Name = $"{controller.Name} Equivalent", LedWizNumber = controller.Number };

                for (var i = 0; i < controller.OutputMappings.Count; ++i) {
                    var o = controller.OutputMappings[i];

                    //It's an MX output, find MX Area attached and create a ledstrip
                    if (o.Output > DofConfigToolOutputEnum.RGBMXOutputs_Start && o.Output < DofConfigToolOutputEnum.RGBMXOutputs_End) {
                        var areas = DofViewSetup.GetViewAreas<DirectOutputViewAreaRGB>(A => A.Enabled && A is DirectOutputViewAreaUpdatable && (A as DirectOutputViewAreaUpdatable).HasOutput(o.Output))
                                                .Where(A=>A.ValueType == DirectOutputViewAreaRGB.ValueTypeEnum.Adressable)
                                                .ToArray();

                        if (areas != null && areas.Length > 0) {
                            var biggestArea = areas.FirstOrDefault(A=>A.OutputSize == areas.Max(area => area.OutputSize));

                            if (biggestArea != null) {
                                var ledstrip = new LedStrip() {
                                    ColorOrder = DirectOutput.Cab.Toys.Layer.RGBOrderEnum.RGB,
                                    FadingCurveName = "SwissLizardsLedCurve",
                                    Name = $"{o.Output} (Mx)",
                                    OutputControllerName = this.Name,
                                    LedStripArrangement = DirectOutput.Cab.Toys.Layer.LedStripArrangementEnum.LeftRightTopDown,
                                    Width = biggestArea.MxWidth,
                                    Height = biggestArea.MxHeight,
                                    FirstLedNumber = firstAdressableLedNumber
                                };

                                firstAdressableLedNumber += biggestArea.MxWidth * biggestArea.MxHeight;
                                p.Cabinet.Toys.Add(ledstrip);

                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = ledstrip.Name, LedWizEquivalentOutputNumber = o.PortNumber };
                                LWE.Outputs.Add(LWEO);
                                Outputs.Add(new Output() { Name = LWEO.OutputName, Number = NbOutputs + 1, Value = 0 });

                                var outputRemap = new OutputRemap() { Toy = ledstrip, LedWizNum = LWE.LedWizNumber, DofOutput = o.Output, OutputIndex = NbOutputs, OutputSize = ledstrip.Width * ledstrip.Height * o.PortRange };
                                OutputMappings.Add(outputRemap);
                                foreach(var area in areas) {
                                    AddAreaMapping(area, outputRemap);
                                }

                                NbOutputs += biggestArea.OutputSize;
                            }
                        }
                    } else {

                        //Create Toy regarding the Output range
                        IToy toy = null;
                        List<IToy> splitToys = new List<IToy>();
                        switch (o.PortRange) {
                            case 1: {
                                toy = new AnalogAlphaToy() {
                                    Name = $"{o.Output} (Analog)",
                                    OutputName = $"{LWE.Name}.{o.PortNumber:00}"
                                };
                                p.Cabinet.Toys.Add(toy);
                                break;
                            }

                            case 3: {
                                toy = new RGBAToy() {
                                    Name = $"{o.Output} (RGB)",
                                    OutputNameRed = $"{LWE.Name}.{o.PortNumber:00}",
                                    OutputNameGreen = $"{LWE.Name}.{o.PortNumber+1:00}",
                                    OutputNameBlue = $"{LWE.Name}.{o.PortNumber+2:00}"
                                };
                                p.Cabinet.Toys.Add(toy);

                                //Create 3 analog Toys for rgbsplit
                                splitToys.Add(new RGBSplitAnalogAlphaToy() { Name = $"{o.Output} rgbsplit Red (Analog)", OutputName = $"{LWE.Name}.{o.PortNumber:00}" });
                                splitToys.Add(new RGBSplitAnalogAlphaToy() { Name = $"{o.Output} rgbsplit Green (Analog)", OutputName = $"{LWE.Name}.{o.PortNumber + 1:00}" });
                                splitToys.Add(new RGBSplitAnalogAlphaToy() { Name = $"{o.Output} rgbsplit Blue (Analog)", OutputName = $"{LWE.Name}.{o.PortNumber + 2:00}" });
                                p.Cabinet.Toys.AddRange(splitToys);
                                break;
                            }

                            default: {
                                break;
                            }
                        }

                        for (var num = o.PortNumber; num < o.PortNumber + o.PortRange; ++num) {
                            var outputName = $"{LWE.Name}.{num:00}";
                            LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = $"{outputName}", LedWizEquivalentOutputNumber = num };
                            LWE.Outputs.Add(LWEO);
                            Outputs.Add(new Output() { Name = outputName, Number = NbOutputs + num + 1 - o.PortNumber, Value = 0 });
                        }

                        if (toy != null) {
                            var outputRemap = new OutputRemap() { Toy = toy, LedWizNum = LWE.LedWizNumber, DofOutput = o.Output, OutputIndex = NbOutputs, OutputSize = o.PortRange };
                            outputRemap.SplitToys.AddRange(splitToys);
                            OutputMappings.Add(outputRemap);

                            var matchingAreas = DofViewSetup.GetViewAreas<DirectOutputViewAreaUpdatable>(A => A is DirectOutputViewAreaUpdatable && (A as DirectOutputViewAreaUpdatable).HasOutput(o.Output));
                            if (matchingAreas.Length > 0) {
                                foreach (var area in matchingAreas) {
                                    AddAreaMapping(area, outputRemap);
                                }
                            }
                        }

                        NbOutputs += o.PortRange;
                    }

                }
                p.Cabinet.Toys.Add(LWE);
            }

            p.Cabinet.Toys.Add(DummyAnalogToy);
            p.Cabinet.Toys.Add(DummyRGBAToy);
            p.Cabinet.Toys.Add(DummyMatrixToy);

            p.Cabinet.OutputControllers.Add(this);
        }

        private void AddAreaMapping(DirectOutputViewAreaUpdatable area, OutputRemap outputRemap)
        {
            if (!AreaMappings.Any(AM=>AM.Area == area)) {
                AreaMappings.Add(new AreaMapping() { Area = area });
            }
            AreaMappings.First(AM=>AM.Area == area).OutputMappings.Add(outputRemap);
        }

        public OutputRemap[] GetToyOutputRemaps(Func<OutputRemap, bool> Match)
        {
            return OutputMappings.Where(M => Match(M)).ToArray();
        }

        public AreaMapping[] GetToyAreaMappings(Func<AreaMapping, bool> Match)
        {
            return AreaMappings.Where(AM => Match(AM)).ToArray();
        }

        private bool Connected = false;

        protected override void ConnectToController()
        {
            Connected = true;
        }

        protected override void DisconnectFromController()
        {
            Connected = false;
        }

        protected override int GetNumberOfConfiguredOutputs()
        {
            return NbOutputs;
        }

        protected override void UpdateOutputs(byte[] OutputValues)
        {
            if (!Connected) return;

            try {
                bool needRefresh = false;
                foreach (var mapping in AreaMappings) {
                    mapping.Area.StartUpdate();
                    foreach (var outputRemap in mapping.OutputMappings) {
                        var pValues = OutputValues.Skip(outputRemap.OutputIndex).Take(outputRemap.OutputSize).ToArray();
                        needRefresh |= mapping.Area.SetValues(pValues);
                    }
                }
                if (needRefresh && Refresh != null) {
                    Refresh.Invoke();
                }
            } catch  {} 
        }

        protected override bool VerifySettings()
        {
            return true;
        }

        private DirectOutputToolkitDummyAnalogToy DummyAnalogToy = new DirectOutputToolkitDummyAnalogToy();
        private DirectOutputToolkitDummyRGBAToy DummyRGBAToy = new DirectOutputToolkitDummyRGBAToy();
        private DirectOutputToolkitDummyMatrixToy DummyMatrixToy = new DirectOutputToolkitDummyMatrixToy();

        internal IToy GetCompatibleDummyToy(TableConfigSetting TCS)
        {
            if (TCS.IsArea) {
                return DummyMatrixToy;
            } else {
                if (TCS.OutputType == OutputTypeEnum.AnalogOutput) {
                    return DummyAnalogToy;
                } else {
                    return DummyRGBAToy;
                }
            }
        }

    }
}
