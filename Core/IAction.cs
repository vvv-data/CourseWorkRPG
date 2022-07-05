namespace RPG.Core
{
    public interface IAction
    {
        void Cancel(); // функция будет при вызове выполнять определения которые ей давали в разных файлах
    }
}
