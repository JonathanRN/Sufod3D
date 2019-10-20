using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Unit Stats")]
public class UnitStats : ScriptableObject
{
	public int Hp, MaxHp, Mp, MaxMp, Ap, MaxAp;
	public int Spd;

	public bool IsDead => Hp <= 0;

	public void ResetAll()
	{
		Hp = MaxHp;
		NewTurn();
	}

	public void NewTurn()
	{
		Mp = MaxMp;
		Ap = MaxAp;
	}
}
