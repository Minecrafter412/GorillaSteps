using GorillaTagModTemplateProject.Patches;

namespace GorillaTagModTemplateProject.Scripts
{
    [System.Serializable]
    internal class PlayerData
    {
        public int steps;

        internal PlayerData()
        {
            steps = HandTapPatch.stepsCount;
        }
    }
}
