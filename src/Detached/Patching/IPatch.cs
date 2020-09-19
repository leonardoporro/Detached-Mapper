namespace Detached.Patching
{
    public interface IPatch
    {
        void Reset();

        bool IsSet(string name);
    } 
}