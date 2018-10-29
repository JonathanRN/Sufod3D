namespace WeSoLit.Perso1
{
	public class Fireball : Ability
	{
		private int range = 3;
		private int apCost = 3;
		private bool isAffectedByPlayerRange = true;
	
		public override int Range
		{
			get { return range; }
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
