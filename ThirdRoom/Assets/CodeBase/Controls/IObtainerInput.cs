namespace CodeBase.Controls
{
    public interface IObtainerInput
    {
        bool IsObtainerReadPressed { get; }
        bool IsObtainerTakePressed { get; }
        bool IsObtainerEscapePressed { get; }
    }
}