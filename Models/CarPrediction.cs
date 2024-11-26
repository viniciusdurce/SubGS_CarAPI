using Microsoft.ML.Data;

namespace SubGS_CarAPI.ML
{
    public class CarPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }
    }
}