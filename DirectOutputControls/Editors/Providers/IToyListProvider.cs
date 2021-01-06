using DirectOutput.Cab.Toys;
using System;
using System.Collections.Generic;

namespace DirectOutputControls
{
    public interface IToyListProvider
    {
        IEnumerable<IToy> GetToyList(Func<IToy, bool> Match);
    }
}
