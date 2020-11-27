using DirectOutput.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedControlToolkit
{
    class EditionTableTypeDescriptor : CustomTypeDescriptor
    {
        public Table EditionTable {get; private set;}

        private Dictionary<string, PropertyDescriptorHandler> PropertyDescriptors = new Dictionary<string, PropertyDescriptorHandler>();

        public EditionTableTypeDescriptor(Table Table)
            : base(TypeDescriptor.GetProvider(Table).GetTypeDescriptor(Table))
        {
            EditionTable = Table;
        }
    }
}
