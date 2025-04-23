namespace Ciart.Pagomoa.Systems.Dialogue.Commands
{
    public class SleepDialogueCommand : IDialogueCommand
    {
        public void Execute()
        {
            Game.Instance.player?.Respawn();
            Game.Instance.UI.ShowDaySummaryUI();
            Game.Instance.Dialogue.StopStory();
        }
    }
}
