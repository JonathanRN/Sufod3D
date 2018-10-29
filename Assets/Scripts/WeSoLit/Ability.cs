using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
	public abstract int Range { get; }
	public abstract int Damage { get; }
	public abstract int ApCost { get; }
	public abstract bool IsAffectedByPlayerRange { get; }

}
