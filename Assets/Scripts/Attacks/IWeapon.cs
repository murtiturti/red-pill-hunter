namespace Attacks
{
    public interface IWeapon
    {
        int Attack(); // return Animation trigger if success, 0 otherwise

        bool IsSilent();
    }
}
