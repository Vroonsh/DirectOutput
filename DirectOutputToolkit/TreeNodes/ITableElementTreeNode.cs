using DirectOutput.Table;

namespace DirectOutputToolkit
{
    interface ITableElementTreeNode
    {
        TableElement GetTableElement();
        DirectOutputToolkitHandler.ETableType GetTableType();
        bool HasNoBoolEffects();
    }
}
