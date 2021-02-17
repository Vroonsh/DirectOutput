using DirectOutput.Cab;
using DirectOutput.Cab.Toys;
using DirectOutput.Cab.Toys.Layer;
using DirectOutput.General.Analog;
using DirectOutput.General.Color;
using DirectOutput.General.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectOutputControls
{
    class DirectOutputToolkitDummyMatrixToy : IMatrixToy<RGBAColor>
    {
        //IToy
        public string Name
        {
            get {
                return "DirectOutputToolkitDummyMatrixToy";
            }
            set {
                if (BeforeNameChanged != null) {
                    BeforeNameChanged.Invoke(this, new NameChangeEventArgs());
                }
                if (AfterNameChanged != null) {
                    AfterNameChanged.Invoke(this, new NameChangeEventArgs());
                }
            }
        }

        public void Init(Cabinet Cabinet) { }
        public void Finish() { }
        public void Reset() { }

        //IMatrixToy
        public RGBAColor[,] GetLayer(int LayerNr) => new RGBAColor[0, 0];
        public int Height => 0;
        public int Width => 0;

        //INamedItem
        public event EventHandler<NameChangeEventArgs> AfterNameChanged;
        public event EventHandler<NameChangeEventArgs> BeforeNameChanged;
    }

    class DirectOutputToolkitDummyAnalogToy : IAnalogAlphaToy
    {
        //IToy
        public string Name
        {
            get {
                return "DirectOutputToolkitDummyAnalogToy";
            }
            set {
                if (BeforeNameChanged != null) {
                    BeforeNameChanged.Invoke(this, new NameChangeEventArgs());
                }
                if (AfterNameChanged != null) {
                    AfterNameChanged.Invoke(this, new NameChangeEventArgs());
                }
            }
        }

        public void Init(Cabinet Cabinet) { }
        public void Reset() { }
        public void Finish() { }

        //IAnalogAlphaToy
        public LayerDictionary<AnalogAlpha> Layers => new LayerDictionary<AnalogAlpha>();

        //INamedItem
        public event EventHandler<NameChangeEventArgs> AfterNameChanged;
        public event EventHandler<NameChangeEventArgs> BeforeNameChanged;
    }

    class DirectOutputToolkitDummyRGBAToy : IRGBAToy
    {
        //IToy
        public string Name
        {
            get {
                return "DirectOutputToolkitDummyRGBAToy";
            }
            set {
                if (BeforeNameChanged != null) {
                    BeforeNameChanged.Invoke(this, new NameChangeEventArgs());
                }
                if (AfterNameChanged != null) {
                    AfterNameChanged.Invoke(this, new NameChangeEventArgs());
                }
            }
        }

        public void Init(Cabinet Cabinet) { }
        public void Reset() { }
        public void Finish() { }

        //IAnalogAlphaToy
        public LayerDictionary<RGBAColor> Layers => new LayerDictionary<RGBAColor>();

        //INamedItem
        public event EventHandler<NameChangeEventArgs> AfterNameChanged;
        public event EventHandler<NameChangeEventArgs> BeforeNameChanged;
    }

}
