namespace PbxApiControl.Services
{
    public static class ServiceConstants
    {
        public const string ExtensionNotFound = "Добавочный номер не найден";
        public const string ExtensionExists = "Добавочный уже существует на АТС";
        public const string DnIsNotExten = "DN is not of type Extension";
        public const string DnIsNotRingGroup = "DN is not of type RingGroup";
        public const string RingGroupNotFound = "Номер группы вызова не найден";

        public const string TryParseError =
            "Input string is not in a correct format or represents a number that is too large or too small for an Int32.";
        public const string ContactIdNotFound = "Контакт с переданным Id не найден";

    }
}

