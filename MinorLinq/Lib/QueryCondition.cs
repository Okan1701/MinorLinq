namespace MinorLinq.Lib 
{
    public enum OperatorType 
    {
        Equal,
        GreaterThan,
        LessThan
    }

    public enum ConditionLinkedListPrefix
    {
        And,
        Or,
        None
    }

    public class QueryCondition
    {
        public string Left { get; set; }
        public string Right { get; set; }
        public OperatorType OperatorType { get; set; }
    }

    public class ConditionLinkedList 
    {
        public ConditionLinkedListPrefix PrefixOperator { get; set; } = ConditionLinkedListPrefix.None;
        public QueryCondition Condition { get; set; }
        public ConditionLinkedList NextCondition { get; set; }
    }
}