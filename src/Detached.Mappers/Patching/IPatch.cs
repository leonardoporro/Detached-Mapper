namespace Detached.Mappers.Patching
{
    public interface IPatch
    {
        void Reset();

        bool IsSet(string name);
    } 
}