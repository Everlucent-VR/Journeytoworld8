using Boom;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAddRemoveFromList : MonoBehaviour
{
    //Set this to true if you want to add and remove fruist from a entity's field list in the world's data.
    [SerializeField] bool listInWorld;

    [SerializeField] Button addAppleButton;
    [SerializeField] Button addPineappleButton;
    [SerializeField] Button addWatermelonButton;

    [SerializeField] TMP_Text addAppleText;
    [SerializeField] TMP_Text addPineappleText;
    [SerializeField] TMP_Text addWatermelonText;

    [SerializeField] TMP_Text content;

    private void Awake()
    {
        UserUtil.AddListenerMainDataChange<MainDataTypes.LoginData>(LoginHandler);

        addAppleButton.onClick.AddListener(AppleClickHandler);
        addPineappleButton.onClick.AddListener(PineappleClickHandler);
        addWatermelonButton.onClick.AddListener(WatermelonClickHandler);
    }

    private void OnDestroy()
    {
        UserUtil.RemoveListenerMainDataChange<MainDataTypes.LoginData>(LoginHandler);

        if (listInWorld == false) UserUtil.RemoveListenerDataChangeSelf<DataTypes.Entity>(EntityDataChange);
        else UserUtil.RemoveListenerDataChange<DataTypes.Entity>(EntityDataChange, BoomManager.Instance.WORLD_CANISTER_ID);

        addAppleButton.onClick.RemoveListener(AppleClickHandler);
        addPineappleButton.onClick.RemoveListener(PineappleClickHandler);
        addWatermelonButton.onClick.RemoveListener(WatermelonClickHandler);
    }
    private void LoginHandler(MainDataTypes.LoginData data)
    {
        if (listInWorld == false) UserUtil.AddListenerDataChangeSelf<DataTypes.Entity>(EntityDataChange, new() { invokeOnRegistration = true });
        else UserUtil.AddListenerDataChange<DataTypes.Entity>(EntityDataChange, new() { invokeOnRegistration = true }, BoomManager.Instance.WORLD_CANISTER_ID);
    }

    private void AppleClickHandler()
    {
        TryAddApple();
    }

    private void PineappleClickHandler()
    {
        TryAddPineapple();
    }

    private void WatermelonClickHandler()
    {
        TryAddWatermelon();
    }

    private async void TryAddApple()
    {
        addAppleButton.interactable = false;
        addPineappleButton.interactable = false;
        addWatermelonButton.interactable = false;

        if (addAppleText.text.Contains("Add")) ActionUtil.ProcessAction(listInWorld == false ? "AddFruitApple" : "AddFruitWorldApple", new List<Candid.World.Models.Field>() { new Candid.World.Models.Field("Fruit", "Apple") });
        else ActionUtil.ProcessAction(listInWorld == false ? "RemoveFruitApple" : "RemoveFruitWorldApple", new List<Candid.World.Models.Field>() { new Candid.World.Models.Field("Fruit", "Apple") });
    }

    private async void TryAddPineapple()
    {
        addAppleButton.interactable = false;
        addPineappleButton.interactable = false;
        addWatermelonButton.interactable = false;

        if (addPineappleText.text.Contains("Add")) ActionUtil.ProcessAction(listInWorld == false ? "AddFruitPineapple" : "AddFruitWorldPineapple", new List<Candid.World.Models.Field>() { new Candid.World.Models.Field("Fruit", "Pineapple") });
        else ActionUtil.ProcessAction(listInWorld == false ? "RemoveFruitPineapple" : "RemoveFruitWorldPineapple", new List<Candid.World.Models.Field>() { new Candid.World.Models.Field("Fruit", "Pineapple") });
    }

    private async void TryAddWatermelon()
    {
        addAppleButton.interactable = false;
        addPineappleButton.interactable = false;
        addWatermelonButton.interactable = false;

        if (addWatermelonText.text.Contains("Add")) ActionUtil.ProcessAction(listInWorld == false ? "AddFruitWatermelon" : "AddFruitWorldWatermelon", new List<Candid.World.Models.Field>() { new Candid.World.Models.Field("Fruit", "Watermelon") });
        else ActionUtil.ProcessAction(listInWorld == false ? "RemoveFruitWatermelon" : "RemoveFruitWorldWatermelon", new List<Candid.World.Models.Field>() { new Candid.World.Models.Field("Fruit", "Watermelon") });
    }

    private void EntityDataChange(Data<DataTypes.Entity> data)
    {
        content.text = "";
        addAppleText.text = "Add Apple";
        addPineappleText.text = "Add Pineapple";
        addWatermelonText.text = "Add Watermelon";

        addAppleButton.interactable = true;
        addPineappleButton.interactable = true;
        addWatermelonButton.interactable = true;

        foreach (var entity in data.elements) 
        {
            if(entity.Value.eid == "Fruits")
            {
                entity.Value.fields.TryGetValue("list", out var list);

                var fruits = list.Split(',');

                foreach (var fruit in fruits)
                {
                    content.text += $"{fruit}\n";

                    if(fruit == "Apple") addAppleText.text = "Remove Apple";
                    else if (fruit == "Pineapple") addPineappleText.text = "Remove Pineapple";
                    else if (fruit == "Watermelon") addWatermelonText.text = "Remove Watermelon";
                }

                return;
            }
        }
    }

}
