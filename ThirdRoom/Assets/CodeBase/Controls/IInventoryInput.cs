namespace CodeBase.Controls
{
    public interface IInventoryInput
    {
        bool IsNextButtonPressed { get; }
        bool IsPreviousButtonPressed { get; }
        bool IsCloseInventoryPressed { get; }
        bool IsSelectInventoryPressed { get; }
    }
}