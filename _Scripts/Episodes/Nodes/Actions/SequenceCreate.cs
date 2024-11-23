

using ParadoxNotion.Design;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using CustomInspector;

namespace NodeCanvas.Tasks.Actions
{
    [Category("THING")]
    public class SequenceCreate : ActionTask
    {
        public SequenceProfile sequenceProfile;

        protected override string info
        {
            get => $"Create [ {sequenceProfile?.Alias ?? "-"} ]";
        }

        protected override void OnExecute()
        {
            base.OnExecute();

            EpisodeEvent.I.TriggerSequenceCreate(sequenceProfile);

            GlobalData.I.journeyProfile.AddLog(LogTypes.EP_SEQUENCE, new SerializableSortedDictionary<string, string>()
            {
                { LogKeys.SequenceKey, GetCurrentState().name },
                { LogKeys.SequenceDescription, string.IsNullOrWhiteSpace(sequenceProfile.Description) ? null : sequenceProfile.Description }
            });

            EndAction(true);
        }

        public FSMState GetCurrentState()
        {
            FSMOwner owner = ownerSystemAgent.GetComponent<FSMOwner>();
            if (owner == null)
                return null;

            FSMState currentState = owner.GetCurrentState() as FSMState;
            if (currentState == null)
                return null;

            return currentState;
        }
    }
}