using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public struct CardData
{
    // 카드 이름
    [SerializeField]
    public enum Name
    {
        GunnerSpawnCard,
        GridMissileTowerSpawnCard,
        BulletTowerSpawnCard,
        GuidedMissileTowerSpawnCard,
        ThrowTowerSpawnCard,
        ProvocationWarriorSpawnCard,
        ProvocationGolemSpawnCard,
        SparkTowerSpawnCard,
        LaserTowerSpawnCard
    }

    [JsonProperty("nameToSpawn")] Entity.Name _nameToSpawn; // 스폰시킬 Entity 이름
    [JsonProperty("areaData")] AreaData _areaData; // 스폰시킬 영역 크기

    [JsonProperty("cardName")] string _cardName; // 카드 이름
    [JsonProperty("cardDescription")] string _cardDescription; // 카드 설명

    [JsonIgnore] public Entity.Name NameToSpawn { get => _nameToSpawn; set => _nameToSpawn = value; }
    [JsonIgnore] public AreaData AreaData { get => _areaData; set => _areaData = value; }
    [JsonIgnore] public string CardName { get => _cardName; set => _cardName = value; }
    [JsonIgnore] public string CardDescription { get => _cardDescription; set => _cardDescription = value; }

    public CardData(
        Entity.Name nameToSpawn,
        AreaData areaData,
        string cardName,
        string cardDescription)
    {
        _nameToSpawn = nameToSpawn;
        _areaData = areaData;
        _cardName = cardName;
        _cardDescription = cardDescription;
    }

    [JsonIgnore] static public CardData GunnerSpawnCardData { get => new CardData(Entity.Name.Gunner, AreaData.TwoXTwo, "슈터", "원거리에서 총을 난사하여 적에게 피해를 입히는 유닛입니다."); }
    [JsonIgnore] static public CardData GridMissileTowerSpawnCardData { get => new CardData(Entity.Name.GridMissileTower, AreaData.TwoXTwo, "미사일 포탑", "일정 범위에 미사일을 발사해 적을 쓸어버리는 타워입니다."); }
    [JsonIgnore] static public CardData BulletTowerSpawnCardData { get => new CardData(Entity.Name.BulletTower, AreaData.ThreeXTwo, "정밀 조준포", " 빠르게 탄환을 발사하여 단일 대상을 집중 공격하는 타워입니다."); }
    [JsonIgnore] static public CardData GuidedMissileTowerSpawnCardData { get => new CardData(Entity.Name.GuidedMissileTower, AreaData.ThreeXTwo, "유도 미사일 포탑", "유도 미사일을 발사하여 멀리 떨어진 적을 공격하는 타워입니다."); }
    [JsonIgnore] static public CardData ThrowTowerSpawnCardData { get => new CardData(Entity.Name.ThrowTower, AreaData.TwoXTwo, "투척 타워", "폭탄을 던져 강력한 광역 피해를 입히는 타워입니다."); }
    [JsonIgnore] static public CardData ProvocationWarriorSpawnCardData { get => new CardData(Entity.Name.ProvocationWarrior, AreaData.TwoXTwo, "도발 방패 전사", "적을 도발 능력을 가진 전사입니다."); }
    [JsonIgnore] static public CardData ProvocationGolemSpawnCardData { get => new CardData(Entity.Name.ProvocationGolem, AreaData.ThreeXThree, "도발 골렘", "적을 도발 능력을 가진 골램입니다."); }
    [JsonIgnore] static public CardData SparkTowerSpawnCardData { get => new CardData(Entity.Name.SparkTower, AreaData.TwoXThree, "번개 방출기", "전기를 방출하여 주변의 여러 적에게 피해를 입히는 타워입니다."); }
    [JsonIgnore] static public CardData LaserTowerSpawnCardData { get => new CardData(Entity.Name.LaserTower, AreaData.ThreeXThree, "레이저 포탑", "강력한 레이저 빔으로 적을 관통하여 피해를 입히는 타워입니다"); }
}
