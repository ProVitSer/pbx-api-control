namespace PbxApiControl.Services
{
    public static class ServiceConstants
    {
        public const string ExtensionNotFound = "Добавочный номер не найден";
        public const string ExtensionExists = "Добавочный уже существует на АТС";
        public const string DnIsNotExten = "Номер отсутствует в списке внутренних номеров";
        public const string DnIsNotRingGroup = "Номер отсутствует в списке номеров групп вызова";
        public const string RingGroupNotFound = "Номер группы вызова не найден";
        public const string TryParseError =
            "Входная строка имеет неправильный формат или представляет собой число, которое слишком велико или слишком мало для Int32.";
        public const string ContactIdNotFound = "Контакт с переданным Id не найден";
        public const string DnIsNotQueue = "Номер отсутствует в списке номеров очередей";
        public const string QueueNotFound = "Номер группы вызова не найден";
        public const string CallInitSuccess = "Успешная инициализация вызова";
        public const string CallInitError = "Проблемы инициализация вызова";
        public const string CallDropSuccess = "Вызов успешно завершен";
        public const string CallDropError = "Проблемы завершения вызова";
        public const string DestinationNumberNotFound = "Внутренний номера на который нужно выполнить перевод не существует";
        public const string DestinationNumberUnregister = "Внутренний номера на который нужно выполнить перевод не зарегистрирова";
        public const string CallTransferSuccess = "Вызов успешно переадресован";
        public const string CallTransferError = "Проблемы переадресации вызова";
        public const string NoActiveConnection = "Нет активных вызовов по номеру";


    }
}

