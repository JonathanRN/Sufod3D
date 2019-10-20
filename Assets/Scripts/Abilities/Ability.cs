namespace WeSoLit
{
	public abstract class Ability
	{
		public abstract string Name { get; }
		public abstract int Range { get; }
		public abstract int AOE { get; }
		public abstract int Damage { get; }
		public abstract int ApCost { get; }
		public abstract int Priority { get; }
		public abstract bool IsAffectedByPlayerRange { get; }
	}
}
