using System;
using NFT1Forge.OSY.JsonModel;

[Serializable]
public class ActiveObjectRequestModel: BaseRequestModel
{
    public string active_pilot;
    public string active_spaceship;
    public string active_weapon;
    public string active_chest;
    public string active_pass;

    public ActiveObjectRequestModel(string active_pilot, string active_spaceship, string active_weapon, string active_chest, string active_pass)
    {
        this.active_pilot = active_pilot;
        this.active_spaceship = active_spaceship;
        this.active_weapon = active_weapon;
        this.active_chest = active_chest;
        this.active_pass = active_pass;
    }

    public ActiveObjectRequestModel(GetActiveObjectModel.ActiveObjectDataModel model)
    {
        this.active_pilot = model.active_pilot;
        this.active_spaceship = model.active_spaceship;
        this.active_weapon = model.active_weapon;
        this.active_chest = model.active_chest;
        this.active_pass = model.active_pass;
    }
}
