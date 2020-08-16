namespace Detached.Patch
{
    public interface IPatch
    {
        void Reset();

        bool IsSet(string name);
    } 
}