
public class MyPlayer
{
    public enum Roles { CITIZEN, WEREWOLF, CUPID, WITCH, HUNTER };

    public string nickName;
    public Roles role;

    public bool isMayor;
    public string lover;

    MyPlayer(string nickName, Roles role, bool isMayor = false, string lover = null)
    {
        this.nickName = nickName;
        this.role = role;
        this.isMayor = isMayor;
        this.lover = lover;
    }
}
