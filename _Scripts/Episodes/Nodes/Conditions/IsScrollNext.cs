using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions
{

	[Category("THING")]
	public class IsScrollNext : ConditionTask
	{

		[Space]
		public bool deletePrev = true;
		
		private bool _isscrolled = false;


        protected override string info
        {
            get 
			{
				string delprev = deletePrev ? ", [ DEL ]" : "";
				return $"SCROLLED {delprev}";
			}
        }

        protected override void OnEnable()
		{
            _isscrolled = false;
            EpisodeEvent.I.eventSequenceReachedBottom += oneventSequenceReachedBottom;
        }

		protected override void OnDisable()
		{
            _isscrolled = false;
            EpisodeEvent.I.eventSequenceReachedBottom -= oneventSequenceReachedBottom;
        }

		protected void oneventSequenceReachedBottom(bool reached, SequenceProfile profile)
		{
			_isscrolled = true;

			if (deletePrev)
				EpisodeEvent.I.TriggerSequenceDestroy();
		}


		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
			return _isscrolled;
		}

	}
}