namespace vega.Core
{
    enum StateAction {
        NextState = 1,
        PrevState,
        Reset,
        Terminate,
        Archive
    }

    static class StringConstants {
        public const string SurveyorsInitials = "SI";
        public const string DrawersInitials = "DI";
        public const char IDil = ':';

    }
}