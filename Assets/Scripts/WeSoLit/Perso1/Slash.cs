namespace WeSoLit.Perso1
{
	public class Slash : Ability
	{
		public override string Name { get; } = "Slash";
		
		public override int Range { get; } = 1;

		public override int Damage { get; } = 1;

		public override int ApCost { get; } = 2;

		public override int Priority { get; } = 2;
		
		public override bool IsAffectedByPlayerRange { get; } = false;
	}
}
