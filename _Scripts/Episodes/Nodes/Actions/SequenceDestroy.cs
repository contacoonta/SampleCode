
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions
{

	[Category("THING")]
	public class SequenceDestroy : ActionTask
	{

        protected override string info
        {
            get => $"DestroyAll";
        }


        protected override void OnExecute()
        {
            base.OnExecute();

            EpisodeEvent.I.TriggerSequenceDestroy();
            EndAction(true);
        }
    }
}