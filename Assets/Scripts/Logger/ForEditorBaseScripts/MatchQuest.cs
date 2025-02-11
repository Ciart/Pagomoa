using System;

namespace Ciart.Pagomoa.Logger.ForEditorBaseScripts
{
    [Serializable]
    public class MatchQuest
    {
        public string owner = "";
        public string id = "";
    }

    [Serializable]
    public class MatchData
    {
        public MatchQuest[] data;
    }
}