public static class DataStore 
{
    public static int budget = 0; //campaign budget

    public static int AttackingIslandID;

    public static bool[] islandControl = new bool[6];

    public static UnitManager brain;

    public static void SetBrain(UnitManager newBrain)
    {
        brain = newBrain;
    }
}
