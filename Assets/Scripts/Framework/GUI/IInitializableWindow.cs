namespace Framework.GUI
{
    public interface IInitializableWindow<in T> where T : struct
    {
        public void Initialize(T data);
    }
}