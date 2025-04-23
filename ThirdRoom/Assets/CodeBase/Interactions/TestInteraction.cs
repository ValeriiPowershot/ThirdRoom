using Codebase.Logic.Interactions;

namespace CodeBase.Interactions
{
    public class TestInteraction : InteractObject
    {
        protected override void OnInteract()
        {
            print("Interacted");
        }
    }
}