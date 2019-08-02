namespace DomainModels.Models
{
    public class Rule
    {
        public string Operator { get; set; }
        public ConditionFilter[] Conditions { get; set; }
        public Effect[] Effects { get; set; }
    }
}
