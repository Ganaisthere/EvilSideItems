namespace EvilSideItems.CustomItems
{
    public class ITM_BrokenMap : Item
    {
        public override bool Use(PlayerManager pm)
        {
            int points = pm.ec.map.TotalFoundCells;
            Singleton<CoreGameManager>.Instance.AddPoints(points, pm.playerNumber, true, false, true);
            if (points > 0)
            {
                pm.ec.map.NotCompleteMapIThink();
            }
            return true;
        }
    }
}
