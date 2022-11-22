using NFT1Forge.OSY.DataModel;

public class GameplayDataManager : Singleton<GameplayDataManager>
{
    public float NormalWeaponInterval = 0.2f;
    public float RapidNormalWeaponInterval = 0.15f;

    public GameMode CurrentGameMode;

    public ushort ItemGoldValue;
    public float ItemHealthValue;
    public float ItemMagnetValue;
    public float ItemRapidFireValue;
    public float ChestDropRate;
    public float ItemDropRate;
    
    public void Initialize()
    {
        //
    }
}
