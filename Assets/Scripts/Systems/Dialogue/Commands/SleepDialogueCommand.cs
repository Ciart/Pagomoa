using Ciart.Pagomoa.Systems.Time;

namespace Ciart.Pagomoa.Systems.Dialogue.Commands
{
    public class SleepDialogueCommand : IDialogueCommand
    {
        public void Execute()
        {
            TimeManager.instance.SkipToNextDay();
        }
    }
}