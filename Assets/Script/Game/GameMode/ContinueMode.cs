using UnityEngine;

[CreateAssetMenu(fileName = "New ContinueMode", menuName = "GameMode/ContinueMode")]
class ContinueMode: CardMode
{
    public override void ShuffleCards(RuntimeGameData runtimeGameData)
    {
        var cards = runtimeGameData.cards;
        
       runtimeGameData.LoadCardData();
    }
}







