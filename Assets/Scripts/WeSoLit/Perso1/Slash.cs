namespace WeSoLit.Perso1
{
	public class Slash : Ability {
	
		private int range = 2;
		private int apCost = 2;
		private bool isAffectedByPlayerRange = false;
		private int damage = 5;

		public override int Range
		{
			get { return range; }
		}
		public override int Damage
		{
			get { return damage; }
		}
		public override int ApCost
		{
			get { return apCost; }
		}
		public override bool IsAffectedByPlayerRange
		{
			get { return isAffectedByPlayerRange; }
		}
	}
}
