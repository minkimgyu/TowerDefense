using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public struct CardData
{
    // ī�� �̸�
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

    [JsonProperty("nameToSpawn")] Entity.Name _nameToSpawn; // ������ų Entity �̸�
    [JsonProperty("areaData")] AreaData _areaData; // ������ų ���� ũ��

    [JsonProperty("cardName")] string _cardName; // ī�� �̸�
    [JsonProperty("cardDescription")] string _cardDescription; // ī�� ����

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

    [JsonIgnore] static public CardData GunnerSpawnCardData { get => new CardData(Entity.Name.Gunner, AreaData.TwoXTwo, "����", "���Ÿ����� ���� �����Ͽ� ������ ���ظ� ������ �����Դϴ�."); }
    [JsonIgnore] static public CardData GridMissileTowerSpawnCardData { get => new CardData(Entity.Name.GridMissileTower, AreaData.TwoXTwo, "�̻��� ��ž", "���� ������ �̻����� �߻��� ���� ��������� Ÿ���Դϴ�."); }
    [JsonIgnore] static public CardData BulletTowerSpawnCardData { get => new CardData(Entity.Name.BulletTower, AreaData.ThreeXTwo, "���� ������", " ������ źȯ�� �߻��Ͽ� ���� ����� ���� �����ϴ� Ÿ���Դϴ�."); }
    [JsonIgnore] static public CardData GuidedMissileTowerSpawnCardData { get => new CardData(Entity.Name.GuidedMissileTower, AreaData.ThreeXTwo, "���� �̻��� ��ž", "���� �̻����� �߻��Ͽ� �ָ� ������ ���� �����ϴ� Ÿ���Դϴ�."); }
    [JsonIgnore] static public CardData ThrowTowerSpawnCardData { get => new CardData(Entity.Name.ThrowTower, AreaData.TwoXTwo, "��ô Ÿ��", "��ź�� ���� ������ ���� ���ظ� ������ Ÿ���Դϴ�."); }
    [JsonIgnore] static public CardData ProvocationWarriorSpawnCardData { get => new CardData(Entity.Name.ProvocationWarrior, AreaData.TwoXTwo, "���� ���� ����", "���� ���� �ɷ��� ���� �����Դϴ�."); }
    [JsonIgnore] static public CardData ProvocationGolemSpawnCardData { get => new CardData(Entity.Name.ProvocationGolem, AreaData.ThreeXThree, "���� ��", "���� ���� �ɷ��� ���� ���Դϴ�."); }
    [JsonIgnore] static public CardData SparkTowerSpawnCardData { get => new CardData(Entity.Name.SparkTower, AreaData.TwoXThree, "���� �����", "���⸦ �����Ͽ� �ֺ��� ���� ������ ���ظ� ������ Ÿ���Դϴ�."); }
    [JsonIgnore] static public CardData LaserTowerSpawnCardData { get => new CardData(Entity.Name.LaserTower, AreaData.ThreeXThree, "������ ��ž", "������ ������ ������ ���� �����Ͽ� ���ظ� ������ Ÿ���Դϴ�"); }
}
