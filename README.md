# FiveM

[![CodeFactor](https://www.codefactor.io/repository/github/thalysmarciobn/FiveM/badge/main)](https://www.codefactor.io/repository/github/thalysmarciobn/FiveM/overview/main)

## 🛠️ Construído com

* [FiveM](https://fivem.net) - Framework usado
* [Json.NET](https://www.newtonsoft.com/json) - Framework de alta perfomance JSON
* [EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore) - Framework para simplificar acesso a bancos de dados relacionais no .NET
* [YamlDotNet](https://www.nuget.org/packages/YamlDotNet) - Biblioteca para processar arquivos YAML


## Sistemas

### Clima e Tempo

Sistema incompleto, o servidor terá que escolher um próximo tempo baseado ao anterior, condizendo com a lógica

```csharp
public void Next()
{
	if (AvaiableTransation.TryGetValue(CurrentWeather, out var transition))
	{
		var rand = Random.Next(0, 100);
		WindDirection = Random.Next(0, 7);
		var linq = transition.Where(x => x.Chance >= rand);
		var count = linq.Count();
		if (count == 1)
		{
			var currentTrasation = linq.First();
			CurrentWeather = currentTrasation.To;
		}
		else if (count > 1)
		{
			var chance = Random.Next(0, count);
			var list = linq.ToArray();
			var element = list.ElementAt(chance);
			CurrentWeather = element.To;
		}
		else
		{
			Next();
		}
	}
}
```

### Criação de Personagem

**Client**
```csharp
BaseScript.TriggerServerEvent(EventName.Server.RegisterCharacter, name, lastName, age, slot, appearance,
    new Action<int>(serverStatus =>
    {
        var player = Game.Player;
    
        if (serverStatus == (int)RegisterCharacterEnum.Success)
        {
            player.Character.IsCollisionEnabled = true;
            player.Character.IsPositionFrozen = false;
            player.Character.IsInvincible = false;
    
            player.CanControlCharacter = true;
        }
    
        cb(new { status = serverStatus });
    }));
```
**Server**
```csharp
[EventHandler(EventName.Server.RegisterCharacter)]
public void RegisterCharacter([FromSource] Player player, string name, string lastName, int age, int slot,
    ExpandoObject appearance, NetworkCallbackDelegate networkCallback)
{
    CharacterController.RegisterCharacter(player, name, lastName, age, slot, appearance, networkCallback);
}
```

```csharp
public void RegisterCharacter(Player player, string name, string lastName, int age, int slot,
	ExpandoObject appearance, NetworkCallbackDelegate networkCallback)
{
	Debug.WriteLine($" {name}  {lastName}  {age} {appearance.Count()} ");
	if (age < 18)
	{
		networkCallback.Invoke((int)RegisterCharacterEnum.InvalidAge);
	}

	else
	{
		var license = player.Identifiers["license"];

		if (GameInstance.Instance.GetPlayer(license, out var gamePlayer))
		{
			var account = gamePlayer.Account;

			if (account.Character.Any(x => x.Slot == slot))
			{
				networkCallback.Invoke((int)RegisterCharacterEnum.SlotInUse);
				return;
			}

			var json = JsonHelper.SerializeObject(appearance);
			var data = JsonHelper.DeserializeObject<NUIRegisterCharacter>(json);

			var createCharacter = new AccountCharacterModel
			{
				AccountId = account.Id,
				Slot = slot,
				Name = name,
				Surname = lastName,
				DateCreated = DateTime.Now,
				Model = data.Model,
				Health = data.Model == "mp_m_freemode_01" ? 200 : 100,
				Armor = 0,
				MoneyBalance = 5000,
				BankBalance = 0,
				EyeColorId = data.EyeColor,
				Heading = 330.44f,
				Position = new AccountCharacterPositionModel
				{
					X = -1037.024f,
					Y = -2735.925f,
					Z = 13.7567f
				},
				Rotation = new AccountCharacterRotationModel
				{
					Z = -30.72f
				},
				PedHead = new AccountCharacterPedHeadModel
				{
					HairColorId = data.Hair.Color,
					HairHighlightColor = data.Hair.Highlight
				},
				PedHeadData = new AccountCharacterPedHeadDataModel
				{
					ShapeFirstID = data.HeadBlend.ShapeFirst,
					ShapeSecondID = data.HeadBlend.ShapeSecond,
					SkinFirstID = data.HeadBlend.SkinFirst,
					SkinSecondID = data.HeadBlend.SkinSecond,
					ShapeMix = data.HeadBlend.ShapeMix,
					SkinMix = data.HeadBlend.SkinMix
				},
				PedFace = new List<AccountCharacterPedFaceModel>
				{
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.NoseWidth,
						Scale = data.FaceFeatures.NoseWidth
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.NosePeakHeight,
						Scale = data.FaceFeatures.NosePeakHeight
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.NosePeakLength,
						Scale = data.FaceFeatures.NosePeakSize
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.NoseBoneHeight,
						Scale = data.FaceFeatures.NoseBoneHeight
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.NosePeakLowering,
						Scale = data.FaceFeatures.NosePeakLowering
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.NoseBoneTwist,
						Scale = data.FaceFeatures.NoseBoneTwist
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.EyeBrowHeight,
						Scale = data.FaceFeatures.EyeBrownHeight
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.EyeBrowLength,
						Scale = data.FaceFeatures.EyeBrownForward
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.CheekBoneHeight,
						Scale = data.FaceFeatures.CheeksBoneHeight
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.CheekBoneWidth,
						Scale = data.FaceFeatures.CheeksBoneWidth
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.CheekWidth,
						Scale = data.FaceFeatures.CheeksWidth
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.EyeOpenings,
						Scale = data.FaceFeatures.EyesOpening
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.LipThickness,
						Scale = data.FaceFeatures.LipsThickness
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.JawBoneWidth,
						Scale = data.FaceFeatures.JawBoneWidth
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.JawBoneLength,
						Scale = data.FaceFeatures.JawBoneBackSize
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.ChinBoneLowering,
						Scale = data.FaceFeatures.ChinBoneLowering
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.ChinBoneLength,
						Scale = data.FaceFeatures.ChinBoneLenght
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.ChinBoneWidth,
						Scale = data.FaceFeatures.ChinBoneSize
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.ChinDimple,
						Scale = data.FaceFeatures.ChinHole
					},
					new AccountCharacterPedFaceModel
					{
						Index = FaceShapeEnum.NeckThickness,
						Scale = data.FaceFeatures.NeckThickness
					}
				},
				PedComponent = new List<AccountCharacterPedComponentModel>
				{
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.Head,
						DrawableId = data.Components.Face.DrawableId,
						TextureId = data.Components.Face.TextureId,
						PalleteId = data.Components.Face.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.Masks,
						DrawableId = data.Components.Mask.DrawableId,
						TextureId = data.Components.Mask.TextureId,
						PalleteId = data.Components.Mask.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.HairStyles,
						DrawableId = data.Components.Hair.DrawableId,
						TextureId = data.Components.Hair.TextureId,
						PalleteId = data.Components.Hair.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.Torsos,
						DrawableId = data.Components.Torso.DrawableId,
						TextureId = data.Components.Torso.TextureId,
						PalleteId = data.Components.Torso.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.Legs,
						DrawableId = data.Components.Legs.DrawableId,
						TextureId = data.Components.Legs.TextureId,
						PalleteId = data.Components.Legs.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.BagsNParachutes,
						DrawableId = data.Components.Bag.DrawableId,
						TextureId = data.Components.Bag.TextureId,
						PalleteId = data.Components.Bag.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.Shoes,
						DrawableId = data.Components.Shoes.DrawableId,
						TextureId = data.Components.Shoes.TextureId,
						PalleteId = data.Components.Shoes.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.Accessories,
						DrawableId = data.Components.Accessory.DrawableId,
						TextureId = data.Components.Accessory.TextureId,
						PalleteId = data.Components.Accessory.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.Undershirts,
						DrawableId = data.Components.Undershirt.DrawableId,
						TextureId = data.Components.Undershirt.TextureId,
						PalleteId = data.Components.Undershirt.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.BodyArmors,
						DrawableId = data.Components.Kevlar.DrawableId,
						TextureId = data.Components.Kevlar.TextureId,
						PalleteId = data.Components.Kevlar.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.Decals,
						DrawableId = data.Components.Badge.DrawableId,
						TextureId = data.Components.Badge.TextureId,
						PalleteId = data.Components.Badge.PalleteId
					},
					new AccountCharacterPedComponentModel
					{
						ComponentId = ComponentVariationEnum.Tops,
						DrawableId = data.Components.Torso2.DrawableId,
						TextureId = data.Components.Torso2.TextureId,
						PalleteId = data.Components.Torso2.PalleteId
					}
				},
				PedProp = new List<AccountCharacterPedPropModel>
				{
					new AccountCharacterPedPropModel
					{
						PropId = PropVariationEnum.Hats,
						ComponentId = data.Props.Hat.ComponentId,
						DrawableId = data.Props.Hat.DrawableId,
						TextureId = data.Props.Hat.TextureId,
						Attach = data.Props.Hat.Attach
					},
					new AccountCharacterPedPropModel
					{
						PropId = PropVariationEnum.Glasses,
						ComponentId = data.Props.Glasses.ComponentId,
						DrawableId = data.Props.Glasses.DrawableId,
						TextureId = data.Props.Glasses.TextureId,
						Attach = data.Props.Glasses.Attach
					},
					new AccountCharacterPedPropModel
					{
						PropId = PropVariationEnum.EarPieces,
						ComponentId = data.Props.EarPieces.ComponentId,
						DrawableId = data.Props.EarPieces.DrawableId,
						TextureId = data.Props.EarPieces.TextureId,
						Attach = data.Props.EarPieces.Attach
					},
					new AccountCharacterPedPropModel
					{
						PropId = PropVariationEnum.Watches,
						ComponentId = data.Props.Watches.ComponentId,
						DrawableId = data.Props.Watches.DrawableId,
						TextureId = data.Props.Watches.TextureId,
						Attach = data.Props.Watches.Attach
					},
					new AccountCharacterPedPropModel
					{
						PropId = PropVariationEnum.Wristbands,
						ComponentId = data.Props.Wristbands.ComponentId,
						DrawableId = data.Props.Wristbands.DrawableId,
						TextureId = data.Props.Wristbands.TextureId,
						Attach = data.Props.Wristbands.Attach
					}
				},
				PedHeadOverlay = new List<AccountCharacterPedHeadOverlayModel>
				{
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.Blemishes,
						Index = data.HeadOverlays.Blemishes.Overlay,
						Opacity = data.HeadOverlays.Blemishes.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.Beard,
						Index = data.HeadOverlays.Beard.Overlay,
						Opacity = data.HeadOverlays.Beard.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.Eyebrows,
						Index = data.HeadOverlays.Eyebrows.Overlay,
						Opacity = data.HeadOverlays.Eyebrows.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.Ageing,
						Index = data.HeadOverlays.Ageing.Overlay,
						Opacity = data.HeadOverlays.Ageing.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.Makeup,
						Index = data.HeadOverlays.MakeUp.Overlay,
						Opacity = data.HeadOverlays.MakeUp.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.Blush,
						Index = data.HeadOverlays.Blush.Overlay,
						Opacity = data.HeadOverlays.Blush.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.Complexion,
						Index = data.HeadOverlays.Complexion.Overlay,
						Opacity = data.HeadOverlays.Complexion.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.SunDamage,
						Index = data.HeadOverlays.SunDamage.Overlay,
						Opacity = data.HeadOverlays.SunDamage.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.Lipstick,
						Index = data.HeadOverlays.Lipstick.Overlay,
						Opacity = data.HeadOverlays.Lipstick.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.MoleAndFreckles,
						Index = data.HeadOverlays.MoleAndFreckles.Overlay,
						Opacity = data.HeadOverlays.MoleAndFreckles.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.ChestHair,
						Index = data.HeadOverlays.ChestHair.Overlay,
						Opacity = data.HeadOverlays.ChestHair.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.BodyBlemishes,
						Index = data.HeadOverlays.BodyBlemishes.Overlay,
						Opacity = data.HeadOverlays.BodyBlemishes.Opacity
					},
					new AccountCharacterPedHeadOverlayModel
					{
						OverlayId = OverlayEnum.AddBodyBlemishes,
						Index = data.HeadOverlays.AddBodyBlemishes.Overlay,
						Opacity = data.HeadOverlays.AddBodyBlemishes.Opacity
					}
				},
				PedHeadOverlayColor = new List<AccountCharacterPedHeadOverlayColorModel>
				{
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.Blemishes,
						ColorType = data.HeadOverlayColors.Blemishes.ColorType,
						ColorId = data.HeadOverlayColors.Blemishes.ColorId,
						SecondColorId = data.HeadOverlayColors.Blemishes.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.Beard,
						ColorType = data.HeadOverlayColors.Beard.ColorType,
						ColorId = data.HeadOverlayColors.Beard.ColorId,
						SecondColorId = data.HeadOverlayColors.Beard.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.Eyebrows,
						ColorType = data.HeadOverlayColors.Eyebrows.ColorType,
						ColorId = data.HeadOverlayColors.Eyebrows.ColorId,
						SecondColorId = data.HeadOverlayColors.Eyebrows.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.Ageing,
						ColorType = data.HeadOverlayColors.Ageing.ColorType,
						ColorId = data.HeadOverlayColors.Ageing.ColorId,
						SecondColorId = data.HeadOverlayColors.Ageing.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.Makeup,
						ColorType = data.HeadOverlayColors.MakeUp.ColorType,
						ColorId = data.HeadOverlayColors.MakeUp.ColorId,
						SecondColorId = data.HeadOverlayColors.MakeUp.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.Blush,
						ColorType = data.HeadOverlayColors.Blush.ColorType,
						ColorId = data.HeadOverlayColors.Blush.ColorId,
						SecondColorId = data.HeadOverlayColors.Blush.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.Complexion,
						ColorType = data.HeadOverlayColors.Complexion.ColorType,
						ColorId = data.HeadOverlayColors.Complexion.ColorId,
						SecondColorId = data.HeadOverlayColors.Complexion.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.SunDamage,
						ColorType = data.HeadOverlayColors.SunDamage.ColorType,
						ColorId = data.HeadOverlayColors.SunDamage.ColorId,
						SecondColorId = data.HeadOverlayColors.SunDamage.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.Lipstick,
						ColorType = data.HeadOverlayColors.Lipstick.ColorType,
						ColorId = data.HeadOverlayColors.Lipstick.ColorId,
						SecondColorId = data.HeadOverlayColors.Lipstick.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.MoleAndFreckles,
						ColorType = data.HeadOverlayColors.MoleAndFreckles.ColorType,
						ColorId = data.HeadOverlayColors.MoleAndFreckles.ColorId,
						SecondColorId = data.HeadOverlayColors.MoleAndFreckles.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.ChestHair,
						ColorType = data.HeadOverlayColors.ChestHair.ColorType,
						ColorId = data.HeadOverlayColors.ChestHair.ColorId,
						SecondColorId = data.HeadOverlayColors.ChestHair.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.BodyBlemishes,
						ColorType = data.HeadOverlayColors.BodyBlemishes.ColorType,
						ColorId = data.HeadOverlayColors.BodyBlemishes.ColorId,
						SecondColorId = data.HeadOverlayColors.BodyBlemishes.SecondColorId
					},
					new AccountCharacterPedHeadOverlayColorModel
					{
						OverlayId = OverlayEnum.AddBodyBlemishes,
						ColorType = data.HeadOverlayColors.AddBodyBlemishes.ColorType,
						ColorId = data.HeadOverlayColors.AddBodyBlemishes.ColorId,
						SecondColorId = data.HeadOverlayColors.AddBodyBlemishes.SecondColorId
					}
				}
			};

			using (var context = DatabaseContextManager.Context)
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						account.Character.Add(createCharacter);
						context.Update(account);

						if (context.SaveChanges() > 0)
						{

							account.CurrentCharacter = createCharacter.Id;

							context.Update(account);
							context.SaveChanges();
						}
						networkCallback.Invoke((int)RegisterCharacterEnum.Success);

						transaction.Commit();
					}
					catch
					{
						transaction.Rollback();

						account.Character.Remove(createCharacter);

						networkCallback.Invoke((int)RegisterCharacterEnum.Fail);
					}
				}
			}
		}
	}
}
```


## HUD

### Exibição da crição da HUD em VueJS

![alt text](https://raw.githubusercontent.com/thalysmarciobn/FiveM/main/hud1.png)
![alt text](https://raw.githubusercontent.com/thalysmarciobn/FiveM/main/hud2.png)
![alt text](https://raw.githubusercontent.com/thalysmarciobn/FiveM/main/hud3.png)
![alt text](https://raw.githubusercontent.com/thalysmarciobn/FiveM/main/hud4.png)

**Client**
```csharp
NuiHelper.SendMessage("interface", "world", new object[]
{
	G_World.Weather,
	G_World.RainLevel,
	G_World.WindSpeed,
	G_World.WindDirection,
	G_World.CurrentTime.Hours,
	G_World.CurrentTime.Minutes,
	G_World.CurrentTime.Seconds
});
```

**UI**
```typescript
import { ref } from "vue"
import Hud from "./components/Hud.vue"
import Creation from "./components/Creation.vue"
import Panel from "./components/Panel.vue"

import { useApp } from "./stores/useApp"
import { useHud } from "./stores/useHud"
import { useCreation } from "./stores/useCreation"
import { usePanel } from "./stores/usePanel"

import { useNotification } from "@kyvg/vue3-notification"

const appStore        = useApp()
const appHud          = useHud()
const appCreation     = useCreation()
const appPanel        = usePanel()

const { notify }      = useNotification()

window.addEventListener("message", (event) => {

const action: string    = event.data.Action
const key: string       = event.data.Key
const params: any[]     = event.data.Params

if (action === 'interface') {
  switch (key) {
    case 'notification':
      const s_type: string    =  params[0] as string
      const s_message: string =  params[1] as string
      notify({
        type: s_type,
        text: s_message
      })
      break;
    case 'world':
      appHud.weather            = params[0] as number
      appHud.rainLevel          = params[1] as number
      appHud.windSpeed          = params[2] as number
      appHud.windDirecion       = params[3] as number
      appHud.hour               = params[4] as number
      appHud.minute             = params[5] as number
      appHud.second             = params[6] as number
      break
    case 'panel':
      appPanel.display          = params[0] as string
      break
    case 'creation':
      appCreation.display       = params[0] as string
      appStore.charSlot         = params[1] as number
      break
  }
}
});
```
