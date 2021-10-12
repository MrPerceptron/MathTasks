namespace Task5
{
    interface ICondition { }
    class Right : ICondition
    {
        static public bool GetCondition(CircleList<string> arr, string leftItem, string rightItem)
        {
            return arr.IndexOf(leftItem) == arr.IndexOf(rightItem) - 1;
        }
    }
    class Left : ICondition
    {
        static public bool GetCondition(CircleList<string> arr, string leftItem, string rightItem)
        {
            return Right.GetCondition(arr, rightItem, leftItem);
        }
    }
    class Between : ICondition
    {
        static public bool GetCondition(CircleList<string> arr, string leftItem, string middleItem, string rightItem)
        {
            return Right.GetCondition(arr, leftItem, middleItem) == Left.GetCondition(arr, middleItem, rightItem);
        }
    }
}
