namespace Pool
{
    public interface IPoolable
    {
        public void Return();
        public void Release();
    }
}