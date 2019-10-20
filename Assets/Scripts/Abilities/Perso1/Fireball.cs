namespace WeSoLit.Perso1
{
	public class Fireball : Ability
	{
		public override string Name { get; } = "Fireball";
		
		public override int Range { get; } = 3;

		public override int AOE { get; } = 2;

		public override int Damage { get; } = 3;

		public override int ApCost { get; } = 3;

		public override int Priority { get; } = 1;
		
		public override bool IsAffectedByPlayerRange { get; } = true;
	}
}
