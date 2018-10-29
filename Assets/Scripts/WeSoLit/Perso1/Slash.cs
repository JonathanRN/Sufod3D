namespace WeSoLit.Perso1
{
	public class Slash : Ability {
	
		private int range = 2;
		private int apCost = 2;
		private bool isAffectedByPlayerRange = false;
	
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
