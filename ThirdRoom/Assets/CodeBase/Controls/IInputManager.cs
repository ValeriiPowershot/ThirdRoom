namespace CodeBase.Controls
{
    public interface IInputManager
    {
        void EnableActionMap(ActionMapType actionMap);
        void DisableActionMap(ActionMapType actionMap);
        void DisableAllActionMaps();
        PlayerInputActions GetPlayerInputActions();
    }
}