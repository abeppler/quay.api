namespace Wms.ProductionLine.CrossCutting
{
    public class EnumModel<T>
    {
        protected EnumModel()
        {
        }

        public EnumModel(T value, string label)
        {
            Value = value;
            Label = label;
        }

        public T Value { get; set; }
        public string Label { get; set; }
    }
}
