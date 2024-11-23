
using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using CustomInspector;


[System.Flags]
public enum SelectFlag
{
    None = 0,
    A = 1 << 0,
    B = 1 << 1,
    C = 1 << 2,
    D = 1 << 3
}

namespace NodeCanvas.Tasks.Conditions
{

	[Category("THING")]
	public class IsSelectNext : ConditionTask
	{

		[Space]
		public bool deletePrev = true;
		
		[Space(5)]
		public SelectFlag selects = SelectFlag.None;

		private bool _isselect = false;


        protected override string info
        {
            get { return string.Format("SELECTED [ {0} ]{1}", selects, deletePrev ? ", [ DEL ]" : ""); }
        }

        protected override void OnEnable()
		{
            _isselect = false;
            EpisodeEvent.I.eventSequenceSelectedNext += oneventSequenceSelectedNext;
        }

		protected override void OnDisable()
		{
            _isselect = false;
            EpisodeEvent.I.eventSequenceSelectedNext -= oneventSequenceSelectedNext;
        }

		protected void oneventSequenceSelectedNext(string question, int selectnum, string answer)
		{
			_isselect = false;

    		SelectFlag flag = (SelectFlag)(1 << (selectnum - 1));

    		if ((selects & flag) != 0)
			{
				_isselect = true;
				
				if (deletePrev)
					EpisodeEvent.I.TriggerSequenceDestroy();

				if (selectnum > 0)
					GlobalData.I.journeyProfile.AddLog(LogTypes.EP_SELECT, new SerializableSortedDictionary<string, string>()
					{
						{ LogKeys.SelectQuestion, question },
						{ LogKeys.SelectNumber, selectnum.ToString() },
						{ LogKeys.SelectAnswer, answer }
					});
			}
		}


		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
			return _isselect;
		}

	}
}